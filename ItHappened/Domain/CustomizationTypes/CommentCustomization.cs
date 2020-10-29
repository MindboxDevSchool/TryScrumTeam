using System;

namespace ItHappened.Domain.CustomizationTypes
{
    public class CommentCustomization
    {
        public CommentCustomization(string value)
        {
            Value = value ?? throw new ArgumentNullException("CommentCustomization");
        }

        public string Value { get; set; }
    }
}