using System;

namespace ItHappened.Domain.Customizations
{
    public class RatingCustom
    {
        public RatingCustom(int value)
        {
            if (value < 1 || value > 10)
                throw new ArgumentOutOfRangeException("RatingCustom");
            
            Value = value;
        }

        public int Value { get; }
    }
}