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
