using System.ServiceModel.DomainServices;

namespace System.Web.DomainServices.FluentMetadata
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    public sealed class AssociationMetadata<TEntity, TMember>
        where TEntity : class where TMember : class
    {
        #region Constants and Fields

        private readonly List<string> fromKeys = new List<string>();

        private readonly string member;

        private readonly MetadataClass<TEntity> metadata;

        private readonly List<string> toKeys = new List<string>();

        private string _name;

        private AssociationAttribute attribute;

        private bool isForeignKey;

        #endregion

        #region Constructors and Destructors

        public AssociationMetadata(MetadataClass<TEntity> metadata, string member)
        {
            this.metadata = metadata;
            this.member = member;

            ResetAttribute();
        }

        #endregion

        #region Public Methods and Operators

        public AssociationMetadata<TEntity, TMember> IsForeignKey()
        {
            isForeignKey = true;
            ResetAttribute();

            return this;
        }

        public AssociationMetadata<TEntity, TMember> WithName(string name)
        {
            _name = name;
            ResetAttribute();
            return this;
        }

        public AssociationMetadata<TEntity, TMember> WithOtherKey(params Expression<Func<TMember, object>>[] keys)
        {
            foreach(var key in keys)
            {
                string keyname = FluentConfigurationExpressions.GetMemberName(key);
                if(toKeys.Contains(keyname) == false)
                {
                    toKeys.Add(keyname);
                }
            }
            ResetAttribute();
            return this;
        }

        public AssociationMetadata<TEntity, TMember> WithThisKey(params Expression<Func<TEntity, object>>[] keys)
        {
            foreach(var key in keys)
            {
                string keyname = FluentConfigurationExpressions.GetMemberName(key);
                if(fromKeys.Contains(keyname) == false)
                {
                    fromKeys.Add(keyname);
                }
            }
            ResetAttribute();
            return this;
        }
        /// <summary>
        /// Indicates that an association references entities belonging to an external DomainContext.
        /// </summary>
        public AssociationMetadata<TEntity, TMember> IsExternalReference()
        {
            metadata.AddMetadata(member, new ExternalReferenceAttribute());
            return this;
        }

        #endregion

        #region Methods

        private static string MakeKey(IEnumerable<string> keys)
        {
            return string.Join(",", keys);
        }

        private void ResetAttribute()
        {
            string name = _name;
            if(name == null)
            {
                string type1 = typeof(TEntity).Name;
                string type2 = typeof(TMember).Name;

                if(isForeignKey)
                {
                    name = type2 + "_" + type1;
                }
                else
                {
                    name = type1 + "_" + type2;
                }
            }
            List<Attribute> memberMetaData = metadata.GetMemberMetadata(member);
            if(attribute != null)
            {
                memberMetaData.Remove(attribute);
            }
            attribute = new AssociationAttribute(name, MakeKey(fromKeys), MakeKey(toKeys))
                { IsForeignKey = isForeignKey };
            metadata.AddMetadata(member, attribute);
        }

        #endregion
    }
}