namespace OpenRiaServices.FluentMetadata
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Dummy class because PropertyDescriptor is abstract and can't be instantiated directly
    /// </summary>
    internal sealed class WrappedPropertyDescriptor : PropertyDescriptor
    {

        private PropertyDescriptor _parentPropDesc;

        public WrappedPropertyDescriptor(PropertyDescriptor propDesc, Attribute[] attributes)
            : base(propDesc, attributes)
        {
            _parentPropDesc = propDesc;
        }

        public override Type ComponentType
        {
            get
            {
                return _parentPropDesc.ComponentType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return _parentPropDesc.IsReadOnly;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _parentPropDesc.PropertyType;
            }
        }

        public override bool CanResetValue(object component)
        {
            return _parentPropDesc.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return _parentPropDesc.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            _parentPropDesc.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            _parentPropDesc.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return _parentPropDesc.ShouldSerializeValue(component);
        }
    }
}