namespace Rediska.Tests
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class BecauseAttribute : Attribute
    {
        public BecauseAttribute(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}