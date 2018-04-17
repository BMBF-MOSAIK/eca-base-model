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
    /// Component prototype that can not be modified.
    /// </summary>
    public abstract class ReadOnlyComponentPrototype
    {
        /// <summary>
        /// GUID that uniquely identifies this component prototype.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Name of the component.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A collection of attribute prototypes.
        /// </summary>
        public abstract ReadOnlyCollection<AttributePrototype> AttributePrototypes { get; }

        /// <summary>
        /// Returns a attribute prototype by its name or throws KeyNotFoundException if the attribute is not defined.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>Attribute prototype.</returns>
        public abstract AttributePrototype this[string attributeName] { get; }

        /// <summary>
        /// Verifies whether this component prototype contains a prototype for an attribute with a given name.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        /// <returns>True if prototype for such attribute is present, false otherwise.</returns>
        public abstract bool ContainsAttributePrototype(string attributeName);

        internal ReadOnlyComponentPrototype(string name, Guid guid)
        {
            Guid = guid;
            Name = name;
        }

        // Needed by persistence plugin.
        internal ReadOnlyComponentPrototype() { }
    }
}
