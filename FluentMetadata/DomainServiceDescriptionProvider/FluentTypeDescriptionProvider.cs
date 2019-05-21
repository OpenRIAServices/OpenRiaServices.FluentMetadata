namespace OpenRiaServices.FluentMetadata
{
    using System;
    using System.ComponentModel;
    using OpenRiaServices.DomainServices.Server;

    internal sealed class FluentTypeDescriptionProvider : DomainServiceDescriptionProvider
    {
        private readonly MetadataContainer container;
        public FluentTypeDescriptionProvider(Type domainServiceType, DomainServiceDescriptionProvider parent, IFluentMetadataConfiguration metaDataConfiguration, DomainServiceDescriptionProvider chainedProvider)
            : base(domainServiceType, parent)
        {
            container = new MetadataContainer(chainedProvider);
            metaDataConfiguration.OnTypeCreation(container);
            container.ResolveInheritedMetadata();
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type type, ICustomTypeDescriptor parent)
        {
            var baseDescriptor = base.GetTypeDescriptor(type, parent);
            var metadata = container.Resolve(type);
            if(metadata != null)
            {
                return new FluentTypeDescriptor(metadata, baseDescriptor);
            }
            return baseDescriptor;
        }
    }
}