using System.Collections.Generic;

namespace System.Web.DomainServices.FluentMetadata
{
    public class MetadataContainer : IEnumerable<MetadataClass>
    {
        internal MetadataContainer()
        {
            this.MetaModelClasses = new List<MetadataClass>();
        }
        internal List<MetadataClass> MetaModelClasses { get; private set; }
        public void Add<TEntity>(MetadataClass<TEntity> metaDataForEntity)
            where TEntity : class
        {
            MetaModelClasses.Add(metaDataForEntity);
        }

        public IEnumerator<MetadataClass> GetEnumerator()
        {
            return MetaModelClasses.GetEnumerator();
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
