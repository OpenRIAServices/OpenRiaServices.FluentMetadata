// FluentConfigurationExpressions.cs
//

namespace System.Web.DomainServices.FluentMetadata
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public static class FluentConfigurationExpressions
    {
        #region Public Methods and Operators

        public static AssociationMetadata<TEntity, TMember> Association<TEntity, TMember>(
            this MemberProjectionMetadata<TEntity, TMember> projectionMetadata) where TEntity : class
            where TMember : class
        {
            return new AssociationMetadata<TEntity, TMember>(projectionMetadata.Metadata, projectionMetadata.MemberName);
        }

        public static MemberProjectionMetadata<TEntity, TMember> Projection<TEntity, TMember>(
            this MetadataClass<TEntity> metadata, Expression<Func<TEntity, TMember>> memberReference) where TEntity : class

        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberProjectionMetadata<TEntity, TMember>(metadata, GetMemberName(memberReference));
        }
        public static MemberProjectionCollectionMetadata<TModel, TMember> Projection<TModel, TMember>(this MetadataClass<TModel> metadata, Expression<Func<TModel, ICollection<TMember>>> memberReference)
            where TModel : class
            where TMember : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }
            return new MemberProjectionCollectionMetadata<TModel, TMember>(metadata, GetMemberName(memberReference));
        }

        public static MemberUIHintsMetadata<TEntity> UIHints<TEntity, TMember>(
            this MetadataClass<TEntity> metadata, Expression<Func<TEntity, TMember>> memberReference) where TEntity : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberUIHintsMetadata<TEntity>(metadata, GetMemberName(memberReference));
        }

        public static MemberUIHintsMetadata<TEntity> UIHints<TEntity>(
            this MetadataClass<TEntity> metadata, string memberName) where TEntity : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }
            if(String.IsNullOrEmpty(memberName))
            {
                throw new ArgumentNullException("memberName");
            }

            return new MemberUIHintsMetadata<TEntity>(metadata, memberName);
        }

        public static ValidationMetadata<TEntity> Validation<TEntity>(this MetadataClass<TEntity> metadata)
            where TEntity : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new ValidationMetadata<TEntity>(metadata);
        }

        public static MemberValidationMetadata<TEntity> Validation<TEntity, TMember>(
            this MetadataClass<TEntity> metadata, Expression<Func<TEntity, TMember>> memberReference) where TEntity : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberValidationMetadata<TEntity>(metadata, GetMemberName(memberReference));
        }

        #endregion

        #region Methods

        internal static string GetMemberName<TEntity, TMember>(Expression<Func<TEntity, TMember>> memberReference)
            where TEntity : class
        {
            MemberExpression expression = memberReference.Body as MemberExpression
                                          ?? ((UnaryExpression)memberReference.Body).Operand as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("memberReference");
            }
            return expression.Member.Name;
        }

        #endregion
    }
}