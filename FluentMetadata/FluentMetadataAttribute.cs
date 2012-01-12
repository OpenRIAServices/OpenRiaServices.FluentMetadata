// AssociateMetadataClassesAttribute.cs
//

using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Server;

namespace System.Web.DomainServices.FluentMetadata
{
    /// <summary>
    /// Attribute applied to a System.ServiceModel.DomainServices.Server.DomainService
    /// type to specify that Metadata for entities is specified using the FluentMetadata interface.
    /// </summary>
    public sealed class FluentMetadataAttribute : DomainServiceDescriptionProviderAttribute
    {
        private readonly IFluentMetadataConfiguration metaDataConfiguration;
        /// <summary>
        /// Initializes a new instance of the FluentMetadataAttribute class.
        /// </summary>
        /// <param name="fluentMetadataConfiguration">Specifies the type of the fluent meta data configruation class. This class should implemement IFluentMetadataConfiguration</param>
        public FluentMetadataAttribute(Type fluentMetadataConfiguration)
            : base(typeof(AssociatedMetadataClassTypeDescriptionProvider))
        {
            if(typeof(IFluentMetadataConfiguration).IsAssignableFrom(fluentMetadataConfiguration) == false)
            {
                throw new ArgumentException("metaDataConfigurationType should implement IFluentMetadataConfiguration.");
            }
            this.metaDataConfiguration = (IFluentMetadataConfiguration)Activator.CreateInstance(fluentMetadataConfiguration);
        }
        public override DomainServiceDescriptionProvider CreateProvider(Type domainServiceType, DomainServiceDescriptionProvider parent)
        {
            return new AssociatedMetadataClassTypeDescriptionProvider(domainServiceType, parent, metaDataConfiguration);
        }

        private sealed class AssociatedMetadataClassTypeDescriptionProvider : DomainServiceDescriptionProvider
        {
            private readonly MetadataContainer container = new MetadataContainer();
            private readonly IFluentMetadataConfiguration metaDataConfiguration;

            public AssociatedMetadataClassTypeDescriptionProvider(Type domainServiceType, DomainServiceDescriptionProvider parent, IFluentMetadataConfiguration metaDataConfiguration)
                : base(domainServiceType, parent)
            {
                this.metaDataConfiguration = metaDataConfiguration;
                metaDataConfiguration.OnTypeCreation(container);
                container.ResolveInheritedMetadata();
            }
            public override ICustomTypeDescriptor GetTypeDescriptor(Type type, ICustomTypeDescriptor parent)
            {
                ICustomTypeDescriptor baseDescriptor = base.GetTypeDescriptor(type, parent);
                var metadata =  GetMetadataClassForType(type);
                if(metadata != null)
                {
                    return new AssociatedMetadataClassTypeDescriptor(metadata, baseDescriptor);
                }
                return baseDescriptor;
            }
            /// <summary>
            /// Finds the MetadataClass for the provided type. That is, either the metadata class defining T, or else the metadata class
            /// of the closest base class of type.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private MetadataClass GetMetadataClassForType(Type type)
            {
                MetadataClass result = null;
                foreach(var metadataClass in container)
                {
                    if(type.Equals(metadataClass.ModelType))
                    {
                        return metadataClass;
                    }
                    if(type.IsSubclassOf(metadataClass.ModelType))
                    {
                        if(result == null || metadataClass.ModelType.IsSubclassOf(result.ModelType))
                        {
                            result = metadataClass;
                        }
                    }
                }
                return result;
            }
        }

        private sealed class AssociatedMetadataClassTypeDescriptor : CustomTypeDescriptor
        {
            private MetadataClass _metadata;

            public AssociatedMetadataClassTypeDescriptor(MetadataClass metadata, ICustomTypeDescriptor parentDescriptor)
                : base(parentDescriptor)
            {
                _metadata = metadata;
            }

            public override AttributeCollection GetAttributes()
            {
                AttributeCollection baseAttributes = base.GetAttributes();

                if(_metadata.HasModelMetadata)
                {
                    List<Attribute> modelAttributes = _metadata.ModelMetadata;
                    return AttributeCollection.FromExisting(baseAttributes, modelAttributes.ToArray());
                }

                return baseAttributes;
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                PropertyDescriptorCollection propDescs = base.GetProperties();
                if(_metadata.HasModelMemberMetadata)
                {
                    List<PropertyDescriptor> newPropDescs = new List<PropertyDescriptor>(propDescs.Count);

                    foreach(PropertyDescriptor propDesc in propDescs)
                    {
                        List<Attribute> memberAttributes = _metadata.GetMemberMetadata(propDesc.Name);
                        if(memberAttributes == null)
                        {
                            newPropDescs.Add(propDesc);
                        }
                        else
                        {
                            newPropDescs.Add(new WrappedPropertyDescriptor(propDesc, memberAttributes.ToArray()));
                        }
                    }

                    propDescs = new PropertyDescriptorCollection(newPropDescs.ToArray());
                }

                return propDescs;
            }
        }

        private sealed class WrappedPropertyDescriptor : PropertyDescriptor
        {

            private PropertyDescriptor _parentPropDesc;

            public WrappedPropertyDescriptor(PropertyDescriptor propDesc, Attribute[] attributes)
                : base(propDesc, attributes)
            {
                _parentPropDesc = propDesc;
            }

            public override Type ComponentType
            {
                get
                {
                    return _parentPropDesc.ComponentType;
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return _parentPropDesc.IsReadOnly;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return _parentPropDesc.PropertyType;
                }
            }

            public override bool CanResetValue(object component)
            {
                return _parentPropDesc.CanResetValue(component);
            }

            public override object GetValue(object component)
            {
                return _parentPropDesc.GetValue(component);
            }

            public override void ResetValue(object component)
            {
                _parentPropDesc.ResetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                _parentPropDesc.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return _parentPropDesc.ShouldSerializeValue(component);
            }
        }
    }
}