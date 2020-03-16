using System;

namespace ExtraUniRx.Exceptions
{
    public class SetterNotProvidedException : Exception
    {
        public SetterNotProvidedException() : base("Setter has not been provided for binding")
        {
        }
    }
}