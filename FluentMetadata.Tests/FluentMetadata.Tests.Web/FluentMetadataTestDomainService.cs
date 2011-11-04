
namespace FluentMetadata.Tests.Web
{
    using System.Collections.Generic;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Web.DomainServices.FluentMetadata;
    using FluentMetadata.Tests.Web.Model;


    [EnableClientAccess()]
    [FluentMetadata(typeof(FluentMetadataConfiguration))]
    public class FluentMetadataTestDomainService : DomainService
    {
        public List<Foo> GetFoo()
        {
            return null;
        }
    }
}


