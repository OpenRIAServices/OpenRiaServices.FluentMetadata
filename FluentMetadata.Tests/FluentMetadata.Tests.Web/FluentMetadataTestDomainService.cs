
namespace FluentMetadata.Tests.Web
{
    using System.Collections.Generic;
    using OpenRiaServices.Hosting;
    using OpenRiaServices.Server;
    using OpenRiaServices.FluentMetadata;
    using FluentMetadata.Tests.Web.Model;


    [EnableClientAccess()]
    [FluentMetadata(typeof(FluentMetadataConfiguration))]
    public class FluentMetadataTestDomainService : DomainService
    {
        public List<Foo> GetFoo()
        {
            return new List<Foo>();
        }
        public List<Bar> GetBar()
        {
            return new List<Bar>();
        }
    }
}


