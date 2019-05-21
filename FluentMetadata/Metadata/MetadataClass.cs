// MetadataClass.cs
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OpenRiaServices.FluentMetadata
{

    public abstract class MetadataClass
    {
        public MetadataContainer Container { get; internal set; }


        private readonly List<Attribute> _entityMetadata;
        private readonly Dictionary<string, List<Attribute>> _memberMetadata;
       
        internal MetadataClass(Type entityType)
        {
            EntityType = entityType;

            _entityMetadata = new List<Attribute>();
            _memberMetadata = new Dictionary<string, List<Attribute>>();
        }

        internal Type EntityType { get; private set; }

        internal bool HasEntityMemberMetadata
        {
            get
            {
                return _memberMetadata.Any();
            }
        }

        internal bool HasEntityMetadata
        {
            get
            {
                return _entityMetadata.Any();
            }
        }

        public void AddMetadata(Attribute attribute)
        {
            if(attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            _entityMetadata.Add(attribute);
        }

        public void AddMetadata(string memberName, Attribute attribute)
        {
            if(attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            List<Attribute> attributes;
            if(_memberMetadata.TryGetValue(memberName, out attributes) == false)
            {
                attributes = new List<Attribute>();
                _memberMetadata[memberName] = attributes;
            }

            attributes.Add(attribute);
        }

        public List<Attribute> GetMemberMetadata(string memberName)
        {
            List<Attribute> attributes;
            if(_memberMetadata.TryGetValue(memberName, out attributes))
            {
                return attributes;
            }
            return null;
        }

        public List<Attribute> EntityMetadata
        {
            get
            {
                return _entityMetadata;
            }
        }
        /// <summary>
        /// Copies the member metadata and entity metadata of the provided source MetadataClass to this instance.
        /// </summary>
        /// <param name="source"></param>
        internal void Merge(MetadataClass source)
        {
            foreach(var member in source._memberMetadata.Keys)
            {
                foreach(var attr in source._memberMetadata[member].ToList())
                {
                    AddMetadata(member, attr);
                }
            }
            EntityMetadata.AddRange(source.EntityMetadata);
        }
    }

    public abstract class MetadataClass<TEntity> : MetadataClass where TEntity : class
    {
        protected MetadataClass()
            : base(typeof(TEntity))
        {
        }
    }
}
