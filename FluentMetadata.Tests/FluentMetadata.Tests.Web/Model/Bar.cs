using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FluentMetadata.Tests.Web.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Bar
    {
        [Key]
        public int Id { get; set; }

        [Key]
        public int Id2 { get; set; }
        public ICollection<Foo> FooSet { get; set; }
    }
}