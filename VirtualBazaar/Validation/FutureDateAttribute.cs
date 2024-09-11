using System.ComponentModel.DataAnnotations;

namespace VirtualBazaar.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class FutureDateAttribute() : ValidationAttribute("Date must be in the future")
    {
        public override bool IsValid(object? value)
        {
            return value is DateTime date && date > DateTime.Now;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}