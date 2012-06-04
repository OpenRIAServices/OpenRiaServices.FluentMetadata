// ValidationMetadata.cs
//

namespace System.Web.DomainServices.FluentMetadata
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    public sealed class ValidationMetadata<TEntity>
        where TEntity : class
    {
        #region Constants and Fields

        private readonly MetadataClass<TEntity> _metadata;

        private Type _resourceType;

        #endregion

        #region Constructors and Destructors

        internal ValidationMetadata(MetadataClass<TEntity> metadata)
        {
            _metadata = metadata;
        }

        #endregion

        #region Public Methods and Operators

        public ValidationMetadata<TEntity> Custom<TRules, TMember>(Expression<Func<TRules, TMember>> ruleReference)
        {
            var expression = ruleReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("ruleReference");
            }

            AddMetadata(new CustomValidationAttribute(typeof(TRules), expression.Member.Name));
            return this;
        }

        public ValidationMetadata<TEntity> Custom<TRules, TMember>(
            Expression<Func<TRules, TMember>> ruleReference, string errorMessage)
        {
            var expression = ruleReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("ruleReference");
            }

            AddMetadata(new CustomValidationAttribute(typeof(TRules), expression.Member.Name), errorMessage);
            return this;
        }

        /// <summary>
        ///     Sets the resource type to use for error-message lookup if validation fails.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public ValidationMetadata<TEntity> ResourceType(Type resourceType)
        {
            _resourceType = resourceType;
            return this;
        }

        #endregion

        #region Methods

        private void AddMetadata(ValidationAttribute attribute)
        {
            _metadata.AddMetadata(attribute);
        }

        private void AddMetadata(ValidationAttribute attribute, string errorMessage)
        {
            if(errorMessage.StartsWith("res:"))
            {
                attribute.ErrorMessage = errorMessage.Substring(4);
                attribute.ErrorMessageResourceType = _resourceType;
            }
            else
            {
                attribute.ErrorMessage = errorMessage;
            }

            AddMetadata(attribute);
        }

        #endregion
    }
}