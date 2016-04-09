using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public class OEEntitySet<TEntity> : OEEntitySet where TEntity : class, INotifyPropertyChanged
    {
        OEChangeTracker _changeTracker;
        IServiceProvider _validationServiceProvider;

        internal OEEntitySet(OEChangeTracker changeTracker)
        {
            ValidateOnPropertyChanged = false;
            _changeTracker = changeTracker;
        }

        public void Attach(TEntity entity)
        {
            if (entity != null)
                _changeTracker.AttachEntry<TEntity>(entity, this);
        }

        public void Detach(TEntity entity)
        {
            if (entity != null)
            {
                var result = _changeTracker.DetachEntry<TEntity>(entity);
            }
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                var result = _changeTracker.AddEntry<TEntity>(entity, this);
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                var result = _changeTracker.DeleteEntry<TEntity>(entity);
            }
        }

        public bool HasChanges()
        {
            return _changeTracker.HasChanges<TEntity>();
        }

        public IEnumerable<TEntity> GetChanges()
        {
            var result = _changeTracker.GetChanges<TEntity>();
            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var result = _changeTracker.GetAll<TEntity>();
            return result;
        }

        public void SetValidationProvider(IServiceProvider validationServiceProvider)
        {
            _validationServiceProvider = validationServiceProvider;
        }

        public override bool Validate(ICollection<ValidationResultWithSeverityLevel> validationResults)
        {
            bool result = true;

            bool supportsSeverityLevels = typeof(IValidatableObjectWithSeverityLevel).IsAssignableFrom(typeof(TEntity));
            bool supportsWriteErrorInfo = typeof(IWriteDataErrorInfo).IsAssignableFrom(typeof(TEntity));

            var entityEntries = _changeTracker.Entries<TEntity>().Where(p => p.State == OEEntityState.Added || p.State == OEEntityState.Modified);
            var entities = entityEntries.Select(p => p.Entity).Cast<TEntity>();

            foreach (var entity in entities)
            {
                bool entityValidationResult = true;

                var validationContext = new ValidationContext(entity, _validationServiceProvider, new Dictionary<object, object>() { { typeof(KeyPropertyAttribute).Name, entities } });
                // validates with severity level if the entity implements IValidatableObjectWithSeverityLevel
                if (supportsSeverityLevels)
                {
                    entityValidationResult = OEEntityValidator.TryValidateObjectWithSeverityLevel((IValidatableObjectWithSeverityLevel)entity, validationContext, validationResults, true);
                }
                else
                {
                    // validates without severity levels if the entity doesn't implement IValidatableObjectWithSeverityLevel
                    ICollection<ValidationResult> simpleValidationResults = new Collection<ValidationResult>();
                    entityValidationResult = OEEntityValidator.TryValidateObject(entity, validationContext, simpleValidationResults, true);

                    foreach (var validationResult in simpleValidationResults)
                        validationResults.Add(new ValidationResultWithSeverityLevel(validationResult.ErrorMessage, validationResult.MemberNames, null, entity));
                }

                // pass the validation results to the validated entity for display if implements IWriteDataErrorInfo
                if (supportsWriteErrorInfo)
                    ((IWriteDataErrorInfo)entity).AddValidationErrors(validationResults);

                result = result && entityValidationResult;
            }
            return result;
        }
    }
}
