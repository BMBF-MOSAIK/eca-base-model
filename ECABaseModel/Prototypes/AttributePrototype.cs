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
using ECABaseModel.Exceptions;
using System.Reflection;

namespace ECABaseModel.Prototypes
{
    /// <summary>
    /// Represents a read-only attribute definition.
    /// </summary>
    public sealed class AttributePrototype
    {
        public AttributePrototype(string name, Type type, object defaultValue, Guid guid)
        {
            Guid = guid;
            Name = name;
            Type = type;

            // The Has* attributes are assigned here for caching reasons.
            var interfaces = type.GetTypeInfo().GetInterfaces();
            HasNotifyCollectionChangedNotification = interfaces.Contains(typeof(System.Collections.Specialized.INotifyCollectionChanged));
            HasPropertyChangedNotification = interfaces.Contains(typeof(System.ComponentModel.INotifyPropertyChanged));

            try
            {
                MethodInfo castMethod = GetType().GetTypeInfo().GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Static);
                MethodInfo specificCastMethod = castMethod.MakeGenericMethod(type);
                DefaultValue = specificCastMethod.Invoke(null, new object[] { defaultValue });
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is InvalidCastException)
                {
                    throw new AttributeDefinitionException(
                        "Default value for the attribute can not be cast to its type.");
                }
                else
                {
                    throw e.InnerException;
                }
            }
        }

        /// <summary>
        /// GUID that identifies this attribute definition.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Name of the attribute.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Default value for the attribute.
        /// </summary>
        public object DefaultValue { get; private set; }

        /// <summary>
        /// Type of the attribute.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Attribute implements System.Collections.Specialized.INotifyCollectionChanged
        /// </summary>
        public bool HasNotifyCollectionChangedNotification { get; private set; }

        /// <summary>
        /// Attribute implements System.ComponentModel.INotifyPropertyChanged
        /// </summary>
        public bool HasPropertyChangedNotification { get; private set; }

        private static T Cast<T>(object o)
        {
            return (T)o;
        }

        // Needed by persistence plugin.
        internal AttributePrototype() { }
    }
}
