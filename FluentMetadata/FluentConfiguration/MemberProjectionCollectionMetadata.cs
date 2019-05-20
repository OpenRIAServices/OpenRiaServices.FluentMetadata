namespace OpenRiaServices.DomainServices.Server.FluentMetadata
{
    public sealed class MemberProjectionCollectionMetadata<TModel, TMember>: MemberProjectionMetadata<TModel, TMember>
        where TModel : class where TMember : class
    {
        public MemberProjectionCollectionMetadata(MetadataClass<TModel> metadata, string memberName)
            : base(metadata, memberName)
        {
        }
    }
}