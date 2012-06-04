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
            metadataContainer.Entity<Bar>().Projection(x => x.FooSet).Association().WithName("MyAssociation").
                WithThisKey(x => x.Id2, x=>x.Id).WithOtherKey(x => x.Id2, x=>x.Id);
        }
        private class FooMetadata : MetadataClass<Foo>
        {
            public FooMetadata()
            {
                this.Projection(x => x.Id2).IsKey();
                this.Projection(x => x.ExcludedString).Exclude();
                this.Validation(x => x.RequiredString).Required();
                this.Validation(x => x.RegularExpressionString).RegularExpression("[a-z]");
            }
        }
    }
}