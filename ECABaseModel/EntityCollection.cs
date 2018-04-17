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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ECABaseModel.Events;
using ECABaseModel.Exceptions;

namespace ECABaseModel
{
    public class EntityCollection : ICollection<Entity>, INotifyCollectionChanged
    {
        public EntityCollection()
        {

        }

        public void Add(Entity entity)
        {
            lock (entities)
                entities.Add(entity.Guid, entity);

            HandleAdded(entity);
        }

        public void Clear()
        {
            List<Entity> removedEntities = new List<Entity>();
            removedEntities.AddRange(entities.Values);
            entities.Clear();
            foreach (Entity entity in removedEntities)
                HandleRemoved(entity);
        }

        public bool Contains(Entity item)
        {
            return entities.ContainsKey(item.Guid);
        }

        public void CopyTo(Entity[] array, int arrayIndex)
        {
            entities.Values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return entities.Count; }
        }

        public bool Remove(Entity item)
        {
            bool didRemoveItem = false;

            lock (entities)
                didRemoveItem = entities.Remove(item.Guid);

            if (didRemoveItem)
            {
                HandleRemoved(item);
                return true;
            }

            return false;
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return entities.Values.GetEnumerator();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        /// <summary>
        /// Raised when a new entity has been added.
        /// </summary>
        public event EventHandler<EntityEventArgs> AddedEntity;

        /// <summary>
        /// Raised when an entity has been removed.
        /// </summary>
        public event EventHandler<EntityEventArgs> RemovedEntity;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Finds an entity by its Guid as string. Throws EntityNotFoundException if entity is not found.
        /// </summary>
        /// <param name="guid">String representation of the unique identifier.</param>
        /// <returns>An entity.</returns>
        public Entity FindEntity(string guid)
        {
            Guid parsedGuid = Guid.Parse(guid);
            return FindEntity(parsedGuid);
        }

        /// <summary>
        /// Finds an entity by its Guid. Throws EntityNotFoundException if entity is not found.
        /// </summary>
        /// <param name="guid">Guid of the entity.</param>
        /// <returns>An entity.</returns>
        public Entity FindEntity(Guid guid)
        {
            if (!entities.ContainsKey(guid))
                throw new EntityNotFoundException("Entity with given guid is not found.");

            return entities[guid];
        }

        /// <summary>
        /// Returns true of an entity with a given guid is present in the collection.
        /// </summary>
        /// <param name="guid">Guid of the searched entity.</param>
        /// <returns>True of an entity with a given guid is present in the collection, false otherwise.</returns>
        public bool ContainsEntity(Guid guid)
        {
            return entities.ContainsKey(guid);
        }

        // Needed by persistence plugin.
        internal EntityCollection(ICollection<Entity> entityCollection)
        {
            foreach (Entity entity in entityCollection)
            {
                entities.Add(entity.Guid, entity);
            }
        }

        private void HandleAdded(Entity entity)
        {
            if (AddedEntity != null)
                AddedEntity(this, new EntityEventArgs(entity));
        }

        private void HandleRemoved(Entity entity)
        {
            if (RemovedEntity != null)
                RemovedEntity(this, new EntityEventArgs(entity));
        }

        protected Dictionary<Guid, Entity> entities = new Dictionary<Guid, Entity>();
    }
}
