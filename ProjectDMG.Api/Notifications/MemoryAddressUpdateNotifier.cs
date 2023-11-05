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

        public void UpdateChannel(int id, ushort subscribedAddress, IEnumerable<KeyValuePair<ushort, byte>> values)
        {
            var notification = new MemoryAddressUpdatedNotification(
                values.ToDictionary(
                    x => x.Key.ToString("X"), 
                    x => new MemoryAddressValueUpdate(
                        GetPreviousValue(x.Key),
                        x.Value)));

            var subscribedAddressHex = subscribedAddress.ToString("X");
            if (notification.AddressesValues[subscribedAddressHex].PreviousValue == notification.AddressesValues[subscribedAddressHex].NewValue)
            {
                return;
            }

            _channels[id].Push(notification);

            byte GetPreviousValue(ushort address)
            {
                if (_channels[id].Count == 0)
                {
                    return 0;    
                }

                var previousNotification = _channels[id].Peek();
                return previousNotification.AddressesValues[address.ToString("X")].NewValue;
            }
        }

        public void Dispose()
        {
            _channels.Clear();
        }
    }
}
