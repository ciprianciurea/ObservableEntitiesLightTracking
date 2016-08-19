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
    public class OEEntitySet<TEntity> : OEEntitySet where TEntity : class
    {
        OEContext _parentContext;
        IServiceProvider _validationServiceProvider;

        internal OEEntitySet(OEContext parentContext)
        {
            _parentContext = parentContext;
            ValidateOnPropertyChanged = false;
        }

        public void Attach(TEntity entity)
        {
            if (entity != null)
                _parentContext.ChangeTracker.AttachEntry<TEntity>(entity, this);
        }

        public void Detach(TEntity entity)
        {
            if (entity != null)
            {
                var result = _parentContext.ChangeTracker.DetachEntry<TEntity>(entity);
            }
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                var result = _parentContext.ChangeTracker.AddEntry<TEntity>(entity, this);
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                var result = _parentContext.ChangeTracker.DeleteEntry<TEntity>(entity);
            }
        }

        public bool HasChanges()
        {
            return _parentContext.ChangeTracker.HasChanges<TEntity>();
        }

        public IEnumerable<OEEntityEntry> GetChanges()
        {
            var result = _parentContext.ChangeTracker.GetChanges<TEntity>();
            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var result = _parentContext.ChangeTracker.GetAll<TEntity>();
            return result;
        }

        public OEEntityState EntityState(TEntity entity)
        {
            var entityEntry = _parentContext.ChangeTracker.Entries().FirstOrDefault(p => p.Entity == entity);
            if (entityEntry == null)
                throw new ArgumentNullException("entity");
            return entityEntry.State;
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

            var entityEntries = _parentContext.ChangeTracker.Entries<TEntity>().Where(p => p.State == OEEntityState.Added || p.State == OEEntityState.Modified);
            var entities = entityEntries.Select(p => p.Entity).Cast<TEntity>();

            var unchangedEntityEntries = _parentContext.ChangeTracker.Entries<TEntity>().Where(p => p.State == OEEntityState.Unchanged);
            var unchangedEntities = unchangedEntityEntries.Select(p => p.Entity).Cast<TEntity>();

            var contextEntities = new List<TEntity>();
            contextEntities.AddRange(entities);
            contextEntities.AddRange(unchangedEntities);

            foreach (var entity in entities)
            {
                bool entityValidationResult = true;

                var validationContext = new ValidationContext(entity, _validationServiceProvider, new Dictionary<object, object>() { { typeof(KeyPropertyAttribute).Name, contextEntities } });

                ICollection<ValidationResult> simpleValidationResults = new Collection<ValidationResult>();
                entityValidationResult = OEEntityValidator.TryValidateObject(entity, validationContext, simpleValidationResults, true);

                ICollection<ValidationResultWithSeverityLevel> entityValidationResults = new Collection<ValidationResultWithSeverityLevel>();

                foreach (var validationResult in simpleValidationResults)
                    entityValidationResults.Add(new ValidationResultWithSeverityLevel(validationResult.ErrorMessage, validationResult.MemberNames, null, entity));

                // validates with severity level if the entity implements IValidatableObjectWithSeverityLevel
                if (supportsSeverityLevels)
                    entityValidationResult = OEEntityValidator.TryValidateObjectWithSeverityLevel((IValidatableObjectWithSeverityLevel)entity, validationContext, entityValidationResults, true, _parentContext.Configuration.ValidationSafeSeverityLevels);

                foreach (var validationResult in entityValidationResults)
                    validationResults.Add(validationResult);

                // pass the validation results to the validated entity for display if implements IWriteDataErrorInfo
                if (supportsWriteErrorInfo)
                    ((IWriteDataErrorInfo)entity).AddValidationErrors(entityValidationResults);

                result = result && entityValidationResult;
            }
            return result;
        }

        public override bool ValidateProperty(object instance, string propertyName, ICollection<ValidationResultWithSeverityLevel> validationResults)
        {
            bool result = true;

            bool supportsSeverityLevels = typeof(IValidatableObjectWithSeverityLevel).IsAssignableFrom(typeof(TEntity));
            bool supportsWriteErrorInfo = typeof(IWriteDataErrorInfo).IsAssignableFrom(typeof(TEntity));

            var entityEntries = _parentContext.ChangeTracker.Entries<TEntity>().Where(p => p.State == OEEntityState.Added || p.State == OEEntityState.Modified);
            var entities = entityEntries.Select(p => p.Entity).Cast<TEntity>();

            var unchangedEntityEntries = _parentContext.ChangeTracker.Entries<TEntity>().Where(p => p.State == OEEntityState.Unchanged);
            var unchangedEntities = unchangedEntityEntries.Select(p => p.Entity).Cast<TEntity>();

            var contextEntities = new List<TEntity>();
            contextEntities.AddRange(entities);
            contextEntities.AddRange(unchangedEntities);

            var validationContext = new ValidationContext(instance, _validationServiceProvider, new Dictionary<object, object>() { { typeof(KeyPropertyAttribute).Name, contextEntities } }) { MemberName = propertyName };
            ICollection<ValidationResult> simpleValidationResults = new Collection<ValidationResult>();

            var property = instance.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var propertyValue = property.GetValue(instance);
                result = OEEntityValidator.TryValidateProperty(propertyValue, validationContext, simpleValidationResults);
            }

            foreach (var validationResult in simpleValidationResults)
                validationResults.Add(new ValidationResultWithSeverityLevel(validationResult.ErrorMessage, validationResult.MemberNames, null, instance));

            // pass the validation results to the validated entity for display if implements IWriteDataErrorInfo
            if (supportsWriteErrorInfo)
                ((IWriteDataErrorInfo)instance).AddPropertyValidationErrors(propertyName, validationResults);

            return result;
        }
    }
}
