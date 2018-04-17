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

namespace ECABaseModel.Events
{
    /// <summary>
    /// Event arguments for ChangedAttribute event in Entity and Component classes.
    /// </summary>
    public class ChangedAttributeEventArgs : EventArgs
    {
        public ChangedAttributeEventArgs(Component component, string attributeName, object oldValue, object newValue)
        {
            Component = component;
            AttributeName = attributeName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Returns containing entity. This is a shorthand for Component.ContainingEntity
        /// </summary>
        public Entity Entity {
            get
            {
                return Component.ContainingEntity;
            }
        }

        public Component Component { get; private set;  }
        public string AttributeName { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
    }
}
