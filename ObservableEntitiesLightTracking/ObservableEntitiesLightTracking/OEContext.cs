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

        public OEContext()
        {
            _setsCollection = new Dictionary<Type, OEEntitySet>();
            _changeTracker = new OEChangeTracker();
            _changeTracker.EntityChanged += changeTracker_EntityChanged;
        }

        void changeTracker_EntityChanged(object sender, EntityEntryEventArgs e)
        {
            #region Trigger entity set validation if ValidateOnPropertyChanged is set to true

            var entitySet = _setsCollection[e.EntityEntry.Entity.GetType()];

            var validationResults = new Collection<ValidationResultWithSeverityLevel>();
            if (entitySet.ValidateOnPropertyChanged)
                entitySet.Validate(validationResults);

            #endregion Trigger entity set validation if ValidateOnPropertyChanged is set to true

            if (EntityChanged != null) EntityChanged(this, e);
        }

        /// <summary>
        /// Event for when an entity in the collection has changed its tracking state.
        /// </summary>
        public event EventHandler EntityChanged;

        public bool HasChanges()
        {
            return _changeTracker.HasChanges();
        }

        public IEnumerable<object> GetChanges()
        {
            var result = _changeTracker.GetChanges();
            return result;
        }

        public void CancelChanges()
        {
            _changeTracker.CancelChanges();
        }

        public void CancelChanges<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            _changeTracker.CancelChanges();
        }

        public void ApplyChanges()
        {
            _changeTracker.ApplyChanges();
        }

        public void ApplyChanges<TEntity>() where TEntity : class, INotifyPropertyChanged
        {
            _changeTracker.ApplyChanges();
        }

        public OEEntitySet<TEntity> Set<TEntity>(IServiceProvider validationServiceProvider) where TEntity : class, INotifyPropertyChanged
        {
            if (!_setsCollection.ContainsKey(typeof(TEntity)))
                _setsCollection.Add(typeof(TEntity), new OEEntitySet<TEntity>(_changeTracker, validationServiceProvider));

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
                result = result && set.Value.Validate(validationResults);
            }
            return result;
        }
    }
}
