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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ECABaseModel.Prototypes
{
    /// <summary>
    /// Component definition that can not be modified.
    /// </summary>
    public abstract class ComponentPrototype
    {
        /// <summary>
        /// GUID that uniquely identifies this component definition.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Name of the component.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A collection of attribute definitions.
        /// </summary>
        public abstract ReadOnlyCollection<AttributePrototype> AttributeDefinitions { get; }

        /// <summary>
        /// Returns a attribute definition by its name or throws KeyNotFoundException if the attribute is not defined.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>Attribute definition.</returns>
        public abstract AttributePrototype this[string attributeName] { get; }

        /// <summary>
        /// Verifies whether this component definition contains a definition for an attribute with a given name.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>True if definition for such attribute is present, false otherwise.</returns>
        public abstract bool ContainsAttributeDefinition(string attributeName);

        internal ComponentPrototype(string name, Guid guid)
        {
            Guid = guid;
            Name = name;
        }

        // Needed by persistence plugin.
        internal ComponentPrototype() { }
    }
}
