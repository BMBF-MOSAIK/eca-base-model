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
using System.Collections.Generic;
using ECABaseModel.Events;
using ECABaseModel.Prototypes;

namespace ECABaseModel
{
    public sealed class Component
    {
        /// <summary>
        /// Returns the name of the component. This is a shorthand for Prototype.Name
        /// </summary>
        public string Name
        {
            get
            {
                return Prototype.Name;
            }
        }

        /// <summary>
        /// GUID that uniquely identifies this componentn.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// The prototype that was used to create this component.
        /// </summary>
        public ComponentPrototype Prototype { get; private set; }

        /// <summary>
        /// An entity that contains this component.
        /// </summary>
        public Entity ContainingEntity { get; private set; }

        /// <summary>
        /// Accessor that allows to get and set attribute values. Users must cast the value to correct type themselves.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns>Value of the attribute.</returns>
        public Attribute this[string attributeName]
        {
            get
            {
                if (!Prototype.ContainsAttributePrototype(attributeName))
                    throw new KeyNotFoundException("Attribute is not present in the component prototype.");

                return attributes[attributeName];
            }
        }

        /// <summary>
        /// An event that is raised when any attribute of this component is changed.
        /// </summary>
        public event EventHandler<ChangedAttributeEventArgs> ChangedAttribute;

        internal void raiseChangeEvent(string attributeName, object oldValue, object newValue)
        {
            if (ChangedAttribute != null)
            {
                if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
                    ChangedAttribute(this, new ChangedAttributeEventArgs(this, attributeName, oldValue, newValue));
            }
        }

        internal void raiseChangeEventFromInternalChange(string attributeName, object newValue)
        {
            if (ChangedAttribute != null)
            {
                ChangedAttribute(this, new ChangedAttributeEventArgs(this, attributeName, newValue, newValue));
            }
        }

        internal Component(ComponentPrototype prototype, Entity containingEntity)
        {
            Guid = Guid.NewGuid();
            ContainingEntity = containingEntity;
            Prototype = prototype;
            InitializeAttributes();
        }

        private void InitializeAttributes()
        {
            attributes = new Dictionary<string, Attribute>();
            foreach (AttributePrototype attributePrototype in Prototype.AttributePrototypes)
            {
                Attribute attribute = new Attribute(attributePrototype, this);
                attributes.Add(attributePrototype.Name, attribute);
            }
        }

        private IDictionary<string, Attribute> attributes { get; set; }

        // Needed by persistence plugin.
        internal Component() { }
    }
}
