using System;
using System.Collections.Generic;
using ProjectDMG.Api.Models;
using ProjectDMG.Api.Notification.Structs;

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
}
