// MemberValidationMetadata.cs
//

using System.ComponentModel.DataAnnotations;

namespace System.Web.DomainServices.FluentMetadata
{

    public sealed class MemberValidationMetadata<TModel> where TModel : class
    {

        private MetadataClass<TModel> _metadata;
        private string _memberName;

        internal MemberValidationMetadata(MetadataClass<TModel> metadata, string memberName)
        {
            _metadata = metadata;
            _memberName = memberName;
        }

        private void AddMetadata(ValidationAttribute attribute)
        {
            _metadata.AddMetadata(_memberName, attribute);
        }

        private void AddMetadata(ValidationAttribute attribute, string errorMessage)
        {
            if(String.IsNullOrEmpty(errorMessage) == false)
            {
                if(errorMessage.StartsWith("res:"))
                {
                    attribute.ErrorMessage = errorMessage.Substring(4);
                    attribute.ErrorMessageResourceType = _metadata.ResourceType;
                }
                else
                {
                    attribute.ErrorMessage = errorMessage;
                }
            }

            AddMetadata(attribute);
        }

        public MemberValidationMetadata<TModel> Custom<TRules>(string ruleName)
        {
            return Custom<TRules>(ruleName, null);
        }

        public MemberValidationMetadata<TModel> Custom<TRules>(string ruleName, string errorMessage)
        {
            if(String.IsNullOrEmpty(ruleName))
            {
                throw new ArgumentNullException("ruleName");
            }
            if(typeof(TRules).GetMethod(ruleName) == null)
            {
                throw new ArgumentException("ruleName");
            }

            AddMetadata(new CustomValidationAttribute(typeof(TRules), ruleName), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TModel> Range(int minimum, int maximum)
        {
            return Range(minimum, maximum, null);
        }

        public MemberValidationMetadata<TModel> Range(int minimum, int maximum, string errorMessage)
        {
            AddMetadata(new RangeAttribute(minimum, maximum), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TModel> Range(double minimum, double maximum)
        {
            return Range(minimum, maximum, null);
        }

        public MemberValidationMetadata<TModel> Range(double minimum, double maximum, string errorMessage)
        {
            AddMetadata(new RangeAttribute(minimum, maximum), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TModel> Required()
        {
            return Required(null);
        }

        public MemberValidationMetadata<TModel> Required(string errorMessage)
        {
            AddMetadata(new RequiredAttribute(), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TModel> RegularExpression(string pattern)
        {
            return RegularExpression(pattern, null);
        }
        public MemberValidationMetadata<TModel> RegularExpression(string pattern, string errorMessage)
        {
            AddMetadata(new RegularExpressionAttribute(pattern), errorMessage);
            return this;
        }

    }
}
