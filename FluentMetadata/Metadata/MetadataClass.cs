// MetadataClass.cs
//

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Web.DomainServices.FluentMetadata
{

    public abstract class MetadataClass
    {
        private readonly List<Attribute> _modelMetadata;
        private readonly Dictionary<string, List<Attribute>> _memberMetadata;

        internal MetadataClass(Type modelType, Type resourceType)
        {
            this.ModelType = modelType;
            this.ResourceType = resourceType;

            _modelMetadata = new List<Attribute>();
            _memberMetadata = new Dictionary<string, List<Attribute>>();
        }

        internal Type ModelType { get; private set; }

        internal bool HasModelMemberMetadata
        {
            get
            {
                return _memberMetadata.Count != 0;
            }
        }

        internal bool HasModelMetadata
        {
            get
            {
                return _modelMetadata.Count != 0;
            }
        }

        internal Type ResourceType { get; private set; }

        protected internal void AddMetadata(Attribute attribute)
        {
            if(attribute == null)
            {
                throw new ArgumentNullException("attribute");
            }

            _modelMetadata.Add(attribute);
        }

        internal void AddMetadata(string memberName, Attribute attribute)
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

        internal List<Attribute> GetMemberMetadata(string memberName)
        {
            List<Attribute> attributes;
            if(_memberMetadata.TryGetValue(memberName, out attributes))
            {
                return attributes;
            }
            return null;
        }

        internal List<Attribute> ModelMetadata
        {
            get
            {
                return _modelMetadata;
            }
        }
    }

    public abstract class MetadataClass<TModel> : MetadataClass where TModel : class
    {

        protected MetadataClass()
            : base(typeof(TModel), null)
        {
        }

        protected MetadataClass(Type resourceType)
            : base(typeof(TModel), resourceType)
        {
        }

        protected void AddMemberMetadata<TMember>(Expression<Func<TModel, TMember>> memberReference, Attribute attribute)
        {
            MemberExpression expression = memberReference.Body as MemberExpression;
            if(expression == null)
            {
                throw new ArgumentNullException("memberReference");
            }

            AddMetadata(expression.Member.Name, attribute);
        }
    }
}
