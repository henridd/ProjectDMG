using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectDMG.Api.Notifications
{
    public interface IMemoryWatcher : IDisposable
    {
        public bool IsDisposed { get; }

        /// <summary>
        /// This method is intentionally async void due to performance issues. 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="newValue"></param>
        void OnMemoryWrittenAsync(ushort address, byte newValue);

        /// <summary>
        /// Add a subscription to the subscribedAddress, so every time a change occurs, a notification will be fired.
        /// </summary>
        /// <param name="subscribedAddress">Address to listen for changes</param>
        /// <param name="relevantAddresses">Other relevant addresses that should be included in the response</param>
        /// <returns></returns>
        ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(AddressRange subscribedAddresses, IEnumerable<AddressRange> relevantAddresses);
    }

    public class MemoryWatcher : IMemoryWatcher
    {
        //TODO Change to ConcurrentDictionary
        private readonly Dictionary<ushort, byte> _currentValues = new();
        private readonly Dictionary<AddressRange, List<MemoryAddressSubscription>> _addressSubscriptions = new();

        private readonly MemoryAddressUpdateNotifier _memoryAddressUpdateNotifier = new();

        private int _lastId = 0;

        public bool IsDisposed { get; private set; }

        internal MemoryWatcher()
        {

        }

        public ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(AddressRange subscribedAddresses, IEnumerable<AddressRange> relevantAddresses)
        {
            if (!_addressSubscriptions.ContainsKey(subscribedAddresses))
                _addressSubscriptions.Add(subscribedAddresses, new List<MemoryAddressSubscription>());

            var subscription = CreateSubscription(subscribedAddresses, relevantAddresses);

            return _memoryAddressUpdateNotifier.AddChannel(subscription.Id);
        }

        private MemoryAddressSubscription CreateSubscription(AddressRange subscribedAddresses, IEnumerable<AddressRange> relevantAddresses)
        {
            var subscription = new MemoryAddressSubscription(_lastId++, subscribedAddresses, relevantAddresses);
            _addressSubscriptions[subscribedAddresses].Add(subscription);
            return subscription;
        }

        public async void OnMemoryWrittenAsync(ushort address, byte newValue)
        {
            _currentValues[address] = newValue;

            foreach (var subscriptionList in _addressSubscriptions.Where(x => x.Key.MemoryAddresses.Contains(address)))
            {
                foreach (var subscription in subscriptionList.Value)
                {
                    var addressesToFetchValues = subscription.RelevantAddresses.ToList();
                    addressesToFetchValues.Add(subscription.SubscribedAddresses);

                    var valuesList = GetAddressValues(subscriptionList, addressesToFetchValues);

                    _memoryAddressUpdateNotifier.UpdateChannel(subscription.Id, subscriptionList.Key, valuesList, address);
                }
            }
        }

        //TODO If performance sucks, optimize by adding address ranges to a local cache 
        private List<AddressRangeValue> GetAddressValues(KeyValuePair<AddressRange, List<MemoryAddressSubscription>> subscriptionList, List<AddressRange> addressesToFetchValues)
        {
            var valuesList = new List<AddressRangeValue>();
            foreach (var addresses in addressesToFetchValues)
            {
                var values = new AddressRangeValue(addresses, addresses.MemoryAddresses.Select(x => GetValueFromCurrentValuesOrDefault(x)));
                valuesList.Add(values);
            }

            return valuesList;
        }

        private byte GetValueFromCurrentValuesOrDefault(ushort x)
        {
            if (!_currentValues.ContainsKey(x))
            {
                return 0;
            }

            return _currentValues[x];
        }

        public void Dispose()
        {
            _addressSubscriptions.Clear();
            _currentValues.Clear();
            _memoryAddressUpdateNotifier.Dispose();
            IsDisposed = true;
        }
    }
}
