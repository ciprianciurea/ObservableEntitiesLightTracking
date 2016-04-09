using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public class OEChangeTracker
    {
        //TODO: implementDetectChanges when entity is not INotifyPropertyChanged and call DetectChanges for HasChanges, GetChanges.

        private ICollection<OEEntityEntry> _trackingEntityCollection;

        internal OEChangeTracker()
        {
            _trackingEntityCollection = new Collection<OEEntityEntry>();
        }

        internal event EventHandler<EntityEntryEventArgs> EntityChanged;

        void entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == sender);
            if (entityEntry != null)
            {
                if (entityEntry.State == OEEntityState.Unchanged)
                    entityEntry.State = OEEntityState.Modified;

                if (entityEntry.State == OEEntityState.Modified)
                {
                    if (entityEntry.ModifiedProperties == null)
                        entityEntry.ModifiedProperties = new List<string>();
                    if (!entityEntry.ModifiedProperties.Contains(e.PropertyName))
                        entityEntry.ModifiedProperties.Add(e.PropertyName);
                }

                OnEntityChanged(entityEntry);
            }
        }

        void OnEntityChanged (OEEntityEntry entityEntry)
        {
            var entityChangedHandler = EntityChanged;
            if (entityChangedHandler != null)
                entityChangedHandler(this, new EntityEntryEventArgs(entityEntry));
        }

        internal OEEntityEntry AttachEntry<TEntity>(TEntity entity, OEEntitySet entitySet) where TEntity : class, INotifyPropertyChanged
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry == null)
            {
                entityEntry = new OEEntityEntry(entity, entitySet) { State = OEEntityState.Unchanged };
                _trackingEntityCollection.Add(entityEntry);
                entity.PropertyChanged += entity_PropertyChanged;
            }

            return entityEntry;
        }

        internal OEEntityEntry DetachEntry<TEntity>(TEntity entity) where TEntity : class, INotifyPropertyChanged
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry != null)
            {
                entity.PropertyChanged -= entity_PropertyChanged;
                _trackingEntityCollection.Remove(entityEntry);
            }

            return entityEntry;
        }

        internal OEEntityEntry AddEntry<TEntity>(TEntity entity, OEEntitySet entitySet) where TEntity : class, INotifyPropertyChanged
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry == null)
            {
                entityEntry = new OEEntityEntry(entity, entitySet) { State = OEEntityState.Added };
                _trackingEntityCollection.Add(entityEntry);
                entity.PropertyChanged += entity_PropertyChanged;

                OnEntityChanged(entityEntry);
            }

            return entityEntry;
        }

        internal OEEntityEntry DeleteEntry<TEntity>(TEntity entity) where TEntity : class, INotifyPropertyChanged
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry != null)
            {
                if (entityEntry.State == OEEntityState.Added)
                    DetachEntry<TEntity>((TEntity)entityEntry.Entity);
                else
                {
                    entityEntry.State = OEEntityState.Deleted;
                    OnEntityChanged(entityEntry);
                }
            }

            return entityEntry;
        }

        internal bool HasChanges()
        {
            var result = _trackingEntityCollection.Any(p => p.State != OEEntityState.Unchanged);
            return result;
        }

        internal IEnumerable<object> GetChanges()
        {
            var result = _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged).Select(p => p.Entity);
            return result;
        }

        internal IEnumerable<TEntity> GetChanges<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            var result = _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity).Select(p => p.Entity as TEntity);
            return result;
        }

        internal void CancelChanges()
        {
            var itemsToDelete = new Collection<OEEntityEntry>();
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged))
            {
                item.CancelChanges();

                if (item.State == OEEntityState.Modified || item.State == OEEntityState.Deleted)
                    item.State = OEEntityState.Unchanged;
                else if (item.State == OEEntityState.Added)
                {
                    if (typeof(INotifyPropertyChanged).IsAssignableFrom(item.Entity.GetType()))
                        ((INotifyPropertyChanged)item.Entity).PropertyChanged -= entity_PropertyChanged;
                    itemsToDelete.Add(item);
                }
            }
            foreach (var item in itemsToDelete)
                _trackingEntityCollection.Remove(item);
        }

        internal void CancelChanges<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity))
            {
                item.CancelChanges();

                if (item.State == OEEntityState.Modified || item.State == OEEntityState.Deleted)
                    item.State = OEEntityState.Unchanged;
            }
        }

        internal void ApplyChanges()
        {
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged))
            {
                item.ApplyChanges();

                if (item.State != OEEntityState.Unchanged)
                    item.State = OEEntityState.Unchanged;
            }
        }

        internal void ApplyChanges<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity))
            {
                item.ApplyChanges();

                if (item.State != OEEntityState.Unchanged)
                    item.State = OEEntityState.Unchanged;
            }
        }

        internal IEnumerable<OEEntityEntry> Entries()
        {
            return _trackingEntityCollection.ToArray();
        }

        internal IEnumerable<OEEntityEntry> Entries<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            return _trackingEntityCollection.Where(p => p.Entity is TEntity);
        }
    }
}
