namespace System.Web.DomainServices.FluentMetadata
{
    using System.Collections.Generic;
    using System.ComponentModel;

    internal sealed class FluentTypeDescriptor : CustomTypeDescriptor
    {
        private MetadataClass _metadata;

        public FluentTypeDescriptor(MetadataClass metadata, ICustomTypeDescriptor parentDescriptor)
            : base(parentDescriptor)
        {
            _metadata = metadata;
        }

        public override AttributeCollection GetAttributes()
        {
            var baseAttributes = base.GetAttributes();

            if(_metadata.HasEntityMetadata)
            {
                var entityMetadata = _metadata.EntityMetadata;
                return AttributeCollection.FromExisting(baseAttributes, entityMetadata.ToArray());
            }
            return baseAttributes;
        }
        
        public override PropertyDescriptorCollection GetProperties()
        {
            var propDescs = base.GetProperties();
            if(_metadata.HasEntityMemberMetadata)
            {
                var newPropDescs = new List<PropertyDescriptor>(propDescs.Count);

                foreach(PropertyDescriptor propDesc in propDescs)
                {
                    var memberMetadata = _metadata.GetMemberMetadata(propDesc.Name);
                    if(memberMetadata == null)
                    {
                        newPropDescs.Add(propDesc);
                    }
                    else
                    {
                        newPropDescs.Add(new WrappedPropertyDescriptor(propDesc, memberMetadata.ToArray()));
                    }
                }
                propDescs = new PropertyDescriptorCollection(newPropDescs.ToArray());
            }
            return propDescs;
        }
    }
}