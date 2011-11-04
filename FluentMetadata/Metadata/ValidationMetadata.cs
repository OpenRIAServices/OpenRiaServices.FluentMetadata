// ValidationMetadata.cs
//

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace System.Web.DomainServices.FluentMetadata
{

    public sealed class ValidationMetadata<TModel> where TModel : class
    {

        private MetadataClass<TModel> _metadata;

        internal ValidationMetadata(MetadataClass<TModel> metadata)
        {
            _metadata = metadata;
        }

        private void AddMetadata(ValidationAttribute attribute)
        {
            _metadata.AddMetadata(attribute);
        }

        private void AddMetadata(ValidationAttribute attribute, string errorMessage)
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

            AddMetadata(attribute);
        }

        public ValidationMetadata<TModel> Custom<TRules, TMember>(Expression<Func<TRules, TMember>> ruleReference)
        {
            MemberExpression expression = ruleReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("ruleReference");
            }

            AddMetadata(new CustomValidationAttribute(typeof(TRules), expression.Member.Name));
            return this;
        }

        public ValidationMetadata<TModel> Custom<TRules, TMember>(Expression<Func<TRules, TMember>> ruleReference, string errorMessage)
        {
            MemberExpression expression = ruleReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("ruleReference");
            }

            AddMetadata(new CustomValidationAttribute(typeof(TRules), expression.Member.Name), errorMessage);
            return this;
        }
    }
}
