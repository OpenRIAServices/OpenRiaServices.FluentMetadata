using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FluentMetadata.Tests.Web.Model
{
    public class Foo
    {
        [DataMember]
        [Key]
        public int Id { get; set; }
        [DataMember]
        public int Id2 { get; set; }
        [DataMember]
        public string ExcludedString { get; set; }
        [DataMember]
        public string RequiredString { get; set; }
        [DataMember]
        public string RegularExpressionString { get; set; }
    }
}