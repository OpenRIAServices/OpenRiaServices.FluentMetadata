// FluentMetadataExtensions.cs
//

using System.Linq.Expressions;

namespace System.Web.DomainServices.FluentMetadata
{

    public static class FluentMetadataExtensions
    {

        private static string GetMemberName<TModel, TMember>(Expression<Func<TModel, TMember>> memberReference) where TModel : class
        {
            MemberExpression expression = memberReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("memberReference");
            }
            return expression.Member.Name;
        }

        public static MemberProjectionMetadata<TModel> Projection<TModel, TMember>(this MetadataClass<TModel> metadata, Expression<Func<TModel, TMember>> memberReference) where TModel : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberProjectionMetadata<TModel>(metadata, GetMemberName(memberReference));
        }

        public static MemberUIHintsMetadata<TModel> UIHints<TModel, TMember>(this MetadataClass<TModel> metadata, Expression<Func<TModel, TMember>> memberReference) where TModel : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberUIHintsMetadata<TModel>(metadata, GetMemberName(memberReference));
        }

        public static MemberUIHintsMetadata<TModel> UIHints<TModel>(this MetadataClass<TModel> metadata, string memberName) where TModel : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }
            if(String.IsNullOrEmpty(memberName))
            {
                throw new ArgumentNullException("memberName");
            }

            return new MemberUIHintsMetadata<TModel>(metadata, memberName);
        }

        public static ValidationMetadata<TModel> Validation<TModel>(this MetadataClass<TModel> metadata) where TModel : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new ValidationMetadata<TModel>(metadata);
        }

        public static MemberValidationMetadata<TModel> Validation<TModel, TMember>(this MetadataClass<TModel> metadata, Expression<Func<TModel, TMember>> memberReference) where TModel : class
        {
            if(metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            return new MemberValidationMetadata<TModel>(metadata, GetMemberName(memberReference));
        }
    }
}
