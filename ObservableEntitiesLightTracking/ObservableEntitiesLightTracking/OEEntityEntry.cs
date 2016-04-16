using System.Collections.Generic;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public class OEEntityEntry
    {
        private readonly object _entity;
        private readonly OEEntitySet _entitySet;
        Dictionary<string, object> _originalValues;

        internal OEEntityEntry(object entity, OEEntitySet entitySet)
        {
            _entity = entity;
            _entitySet = entitySet;
            _originalValues = new Dictionary<string, object>();

            InitializeOriginalValues();
        }

        public object Entity
        {
            get { return _entity; }
        }

        public OEEntitySet EntitySet
        {
            get { return _entitySet; }
        }

        /// <summary>
        /// Gets or sets the state of an entity.
        /// </summary>
        public OEEntityState State { get; set; }

        /// <summary>
        /// Properties on an entity that have been modified.
        /// </summary>
        public ICollection<string> ModifiedProperties { get; set; }

        private void InitializeOriginalValues()
        {
            foreach (var property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
            {
                _originalValues.Add(property.Name, property.GetValue(_entity));
            }
        }

        internal void CancelChanges()
        {
            foreach (var property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
            {
                if (_originalValues.ContainsKey(property.Name) && property.GetValue(_entity) != _originalValues[property.Name])
                {
                    property.SetValue(_entity, _originalValues[property.Name]);
                }
            }
        }

        internal void ApplyChanges()
        {
            foreach (var property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
            {
                if (_originalValues.ContainsKey(property.Name) && property.GetValue(_entity) != _originalValues[property.Name])
                    _originalValues[property.Name] = property.GetValue(_entity);
            }
        }
    }
}
