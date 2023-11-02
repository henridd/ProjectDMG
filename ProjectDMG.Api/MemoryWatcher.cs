using Prism.Events;
using ProjectDMG.Api.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDMG.Api
{
    public interface IMemoryWatcher
    {
        /// <summary>
        /// This method is intentionally async void due to performance issues. 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="newValue"></param>
        void OnMemoryUpdatedAsync(ushort address, byte newValue);

        /// <summary>
        /// Add a subscription to the subscribedAddress, so every time a change occurs, a notification will be fired.
        /// </summary>
        /// <param name="subscribedAddress">Address to listen for changes</param>
        /// <param name="relevantAddresses">Other relevant addresses that should be included in the response</param>
        /// <returns></returns>
        int AddSubscription(ushort subscribedAddress, IEnumerable<ushort> relevantAddresses);

        int AddSubscription(string subscribedAddressInHex, IEnumerable<ushort> relevantAddresses);
    }

    public class MemoryWatcher : IMemoryWatcher
    {
        //TODO Change to ConcurrentDictionary
        private readonly Dictionary<ushort, byte> _currentValues = new Dictionary<ushort, byte>();
        private readonly Dictionary<ushort, List<MemoryAddressSubscription>> _addressSubscriptions = new Dictionary<ushort, List<MemoryAddressSubscription>>();
        private readonly IEventAggregator _eventAggregator = new EventAggregator();

        private int _lastId = 0;

        internal MemoryWatcher()
        {

        }

        //TODO Accept address range
        public int AddSubscription(string subscribedAddressInHex, IEnumerable<ushort> relevantAddresses)
        {
            var address = Convert.ToUInt16(subscribedAddressInHex, 16);

            return AddSubscription(address, relevantAddresses); 
        }

        public int AddSubscription(ushort subscribedAddress, IEnumerable<ushort> relevantAddresses)
        {
            if (!_addressSubscriptions.ContainsKey(subscribedAddress))
                _addressSubscriptions.Add(subscribedAddress, new List<MemoryAddressSubscription>());

            var subscription = new MemoryAddressSubscription(_lastId++, subscribedAddress, relevantAddresses);
            _addressSubscriptions[subscribedAddress].Add(subscription);

            return subscription.Id;
        }

        public async void OnMemoryUpdatedAsync(ushort address, byte newValue)
        {
            _currentValues[address] = newValue;

            if (!_addressSubscriptions.ContainsKey(address))
                return;

            foreach (var subscription in _addressSubscriptions[address])
            {
                var values = _currentValues.Where(x => subscription.RelevantAddresses.Contains(x.Key) || subscription.SubscribedAddress == x.Key);
                _eventAggregator.GetEvent<MemoryAddressUpdatedEvent>().Publish(new MemoryAddressUpdatedPayload(values.ToDictionary(x => x.Key, x => x.Value)));
            }
        }
    }
}
