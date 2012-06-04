// AssociateMetadataClassesAttribute.cs
//

using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using System.Linq;

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
            : base(typeof(FluentTypeDescriptionProvider))
        {
            if(typeof(IFluentMetadataConfiguration).IsAssignableFrom(fluentMetadataConfiguration) == false)
            {
                throw new ArgumentException("metaDataConfigurationType should implement IFluentMetadataConfiguration.");
            }
            this.metaDataConfiguration =
                (IFluentMetadataConfiguration)Activator.CreateInstance(fluentMetadataConfiguration);
        }

        public override DomainServiceDescriptionProvider CreateProvider(
            Type domainServiceType, DomainServiceDescriptionProvider parent)
        {
            var chainedProvider = CreateProviderChain(domainServiceType);
            return new FluentTypeDescriptionProvider(domainServiceType, parent, metaDataConfiguration, chainedProvider ?? parent);
        }

        private DomainServiceDescriptionProvider CreateProviderChain(Type domainServiceType)
        {
            var providers = new List<DomainServiceDescriptionProviderAttribute>();

            // Get DomainServiceDescriptionProviderAttribute attributes for this domain service type
            var thisProviders =
                domainServiceType.GetCustomAttributes(true).OfType<DomainServiceDescriptionProviderAttribute>().Where(
                    x => x.GetType() != this.GetType());
            // Get DomainServiceDescriptionProviderAttribute attributes for base domain service type
            var baseProviders =
                domainServiceType.BaseType.GetCustomAttributes(true).OfType<DomainServiceDescriptionProviderAttribute>()
                    .Where(x => x.GetType() != this.GetType());

            providers.AddRange(baseProviders);
            providers.AddRange(thisProviders);
            if(providers.Any() == false)
            {
                return null;
            }
            // Chain them together
            var provider = providers.Skip(1).Aggregate(
                providers.First().CreateProvider(domainServiceType, null),
                (x, y) => y.CreateProvider(domainServiceType, x));
            return provider;
        }
    }
}