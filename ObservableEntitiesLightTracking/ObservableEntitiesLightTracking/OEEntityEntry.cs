using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public class OEEntityEntry
    {
        private readonly object _entity;
        private readonly OEEntitySet _entitySet;
        Dictionary<string, object> _originalValues;
        IList<OEModifiedPropertyInfo> _modifiedProperties;

        internal OEEntityEntry(object entity, OEEntitySet entitySet)
        {
            _entity = entity;
            _entitySet = entitySet;
            _originalValues = new Dictionary<string, object>();

            InitializeOriginalValues();
            _modifiedProperties = new List<OEModifiedPropertyInfo>();
            ModifiedProperties = new ReadOnlyCollection<OEModifiedPropertyInfo>(_modifiedProperties);
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
        public IReadOnlyCollection<OEModifiedPropertyInfo> ModifiedProperties { get; private set; }

        private void InitializeOriginalValues()
        {
            foreach (System.Reflection.PropertyInfo property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
            {
                var ignoreAttribute = property.GetCustomAttributes(typeof(IgnorePropertyAttribute), true).FirstOrDefault();
                if (ignoreAttribute == null)
                    _originalValues.Add(property.Name, property.GetValue(_entity));
            }
        }

        internal void AddModifiedProperty(string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
                return;

            var modifiedProperty = _modifiedProperties.FirstOrDefault(p => p.Name == propertyName);
            if (modifiedProperty == null)
            {
                _modifiedProperties.Add(new OEModifiedPropertyInfo(propertyName, _originalValues.FirstOrDefault(p => p.Key == propertyName).Value));
            }

            if (State == OEEntityState.Unchanged)
                State = OEEntityState.Modified;
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

            _modifiedProperties.Clear();
        }

        internal void ApplyChanges()
        {
            foreach (var property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
            {
                if (_originalValues.ContainsKey(property.Name) && property.GetValue(_entity) != _originalValues[property.Name])
                    _originalValues[property.Name] = property.GetValue(_entity);
            }

            _modifiedProperties.Clear();
        }

        internal void DetectChanges()
        {
            if (EntitySet.AlwaysTrackModifiedProperties || State == OEEntityState.Unchanged || State == OEEntityState.Modified)
            {
                foreach (var property in _entity.GetType().GetProperties().Where(p => p.GetSetMethod() != null))
                {
                    if (_originalValues.ContainsKey(property.Name) && !property.GetValue(_entity).Equals(_originalValues[property.Name]))
                    {
                        AddModifiedProperty(property.Name);

                        if (State == OEEntityState.Unchanged)
                            State = OEEntityState.Modified;
                    }
                }
            }
        }
    }
}
