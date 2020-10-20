using System;

namespace ItHappened.Domain.Customizations
{
    public class CommentCustom
    {
        public CommentCustom(string value)
        {
            Value = value ?? throw new ArgumentNullException("CommentCustom");
        }

        public string Value { get; }
    }
}