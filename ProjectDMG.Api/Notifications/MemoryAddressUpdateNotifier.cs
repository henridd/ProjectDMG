using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDMG.Api.Notifications
{
    internal class MemoryAddressUpdateNotifier : IDisposable
    {
        private readonly Dictionary<int, ObservableStack<MemoryAddressUpdatedNotification>> _channels = new();

        public ObservableStack<MemoryAddressUpdatedNotification> AddChannel(int id)
        {
            var observableStack = new ObservableStack<MemoryAddressUpdatedNotification>();

            _channels.Add(id, observableStack);

            return observableStack;
        }

        public void UpdateChannel(int id, AddressRange subscribedAddress, IEnumerable<AddressRangeValue> values)
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

            _channels[id].Push(notification);

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

        public void Dispose()
        {
            _channels.Clear();
        }
    }
}
