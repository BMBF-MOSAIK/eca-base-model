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

using ECABaseModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ECABaseModel.Prototypes
{
    /// <summary>
    /// Represents a modifiable component prototype.
    ///
    /// It can be used to define new components as following:
    /// <example>
    ///     ComponentDefinition mesh = new ComponentDefinition("mesh");
    ///     mesh.AddAttribute<string>("uri", "mesh://default");
    ///     mesh.AddAttribute<bool>("visible");
    ///     mesh.AddAttribute<Vector>("scale", new Vector(1, 1, 1));
    ///     ComponentRegistry.Instance.Register(mesh);
    /// </example>
    /// </summary>
    public sealed class ComponentPrototype : ReadOnlyComponentPrototype
    {
        /// <summary>
        /// Constructs an instance of the ComponentPrototype.
        /// </summary>
        /// <param name="name">Name of the component.</param>
        public ComponentPrototype(string name)
            : base(name, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Constructs an instance of the ComponentPrototype with specified GUID.
        /// </summary>
        /// <param name="name">Name of the component.</param>
        /// <param name="guid">Guid for the component.</param>
        public ComponentPrototype(string name, Guid guid)
            : base(name, guid)
        {
        }

        /// <summary>
        /// Adds a new attribute definition to the component prototype. Default value for the type is used as default
        /// value for the attribute.
        /// </summary>
        /// <typeparam name="T">Type of the new attribute.</typeparam>
        /// <param name="name">Name of the new attribute.</param>
        public void AddAttribute<T>(string name)
        {
            AddAttribute(name, typeof(T), default(T));
        }

        /// <summary>
        /// Adds a new attribute definition to the component prototype. Specified default value is used.
        /// </summary>
        /// <typeparam name="T">Type of the new attribute.</typeparam>
        /// <param name="name">Name of the new attribute.</param>
        /// <param name="defaultValue">Default value of the new attribute.</param>
        public void AddAttribute<T>(string name, object defaultValue)
        {
            AddAttribute(name, typeof(T), defaultValue);
        }

        /// <summary>
        /// Adds a new attribute definition to the component prototype with specified default value and type.
        /// </summary>
        /// <param name="name">Name of the new attribute.</param>
        /// <param name="type">Type of the new attribute.</param>
        /// <param name="defaultValue">Default value of the new attribute.</param>
        public void AddAttribute(string name, Type type, object defaultValue)
        {
            AddAttribute(new AttributePrototype(name, type, defaultValue, Guid.NewGuid()));
        }

        /// <summary>
        /// Add a new attribute prototype to the component prototype.
        /// </summary>
        /// <param name="prototype">Attribute prototype.</param>
        public void AddAttribute(AttributePrototype prototype)
        {
            if (attributePrototypes.ContainsKey(prototype.Name))
                throw new AttributePrototypeException("Attribute with such name is already defined.");

            attributePrototypes[prototype.Name] = prototype;
        }

        public override ReadOnlyCollection<AttributePrototype> AttributePrototypes
        {
            get
            {
                return new ReadOnlyCollection<AttributePrototype>(new List<AttributePrototype>(attributePrototypes.Values));
            }
        }

        public override AttributePrototype this[string attributeName]
        {
            get
            {
                return attributePrototypes[attributeName];
            }
        }

        public override bool ContainsAttributePrototype(string attributeName)
        {
            return attributePrototypes.ContainsKey(attributeName);
        }

        private IDictionary<string, AttributePrototype> attributePrototypes =
            new Dictionary<string, AttributePrototype>();
    }
}