using System;

namespace ObservableEntitiesLightTracking
{
    internal class EntityEntryPropertyChangedEventArgs : EventArgs
    {
        internal EntityEntryPropertyChangedEventArgs(OEEntityEntry entityEntry, string propertyName)
        {
            EntityEntry = entityEntry;
            PropertyName = propertyName;
        }

        internal OEEntityEntry EntityEntry { get; set; }

        internal string PropertyName { get; set; }
    }
}
