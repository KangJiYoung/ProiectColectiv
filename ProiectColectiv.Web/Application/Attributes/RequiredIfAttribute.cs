using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.Application.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private object[] RequiredValues { get; }

        private readonly RequiredAttribute requiredAttribute = new RequiredAttribute();

        public RequiredIfAttribute(params object[] requiredValues)
        {
            RequiredValues = requiredValues;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var shouldBeRequired = true;
            for (var i = 0; i < RequiredValues.Length; i += 2)
            {
                var dependentValue = context.ObjectInstance.GetType().GetProperty(RequiredValues[i].ToString()).GetValue(context.ObjectInstance, null);

                if (dependentValue?.ToString() != RequiredValues[i + 1].ToString())
                {
                    shouldBeRequired = false;
                    break;
                }
            }

            if (shouldBeRequired && !requiredAttribute.IsValid(value))
                return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });

            return ValidationResult.Success;
        }
    }
}
