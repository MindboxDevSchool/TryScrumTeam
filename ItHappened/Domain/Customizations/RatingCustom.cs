using System;

namespace ItHappened.Domain.Customizations
{
    public class RatingCustom
    {
        public int Value { get; }

        public RatingCustom(int value)
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException("RatingCustom");
            
            Value = value;
        }
    }
}