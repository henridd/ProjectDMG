using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDMG.Api.Notifications
{
    internal class MemoryAddressUpdateNotifier : IDisposable
    {
        private readonly Dictionary<int, ObservableStack<MemoryAddressUpdatedNotification>> _channels = new();
        private readonly Dictionary<int, QueuedNotification> _queuedNotifications = new();
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

            if (notification.AddressesValues[subscribedAddress].PreviousValue == notification.AddressesValues[subscribedAddress].NewValue)
            {
                return;
            }

            AddNotificationToQueue(id, subscribedAddress, notification, updatedAddress);

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

        private void AddNotificationToQueue(int id, AddressRange subscribedAddress, MemoryAddressUpdatedNotification notification, ushort updatedAddress)
        {
            lock (_lock)
            {
                if(!_queuedNotifications.ContainsKey(id))
                {
                    _queuedNotifications.Add(id, new(id, notification, subscribedAddress));
                }

                _queuedNotifications[id].RemainingAddressesToUpdate.Remove(updatedAddress);

                if(_queuedNotifications[id].RemainingAddressesToUpdate.Count == 0)
                {
                    _channels[id].Push(notification);
                    _queuedNotifications.Remove(id);
                }
            }
        }

        public void Dispose()
        {
            _channels.Clear();
        }

        private readonly struct QueuedNotification
        {
            public int Channel { get; }
            public MemoryAddressUpdatedNotification Notification { get; }
            public HashSet<ushort> RemainingAddressesToUpdate { get; }

            public QueuedNotification(int channel, MemoryAddressUpdatedNotification notification, AddressRange addressRange)
            {
                Channel = channel;
                Notification = notification;
                RemainingAddressesToUpdate = new HashSet<ushort>(addressRange.MemoryAddresses);
            }
        }
    }
}
