// MemberProjectionMetadata.cs
//

using System.ServiceModel.DomainServices.Server;

namespace System.Web.DomainServices.FluentMetadata
{

    public sealed class MemberProjectionMetadata<TModel> where TModel : class
    {
        private MetadataClass<TModel> _metadata;
        private string _memberName;

        internal MemberProjectionMetadata(MetadataClass<TModel> metadata, string memberName)
        {
            _metadata = metadata;
            _memberName = memberName;
        }
        /// <summary>
        /// Indicates that an entity member should not exist in the code generated client
        /// view of the entity, and that the value should never be sent to the client.
        /// </summary>
        public void Exclude()
        {
            _metadata.AddMetadata(_memberName, new ExcludeAttribute());
        }
        /// <summary>
        /// Indicates that the associated entity should be included.
        /// </summary>
        public void Include()
        {
            _metadata.AddMetadata(_memberName, new IncludeAttribute());
        }
        /// <summary>
        /// Indicates the the associated entity shoudl be included.
        /// </summary>
        /// <param name="memberPath">Dotted path specifying the navigation path from the member this attribute
        ///     is applied to, to the member to be projected. The projected member must be
        ///     a scalar.</param>
        /// <param name="name">The member name for the projected member.</param>
        public void IncludeMember(string memberPath, string name)
        {
            _metadata.AddMetadata(_memberName, new IncludeAttribute(memberPath, name));
        }
    }
}
