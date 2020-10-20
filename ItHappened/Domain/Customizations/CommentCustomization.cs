using System;

namespace ItHappened.Domain.Customizationizations
{
    public class CommentCustomization
    {
        public CommentCustomization(string value)
        {
            Value = value ?? throw new ArgumentNullException("CommentCustomization");
        }

        public string Value { get; }
    }
}