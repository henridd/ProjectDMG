using System;
using System.Collections.Generic;
using System.Linq;
using ProjectDMG.Api.Models;
using ProjectDMG.Api.Notification.Structs;

namespace ProjectDMG.Api.Notifications
{
    internal class MemoryAddressUpdateNotifier : IDisposable
    {
        private readonly Dictionary<int, ObservableStack<MemoryAddressUpdatedNotification>> _channels = new();
        private readonly Dictionary<int, HashSet<ushort>> _remainingAddressToUpdate = new();
        private object _lock = new object();

        public ObservableStack<MemoryAddressUpdatedNotification> AddChannel(int id)
        {
            var observableStack = new ObservableStack<MemoryAddressUpdatedNotification>();

            _channels.Add(id, observableStack);

            return observableStack;
        }

        public void UpdateChannel(int id, AddressRange subscribedAddress, IEnumerable<AddressRangeValue> values, ushort updatedAddress)
        {
            var notification = new MemoryAddressUpdatedNotification(
                values.ToDictionary(
                    x => x.AddressRange,
                    x => new MemoryAddressValueUpdate(
                        GetPreviousValue(x.AddressRange),
                        x.Values.ToArray())));

            SendNotificationIfAllAddressesUpdated(id, subscribedAddress, notification, updatedAddress);

            byte[] GetPreviousValue(AddressRange address)
            {
                if (_channels[id].Count == 0)
                {
                    return new byte[0];
                }

                var previousNotification = _channels[id].Peek();
                return previousNotification.AddressesValues[address].NewValue;
            }
        }

        private void SendNotificationIfAllAddressesUpdated(int id, AddressRange subscribedAddress, MemoryAddressUpdatedNotification notification, ushort updatedAddress)
        {
            lock (_lock)
            {
                if(!_remainingAddressToUpdate.ContainsKey(id))
                {
                    _remainingAddressToUpdate.Add(id, new(subscribedAddress.MemoryAddresses));
                }

                _remainingAddressToUpdate[id].Remove(updatedAddress);

                if(_remainingAddressToUpdate[id].Count == 0)
                {
                    if (notification.AddressesValues[subscribedAddress].PreviousValue != notification.AddressesValues[subscribedAddress].NewValue)
                    {
                        _channels[id].Push(notification);
                    }

                    _remainingAddressToUpdate.Remove(id);
                }
            }
        }

        public void Dispose()
        {
            _channels.Clear();
        }
    }
}
