using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ECABaseModel.Prototypes;

namespace ECABaseModel
{
    public class Attribute
    {
        public Attribute(AttributePrototype definition, Component parentComponent)
        {
            ParentComponent = parentComponent;
            Definition = definition;
            Value = definition.DefaultValue;
        }

        public Component ParentComponent { get; private set; }

        public object Value
        {
            get
            {
                return CurrentValue;
            }

            internal set
            {
                Set(value);
            }
        }

        public Type Type
        {
            get { return Definition.Type; }
        }

        private object convertValueToAttributeType(object value)
        {
            if (value == null || value.GetType() == this.Type)
                return value;

            return Convert.ChangeType(value, this.Type);
        }

        public T As<T>()
        {
            return (T)Value;
        }

        internal void Set(object value)
        {
            if (value != null && CurrentValue != null && !value.Equals(CurrentValue))
            {
                deRegisterEventHandler();
            }
            var oldValue = CurrentValue;
            CurrentValue = value;
            registerChangedEventHandlers();
            ParentComponent.raiseChangeEvent(Definition.Name, oldValue, CurrentValue);
        }

        private void deRegisterEventHandler()
        {
            if (Definition.HasNotifyCollectionChangedNotification)
            {
                ((INotifyCollectionChanged)Value).CollectionChanged -= OnCollectionChanged;
            }
            else if (Definition.HasPropertyChangedNotification)
            {
                ((INotifyPropertyChanged)Value).PropertyChanged -= OnPropertyChanged;
            }
        }

        private void registerChangedEventHandlers()
        {
            if (Definition.HasNotifyCollectionChangedNotification)
            {
                ((INotifyCollectionChanged)Value).CollectionChanged += OnCollectionChanged;
            }
            else if (Definition.HasPropertyChangedNotification)
            {
                ((INotifyPropertyChanged)Value).PropertyChanged += OnPropertyChanged;
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ParentComponent.raiseChangeEventFromInternalChange(Definition.Name, CurrentValue);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            ParentComponent.raiseChangeEventFromInternalChange(Definition.Name, CurrentValue);
        }

        public AttributePrototype Definition;
        private object CurrentValue;
    }
}
