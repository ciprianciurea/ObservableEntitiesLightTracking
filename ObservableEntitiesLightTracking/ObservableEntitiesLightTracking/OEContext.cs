using ObservableEntitiesLightTracking.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ObservableEntitiesLightTracking
{
    public class OEContext
    {
        private Dictionary<Type, OEEntitySet> _setsCollection;
        private OEChangeTracker _changeTracker;
        private readonly OEContextConfiguration _configuration;

        public OEContext()
        {
            _setsCollection = new Dictionary<Type, OEEntitySet>();
            _changeTracker = new OEChangeTracker();
            _configuration = new OEContextConfiguration();
            _changeTracker.EntityChanged += ChangeTracker_EntityChanged;
            _changeTracker.EntityPropertyChanged += ChangeTracker_EntityPropertyChanged;
        }

        void ChangeTracker_EntityPropertyChanged(object sender, EntityEntryPropertyChangedEventArgs e)
        {
            #region Trigger entity set validation if ValidateOnPropertyChanged is set to true

            var entitySet = e.EntityEntry.EntitySet;

            var validationResults = new Collection<ValidationResultWithSeverityLevel>();
            if (entitySet.ValidateOnPropertyChanged)
                entitySet.ValidateProperty(e.EntityEntry.Entity, e.PropertyName, validationResults);

      #endregion Trigger entity set validation if ValidateOnPropertyChanged is set to true

            EntityChanged?.Invoke(this, e);
        }

        void ChangeTracker_EntityChanged(object sender, EntityEntryEventArgs e)
        {
            EntityChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Event for when an entity in the collection has changed its tracking state.
        /// </summary>
        public event EventHandler EntityChanged;

        internal OEChangeTracker ChangeTracker { get { return _changeTracker; } }
        public OEContextConfiguration Configuration { get { return _configuration; } }

        public bool HasChanges()
        {
            return _changeTracker.HasChanges();
        }

        public IEnumerable<OEEntityEntry> GetChanges()
        {
            var result = _changeTracker.GetChanges();
            return result;
        }

        public void CancelChanges()
        {
            _changeTracker.CancelChanges();
        }

        public void CancelChanges<TEntity>() where TEntity : class
        {
            _changeTracker.CancelChanges<TEntity>();
        }

        public void ApplyChanges()
        {
            _changeTracker.ApplyChanges();
        }

        public void ApplyChanges<TEntity>() where TEntity : class
        {
            _changeTracker.ApplyChanges<TEntity>();
        }

        public OEEntitySet<TEntity> Set<TEntity>() where TEntity : class
        {
            if (!_setsCollection.ContainsKey(typeof(TEntity)))
                _setsCollection.Add(typeof(TEntity), new OEEntitySet<TEntity>(this));

            return _setsCollection[typeof(TEntity)] as OEEntitySet<TEntity>;
        }

        public bool Validate()
        {
            var validationResults = new Collection<ValidationResultWithSeverityLevel>();
            bool result = Validate(validationResults);
            return result;
        }

        public bool Validate(ICollection<ValidationResultWithSeverityLevel> validationResults)
        {
            bool result = true;
            foreach (var set in _setsCollection)
            {
                var setResult = set.Value.Validate(validationResults);
                result = result && setResult;
            }
            return result;
        }
    }
}
