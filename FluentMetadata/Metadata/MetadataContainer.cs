namespace System.Web.DomainServices.FluentMetadata
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Server;

    public class MetadataContainer
    {
        #region Constants and Fields

        private readonly List<MetadataClass> metaData = new List<MetadataClass>();

        private readonly DomainServiceDescriptionProvider provider;

        #endregion

        #region Constructors and Destructors

        internal MetadataContainer(DomainServiceDescriptionProvider provider)
        {
            this.provider = provider;
        }

        #endregion

        #region Public Methods and Operators

        public void Add<T>(MetadataClass<T> metadataClass)
            where T : class
        {
            if(Fetch<T>() != null)
            {
                throw new Exception("a metadata class already exists for entity type " + typeof(T).Name);
            }
            metaData.Add(metadataClass);
        }

        public MetadataClass<T> Entity<T>() where T : class
        {
            MetadataClass existing = Fetch<T>();
            if(existing == null)
            {
                existing = new AnonymousMetadataClass<T> { Container = this };
                metaData.Add(existing);
            }
            return existing as MetadataClass<T>;
        }

        public ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            var entryProvider = new AssociatedMetadataTypeTypeDescriptionProvider(type);
            ICustomTypeDescriptor typeDescr = provider.GetTypeDescriptor(type, entryProvider.GetTypeDescriptor(type));
            return typeDescr;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the MetadataClass for the provided type. That is, either the metadata class defining T, or else the metadata class
        /// of the closest base class of type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal MetadataClass Resolve(Type type)
        {
            MetadataClass result = null;
            foreach(MetadataClass metadataClass in metaData)
            {
                if(type == metadataClass.EntityType)
                {
                    return metadataClass;
                }
                if(type.IsSubclassOf(metadataClass.EntityType))
                {
                    if(result == null || metadataClass.EntityType.IsSubclassOf(result.EntityType))
                    {
                        result = metadataClass;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Finds the MetadataClass for the provided type. That is, either the metadata class defining T, or else the metadata class
        /// of the closest base class of type.
        /// </summary>
        /// <returns></returns>
        internal MetadataClass<TEntity> Resolve<TEntity>() where TEntity : class
        {
            return Resolve(typeof(TEntity)) as MetadataClass<TEntity>;
        }

        /// <summary>
        /// Add metadata to subtypes that is derived from base types
        /// </summary>
        internal void ResolveInheritedMetadata()
        {
            foreach(MetadataClass metadataClass in metaData)
            {
                Type baseType = metadataClass.EntityType.BaseType;
                while(baseType != null)
                {
                    MetadataClass baseMetadataClass = metaData.SingleOrDefault(mdClass => mdClass.EntityType == baseType);
                    if(baseMetadataClass != null)
                    {
                        metadataClass.Merge(baseMetadataClass);
                    }
                    baseType = baseType.BaseType;
                }
            }
        }

        private MetadataClass Fetch<T>() where T : class
        {
            return metaData.SingleOrDefault(x => x.EntityType == typeof(T));
        }

        #endregion
    }
}