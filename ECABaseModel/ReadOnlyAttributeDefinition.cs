using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECABaseModel.Exceptions;
using System.Reflection;

namespace ECABaseModel
{
    /// <summary>
    /// Represents a read-only attribute definition.
    /// </summary>
    public sealed class ReadOnlyAttributeDefinition
    {
        public ReadOnlyAttributeDefinition(string name, Type type, object defaultValue, Guid guid)
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
        internal ReadOnlyAttributeDefinition() { }
    }
}
