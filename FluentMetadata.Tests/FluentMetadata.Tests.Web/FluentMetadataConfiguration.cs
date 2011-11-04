using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DomainServices.FluentMetadata;
using FluentMetadata.Tests.Web.Model;

namespace FluentMetadata.Tests.Web
{
    public class FluentMetadataConfiguration : IFluentMetadataConfiguration
    {
        public void OnTypeCreation(MetadataContainer metadataContainer)
        {
            metadataContainer.Add(new FooMetadata());

        }
        private class FooMetadata : MetadataClass<Foo>
        {
            public FooMetadata()
            {
                this.Projection(x => x.ExcludedString).Exclude();
                this.Validation(x => x.RequiredString).Required();
                this.Validation(x => x.RegularExpressionString).RegularExpression("[a-z]");
            }
        }
    }
}