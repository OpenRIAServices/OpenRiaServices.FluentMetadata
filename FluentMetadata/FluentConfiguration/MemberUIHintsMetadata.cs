// MemberUIHintsMetadata.cs
//

namespace System.Web.DomainServices.FluentMetadata
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public sealed class MemberUIHintsMetadata<TEntity>
        where TEntity : class
    {
        #region Constants and Fields

        private readonly string _memberName;

        private readonly MetadataClass<TEntity> _metadata;

        #endregion

        #region Constructors and Destructors

        internal MemberUIHintsMetadata(MetadataClass<TEntity> metadata, string memberName)
        {
            _metadata = metadata;
            _memberName = memberName;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Indicates that the UI should be generated automatically to display this field
        /// </summary>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> AutoGenerateField()
        {
            GetDisplayAttribute().AutoGenerateField = true;
            return this;
        }

        /// <summary>
        /// indicates that the UI should be generated automatically in order to display filtering for this field.
        /// </summary>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> AutoGenerateFilter()
        {
            GetDisplayAttribute().AutoGenerateFilter = true;
            return this;
        }

        /// <summary>
        /// Provide a description to be used to display in the UI.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> Describe(string description)
        {
            GetDisplayAttribute().Description = description;
            return this;
        }

        /// <summary>
        /// Provide a name that is used to group fields in the UI.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> GroupName(string groupName)
        {
            GetDisplayAttribute().GroupName = groupName;
            return this;
        }

        /// <summary>
        /// Indicates that no UI should be generated to display this field.
        /// </summary>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> HideField()
        {
            GetDisplayAttribute().AutoGenerateField = false;
            return this;
        }

        /// <summary>
        /// Provide the name to display in the UI.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> Label(string label)
        {
            GetDisplayAttribute().Name = label;
            return this;
        }

        /// <summary>
        /// Sets the order weight of the column.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> Order(int order)
        {
            GetDisplayAttribute().Order = order;
            return this;
        }

        /// <summary>
        ///  Specifies the watermark for prompts in the UI.
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> Prompt(string prompt)
        {
            GetDisplayAttribute().Prompt = prompt;
            return this;
        }

        /// <summary>
        /// Provide the resourceType to display in the UI.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> ResourceType(Type type)
        {
            GetDisplayAttribute().ResourceType = type;
            return this;
        }

        /// <summary>
        /// Specifies the name that is used for the grid column label.
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TEntity> ShortName(string shortName)
        {
            GetDisplayAttribute().ShortName = shortName;
            return this;
        }

        #endregion

        #region Methods

        private DisplayAttribute GetDisplayAttribute()
        {
            DisplayAttribute displayAttribute = null;

            List<Attribute> attributes = _metadata.GetMemberMetadata(_memberName);
            if(attributes != null)
            {
                displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            }

            if(displayAttribute == null)
            {
                displayAttribute = new DisplayAttribute();
                displayAttribute.ResourceType = GetDisplayAttribute().ResourceType;
                _metadata.AddMetadata(_memberName, displayAttribute);
            }

            return displayAttribute;
        }

        #endregion
    }
}