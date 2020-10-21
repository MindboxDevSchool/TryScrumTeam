using System;

namespace ItHappened.Domain.CustomizationTypes
{
    public class RatingCustomization
    {
        public RatingCustomization(int value)
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException("RatingCustomization");
            
            Value = value;
        }

        public int Value { get; }
    }
}