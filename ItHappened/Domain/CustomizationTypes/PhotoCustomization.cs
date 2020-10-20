using System;

namespace ItHappened.Domain.CustomizationTypes
{
    public class PhotoCustomization
    {
        public PhotoCustomization(byte[] value)
        {
            Value = value ?? throw new ArgumentNullException("PhotoCustomization");
        }

        public byte[] Value { get; }
    }
}