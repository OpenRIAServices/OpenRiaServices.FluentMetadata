using System.Collections.Generic;
using System.Linq;

namespace System.Web.DomainServices.FluentMetadata
{
    public class MetadataContainer : IEnumerable<MetadataClass>
    {
        internal MetadataContainer()
        {
            this.MetaDataClasses = new List<MetadataClass>();
        }
        internal List<MetadataClass> MetaDataClasses { get; private set; }
        /// <summary>
        /// Add metadata to subtypes that is derived from base types
        /// </summary>
        internal void ResolveInheritedMetadata()
        {
            foreach(var metadataClass in MetaDataClasses)
            {
                var baseType = metadataClass.ModelType.BaseType;
                while(baseType != null)
                {
                    var baseMetadataClass = MetaDataClasses.SingleOrDefault(mdClass => mdClass.ModelType.Equals(baseType));
                    if(baseMetadataClass != null)
                    {
                        metadataClass.Merge(baseMetadataClass);
                    }
                    baseType = baseType.BaseType;
                }
            }
        }
        public void Add<TEntity>(MetadataClass<TEntity> metaDataForEntity)
            where TEntity : class
        {
            MetaDataClasses.Add(metaDataForEntity);
        }

        public IEnumerator<MetadataClass> GetEnumerator()
        {
            return MetaDataClasses.GetEnumerator();
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
