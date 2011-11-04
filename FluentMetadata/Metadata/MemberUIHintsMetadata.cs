// MemberUIHintsMetadata.cs
//

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.Web.DomainServices.FluentMetadata
{

    public sealed class MemberUIHintsMetadata<TModel> where TModel : class
    {

        private MetadataClass<TModel> _metadata;
        private string _memberName;

        internal MemberUIHintsMetadata(MetadataClass<TModel> metadata, string memberName)
        {
            _metadata = metadata;
            _memberName = memberName;
        }

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
                if(_metadata.ResourceType != null)
                {
                    displayAttribute.ResourceType = _metadata.ResourceType;
                }
                _metadata.AddMetadata(_memberName, displayAttribute);
            }

            return displayAttribute;
        }
        /// <summary>
        /// Indicates that the UI should be generated automatically to display this field
        /// </summary>
        /// <returns></returns>
        public MemberUIHintsMetadata<TModel> AutoGenerateField()
        {
            GetDisplayAttribute().AutoGenerateField = true;
            return this;
        }
        /// <summary>
        /// Indicates that no UI should be generated to display this field.
        /// </summary>
        /// <returns></returns>
        public MemberUIHintsMetadata<TModel> HideField()
        {
            GetDisplayAttribute().AutoGenerateField = false;
            return this;
        }
        /// <summary>
        /// Provide a description to be used to display in the UI.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TModel> Describe(string description)
        {
            GetDisplayAttribute().Description = description;
            return this;
        }
        /// <summary>
        /// Provide the name to display in the UI.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public MemberUIHintsMetadata<TModel> Label(string label)
        {
            GetDisplayAttribute().Name = label;
            return this;
        }
    }
}
