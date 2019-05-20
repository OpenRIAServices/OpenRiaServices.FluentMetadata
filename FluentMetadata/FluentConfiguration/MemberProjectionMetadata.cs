using System.ComponentModel.DataAnnotations;
using OpenRiaServices.DomainServices;
using OpenRiaServices.DomainServices.Server;

namespace OpenRiaServices.FluentMetadata
{
    public class MemberProjectionMetadata<TEntity, TMember>
        where TEntity : class
    {
        #region Constants and Fields

        private readonly MetadataClass<TEntity> _metadata;

        #endregion

        #region Constructors and Destructors

        public MemberProjectionMetadata(MetadataClass<TEntity> metadata, string memberName)
        {
            _metadata = metadata;
            MemberName = memberName;
        }

        #endregion

        #region Public Properties

        public string MemberName { get; private set; }

        public MetadataClass<TEntity> Metadata
        {
            get { return _metadata; }
        }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Indicates that an association references entities belonging to an external DomainContext.
        /// </summary>
        public void ExternalReference()
        {
            _metadata.AddMetadata(MemberName, new ExternalReferenceAttribute());
        }

        /// <summary>
        ///   Indicates that an entity member should not exist in the code generated client
        ///   view of the entity, and that the value should never be sent to the client.
        /// </summary>
        public void Exclude()
        {
            _metadata.AddMetadata(MemberName, new ExcludeAttribute());
        }

        /// <summary>
        ///   Indicates that the associated entity should be included.
        /// </summary>
        public void Include()
        {
            _metadata.AddMetadata(MemberName, new IncludeAttribute());
        }

        /// <summary>
        ///   Indicates that the associated entity should be included.
        /// </summary>
        /// <param name="memberPath"> Dotted path specifying the navigation path from the member this attribute is applied to, to the member to be projected. The projected member must be a scalar. </param>
        /// <param name="name"> The member name for the projected member. </param>
        public void IncludeMember(string memberPath, string name)
        {
            _metadata.AddMetadata(MemberName, new IncludeAttribute(memberPath, name));
        }

        /// <summary>
        ///   Denotes one or more properties that uniquely identify an entity.
        /// </summary>
        public void IsKey()
        {
            _metadata.AddMetadata(MemberName, new KeyAttribute());
        }

        /// <summary>
        ///   Indicates that the original value
        ///   of the member should be sent back to the server when the object is updated.
        /// </summary>
        public void RoundTripOriginal()
        {
            _metadata.AddMetadata(MemberName, new RoundtripOriginalAttribute());
        }

        #endregion
    }
}