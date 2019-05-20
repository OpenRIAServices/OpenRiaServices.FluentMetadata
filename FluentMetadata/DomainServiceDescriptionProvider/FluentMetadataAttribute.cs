// AssociateMetadataClassesAttribute.cs
//

using System.Collections.Generic;
using System.Reflection;
using OpenRiaServices.DomainServices.Server;
using System.Linq;
using System;

namespace OpenRiaServices.DomainServices.Server.FluentMetadata
{
    /// <summary>
    /// Attribute applied to a OpenRiaServices.DomainServices.Server.DomainService
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
        /// <summary>
        /// Collect all non-fluent metadata description providers by visiting the type hierarchy of the domain service.
        /// </summary>
        /// <param name="domainServiceType"></param>
        /// <returns></returns>
        private IEnumerable<DomainServiceDescriptionProviderAttribute> GetNonFluentDescriptionProviders(Type domainServiceType)
        {
            var thisType = this.GetType();
            var type = domainServiceType;
            var allProviders = new List<DomainServiceDescriptionProviderAttribute>();
            while(type != null)
            {
               var providers= type.GetCustomAttributes(false).OfType<DomainServiceDescriptionProviderAttribute>().Where(
                    x => x.GetType() != thisType);
                allProviders.AddRange(providers);
                type = type.BaseType;
            }
            return allProviders;
        }
        private DomainServiceDescriptionProvider CreateProviderChain(Type domainServiceType)
        {
            var providers = new List<DomainServiceDescriptionProviderAttribute>();

            // Get all domain service description providers for the domain services, other than the fluent metadata description provider
            var nonFLuentProviders = GetNonFluentDescriptionProviders(domainServiceType);
            providers.AddRange(nonFLuentProviders);
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