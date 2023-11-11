namespace ProjectDMG.Api.Notification.Structs
{
    public readonly struct MemoryAddressValueUpdate
    {
        public byte[] PreviousValue { get; }
        public byte[] NewValue { get; }

        public MemoryAddressValueUpdate(byte[] previousValue, byte[] newValue)
        {
            PreviousValue = previousValue;
            NewValue = newValue;
        }

    }
}
