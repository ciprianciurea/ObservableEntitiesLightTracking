using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public class OEChangeTracker
    {
        private ICollection<OEEntityEntry> _trackingEntityCollection;

        internal OEChangeTracker()
        {
            _trackingEntityCollection = new Collection<OEEntityEntry>();
        }

        internal event EventHandler<EntityEntryEventArgs> EntityChanged;
        internal event EventHandler<EntityEntryPropertyChangedEventArgs> EntityPropertyChanged;

        void entity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName))
                return;

            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == sender);
            if (entityEntry != null)
            {
                if (entityEntry.EntitySet.AlwaysTrackModifiedProperties || entityEntry.State == OEEntityState.Unchanged || entityEntry.State == OEEntityState.Modified)
                {
                    entityEntry.AddModifiedProperty(e.PropertyName);
                }

                OnEntityPropertyChanged(entityEntry, e.PropertyName);
            }
        }

        void OnEntityChanged(OEEntityEntry entityEntry)
        {
            var entityChangedHandler = EntityChanged;
            if (entityChangedHandler != null)
                entityChangedHandler(this, new EntityEntryEventArgs(entityEntry));
        }

        void OnEntityPropertyChanged(OEEntityEntry entityEntry, string propertyName)
        {
            var entityChangedHandler = EntityPropertyChanged;
            if (entityChangedHandler != null)
                entityChangedHandler(this, new EntityEntryPropertyChangedEventArgs(entityEntry, propertyName));
        }

        internal OEEntityEntry AttachEntry<TEntity>(TEntity entity, OEEntitySet entitySet) where TEntity : class
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry == null)
            {
                entityEntry = new OEEntityEntry(entity, entitySet) { State = OEEntityState.Unchanged };
                _trackingEntityCollection.Add(entityEntry);
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(entity.GetType()))
                    ((INotifyPropertyChanged)entity).PropertyChanged += entity_PropertyChanged;
            }

            return entityEntry;
        }

        internal OEEntityEntry DetachEntry<TEntity>(TEntity entity) where TEntity : class
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry != null)
            {
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(entity.GetType()))
                    ((INotifyPropertyChanged)entity).PropertyChanged -= entity_PropertyChanged;
                _trackingEntityCollection.Remove(entityEntry);
            }

            return entityEntry;
        }

        internal OEEntityEntry AddEntry<TEntity>(TEntity entity, OEEntitySet entitySet) where TEntity : class
        {
            var entityEntry = _trackingEntityCollection.FirstOrDefault(p => p.Entity == entity);

            if (entityEntry == null)
            {
                entityEntry = new OEEntityEntry(entity, entitySet) { State = OEEntityState.Added };
                _trackingEntityCollection.Add(entityEntry);
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(entity.GetType()))
                    ((INotifyPropertyChanged)entity).PropertyChanged += entity_PropertyChanged;

                OnEntityChanged(entityEntry);
            }

            return entityEntry;
        }

        internal OEEntityEntry DeleteEntry<TEntity>(TEntity entity) where TEntity : class
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
            foreach (var entityEntry in _trackingEntityCollection.Where(p => !typeof(INotifyPropertyChanged).IsAssignableFrom(p.Entity.GetType())).ToList())
                entityEntry.DetectChanges();

            var result = _trackingEntityCollection.Any(p => p.State != OEEntityState.Unchanged);
            return result;
        }

        internal bool HasChanges<TEntity>() where TEntity : class
        {
            foreach (var entityEntry in _trackingEntityCollection.Where(p => !typeof(INotifyPropertyChanged).IsAssignableFrom(p.Entity.GetType())).ToList())
                entityEntry.DetectChanges();

            var result = _trackingEntityCollection.Any(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity);
            return result;
        }

        internal IEnumerable<OEEntityEntry> GetChanges()
        {
            foreach (var entityEntry in _trackingEntityCollection.Where(p => !typeof(INotifyPropertyChanged).IsAssignableFrom(p.Entity.GetType())).ToList())
                entityEntry.DetectChanges();

            var result = _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged).ToArray();
            return result;
        }

        internal IEnumerable<OEEntityEntry> GetChanges<TEntity>() where TEntity : class
        {
            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TEntity)))
            {
                foreach (var entityEntry in _trackingEntityCollection.Where(p => p.Entity is TEntity).ToList())
                    entityEntry.DetectChanges();
            }

            var result = _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity).ToArray();
            return result;
        }

        internal IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            var result = _trackingEntityCollection.Where(p => p.Entity is TEntity).Select(p => p.Entity as TEntity).ToArray();
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

        internal void CancelChanges<TEntity>() where TEntity : class
        {
            var itemsToDelete = new Collection<OEEntityEntry>();
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity))
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

        internal void ApplyChanges()
        {
            var itemsToDelete = new Collection<OEEntityEntry>();
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged))
            {
                item.ApplyChanges();

                if (item.State == OEEntityState.Deleted)
                    itemsToDelete.Add(item);
                if (item.State != OEEntityState.Unchanged)
                    item.State = OEEntityState.Unchanged;
            }
            foreach (var item in itemsToDelete)
                _trackingEntityCollection.Remove(item);
        }

        internal void ApplyChanges<TEntity>() where TEntity : class
        {
            var itemsToDelete = new Collection<OEEntityEntry>();
            foreach (var item in _trackingEntityCollection.Where(p => p.State != OEEntityState.Unchanged && p.Entity is TEntity))
            {
                item.ApplyChanges();

                if (item.State == OEEntityState.Deleted)
                    itemsToDelete.Add(item);
                if (item.State != OEEntityState.Unchanged)
                    item.State = OEEntityState.Unchanged;
            }
            foreach (var item in itemsToDelete)
                _trackingEntityCollection.Remove(item);
        }

        internal IEnumerable<OEEntityEntry> Entries()
        {
            return _trackingEntityCollection.ToArray();
        }

        internal IEnumerable<OEEntityEntry> Entries<TEntity>() where TEntity : class
        {
            return _trackingEntityCollection.Where(p => p.Entity is TEntity);
        }
    }
}
