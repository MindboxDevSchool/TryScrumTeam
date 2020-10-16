using System;

namespace ItHappened.Domain.Exceptions
{
    public class EditingImmutableDataException: Exception
    {
        public EditingImmutableDataException(string param)
            : base($"Trying of editing immutable object: {param}")
        {
            
        }
    }
}