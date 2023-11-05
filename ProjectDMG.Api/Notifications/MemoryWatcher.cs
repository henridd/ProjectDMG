using System;
using System.Collections.Generic;
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
        ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(ushort subscribedAddress, IEnumerable<ushort> relevantAddresses);

        ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(string subscribedAddressInHex, IEnumerable<ushort> relevantAddresses);

        void OnCycleFinished();
    }

    public class MemoryWatcher : IMemoryWatcher
    {
        //TODO Change to ConcurrentDictionary
        private readonly Dictionary<ushort, byte> _currentValues = new();
        private readonly Dictionary<ushort, List<MemoryAddressSubscription>> _addressSubscriptions = new();

        private readonly MemoryAddressUpdateNotifier _memoryAddressUpdateNotifier = new();

        private int _lastId = 0;

        public bool IsDisposed { get; private set;  }

        internal MemoryWatcher()
        {

        }

        //TODO Accept address range
        public ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(string subscribedAddressInHex, IEnumerable<ushort> relevantAddresses)
        {
            var address = Convert.ToUInt16(subscribedAddressInHex, 16);

            return AddSubscription(address, relevantAddresses);
        }

        public ObservableStack<MemoryAddressUpdatedNotification> AddSubscription(ushort subscribedAddress, IEnumerable<ushort> relevantAddresses)
        {
            if (!_addressSubscriptions.ContainsKey(subscribedAddress))
                _addressSubscriptions.Add(subscribedAddress, new List<MemoryAddressSubscription>());

            var subscription = CreateSubscription(subscribedAddress, relevantAddresses);

            return _memoryAddressUpdateNotifier.AddChannel(subscription.Id);
        }

        private MemoryAddressSubscription CreateSubscription(ushort subscribedAddress, IEnumerable<ushort> relevantAddresses)
        {
            var subscription = new MemoryAddressSubscription(_lastId++, subscribedAddress, relevantAddresses);
            _addressSubscriptions[subscribedAddress].Add(subscription);
            return subscription;
        }

        public async void OnMemoryWrittenAsync(ushort address, byte newValue)
        {
            _currentValues[address] = newValue;

            if (!_addressSubscriptions.ContainsKey(address))
                return;

            foreach (var subscription in _addressSubscriptions[address])
            {
                var values = _currentValues.Where(x => subscription.RelevantAddresses.Contains(x.Key) || subscription.SubscribedAddress == x.Key);

                _memoryAddressUpdateNotifier.UpdateChannel(subscription.Id, subscription.SubscribedAddress, values);
            }
        }

        public void OnCycleFinished()
        {
            _memoryAddressUpdateNotifier.OnCycleFinished();
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
