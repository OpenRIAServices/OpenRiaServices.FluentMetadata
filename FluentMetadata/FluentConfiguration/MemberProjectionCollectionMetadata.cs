namespace System.Web.DomainServices.FluentMetadata
{
    public sealed class MemberProjectionCollectionMetadata<TModel, TMember>: MemberProjectionMetadata<TModel, TMember>
        where TModel : class where TMember : class
    {
        internal MemberProjectionCollectionMetadata(MetadataClass<TModel> metadata, string memberName)
            : base(metadata, memberName)
        {
        }
    }
}