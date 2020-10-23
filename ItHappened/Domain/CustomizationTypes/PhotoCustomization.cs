using System;

namespace ItHappened.Domain.CustomizationTypes
{
    public class PhotoCustomization
    {
        public PhotoCustomization(string value)
        {
            Value = value ?? throw new ArgumentNullException("PhotoCustomization");
        }

        public string Value { get; }
    }
}