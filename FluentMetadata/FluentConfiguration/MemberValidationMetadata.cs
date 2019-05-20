// MemberValidationMetadata.cs
//

namespace OpenRiaServices.DomainServices.Server.FluentMetadata
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public sealed class MemberValidationMetadata<TEntity>
        where TEntity : class
    {
        #region Constants and Fields

        private readonly string _memberName;

        private readonly MetadataClass<TEntity> _metadata;

        private Type _resourceType;

        #endregion

        #region Constructors and Destructors

        internal MemberValidationMetadata(MetadataClass<TEntity> metadata, string memberName)
        {
            _metadata = metadata;
            _memberName = memberName;
        }

        #endregion

        #region Public Methods and Operators

        public MemberValidationMetadata<TEntity> Custom<TRules>(string ruleName)
        {
            return Custom<TRules>(ruleName, null);
        }

        public MemberValidationMetadata<TEntity> Custom<TRules>(string ruleName, string errorMessage)
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

        public MemberValidationMetadata<TEntity> Range(int minimum, int maximum)
        {
            return Range(minimum, maximum, null);
        }

        public MemberValidationMetadata<TEntity> Range(int minimum, int maximum, string errorMessage)
        {
            AddMetadata(new RangeAttribute(minimum, maximum), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TEntity> Range(double minimum, double maximum)
        {
            return Range(minimum, maximum, null);
        }

        public MemberValidationMetadata<TEntity> Range(double minimum, double maximum, string errorMessage)
        {
            AddMetadata(new RangeAttribute(minimum, maximum), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TEntity> RegularExpression(string pattern)
        {
            return RegularExpression(pattern, null);
        }

        public MemberValidationMetadata<TEntity> RegularExpression(string pattern, string errorMessage)
        {
            AddMetadata(new RegularExpressionAttribute(pattern), errorMessage);
            return this;
        }

        public MemberValidationMetadata<TEntity> Required()
        {
            return Required(null);
        }

        public MemberValidationMetadata<TEntity> Required(string errorMessage)
        {
            AddMetadata(new RequiredAttribute(), errorMessage);
            return this;
        }

        /// <summary>
        ///     Sets the resource type to use for error-message lookup if validation fails.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public MemberValidationMetadata<TEntity> ResourceType(Type resourceType)
        {
            _resourceType = resourceType;
            return this;
        }

        #endregion

        #region Methods

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
                    attribute.ErrorMessageResourceName = errorMessage.Substring(4);
                    attribute.ErrorMessageResourceType = _resourceType;
                }
                else
                {
                    attribute.ErrorMessage = errorMessage;
                }
            }

            AddMetadata(attribute);
        }

        #endregion
    }
}