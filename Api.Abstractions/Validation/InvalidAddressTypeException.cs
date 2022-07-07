using System;

namespace Api.Abstractions.Validation
{
    public class InvalidAddressTypeException : Exception
    {
        public InvalidAddressTypeException() : base("Provided Address Type is Invalid, it should either be IP/Domain")
        {
        }
    }
}
