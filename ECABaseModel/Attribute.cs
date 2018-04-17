/*
Copyright 2018 T.Spieldenner, DFKI GmbH

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ECABaseModel.Prototypes;

namespace ECABaseModel
{
    public class Attribute
    {
        public Attribute(AttributePrototype prototype, Component parentComponent)
        {
            ParentComponent = parentComponent;
            Prototype = prototype;
            Value = prototype.DefaultValue;
        }

        public Component ParentComponent { get; private set; }

        public object Value
        {
            get
            {
                return CurrentValue;
            }

            set
            {
                Set(value);
            }
        }

        public Type Type
        {
            get { return Prototype.Type; }
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
            ParentComponent.raiseChangeEvent(Prototype.Name, oldValue, CurrentValue);
        }

        private void deRegisterEventHandler()
        {
            if (Prototype.HasNotifyCollectionChangedNotification)
            {
                ((INotifyCollectionChanged)Value).CollectionChanged -= OnCollectionChanged;
            }
            else if (Prototype.HasPropertyChangedNotification)
            {
                ((INotifyPropertyChanged)Value).PropertyChanged -= OnPropertyChanged;
            }
        }

        private void registerChangedEventHandlers()
        {
            if (Prototype.HasNotifyCollectionChangedNotification)
            {
                ((INotifyCollectionChanged)Value).CollectionChanged += OnCollectionChanged;
            }
            else if (Prototype.HasPropertyChangedNotification)
            {
                ((INotifyPropertyChanged)Value).PropertyChanged += OnPropertyChanged;
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ParentComponent.raiseChangeEventFromInternalChange(Prototype.Name, CurrentValue);
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            ParentComponent.raiseChangeEventFromInternalChange(Prototype.Name, CurrentValue);
        }

        public AttributePrototype Prototype;
        private object CurrentValue;
    }
}
