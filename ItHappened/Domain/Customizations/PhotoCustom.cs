using System;

namespace ItHappened.Domain.Customizations
{
    public class PhotoCustom
    {
        public PhotoCustom(byte[] value)
        {
            Value = value ?? throw new ArgumentNullException("PhotoCustom");
        }

        public byte[] Value { get; }
    }
}