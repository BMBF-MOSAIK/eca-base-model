using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ECABaseModel.Events;
using ECABaseModel.Exceptions;
using ECABaseModel.Prototypes;

namespace ECABaseModel
{
    public interface IComponentRegistry
    {
        ReadOnlyCollection<ComponentPrototype> RegisteredComponents { get; }
        void Register(ComponentDefinition definition);
        ComponentPrototype FindComponentDefinition(string componentName);
        event EventHandler<RegisteredComponentEventArgs> RegisteredComponent;
    }

    /// <summary>
    /// Delegate type which describes a function used to upgrade components. It should set attribute values in
    /// <paramref name="newComponent"/> based on values in <paramref name="oldComponent"/>. Unless modified
    /// attributes in the <paramref name="newComponent"/> will have their default values.
    /// </summary>
    /// <param name="oldComponent">Old component.</param>
    /// <param name="newComponent">New component.</param>
    public delegate void ComponentUpgrader(Component oldComponent, Component newComponent);

    /// <summary>
    /// Manages component definitions.
    /// </summary>
    public sealed class ComponentRegistry : IComponentRegistry
    {
        public static IComponentRegistry Instance = new ComponentRegistry();

        /// <summary>
        /// A collection of registered components.
        /// </summary>
        public ReadOnlyCollection<ComponentPrototype> RegisteredComponents
        {
            get
            {
                var collection = new List<ComponentPrototype>(registeredComponents.Values);
                return new ReadOnlyCollection<ComponentPrototype>(collection);
            }
        }

        /// <summary>
        /// Registers a new component definition. An exception will be raised if the component with the same name is 
        /// already registered.
        /// </summary>
        /// <param name="definition">New component definition.</param>
        public void Register(ComponentDefinition definition)
        {
            if (registeredComponents.ContainsKey(definition.Name))
                throw new ComponentRegistrationException("Component with the same name is already registered.");

            registeredComponents.Add(definition.Name, definition);

            if (RegisteredComponent != null)
                RegisteredComponent(this, new RegisteredComponentEventArgs(definition));
        }

        /// <summary>
        /// Finds component definition by component's name. If component is not defined, null is returned.
        /// </summary>
        /// <param name="componentName">Component name.</param>
        /// <returns>Component definition.</returns>
        public ComponentPrototype FindComponentDefinition(string componentName)
        {
            if (!registeredComponents.ContainsKey(componentName))
                return null;

            return registeredComponents[componentName];
        }

        public event EventHandler<RegisteredComponentEventArgs> RegisteredComponent;

        private Dictionary<string, ComponentDefinition> registeredComponents =
            new Dictionary<string, ComponentDefinition>();

        internal IEnumerable<Entity> cec = CEC.Instance;
    }
}
