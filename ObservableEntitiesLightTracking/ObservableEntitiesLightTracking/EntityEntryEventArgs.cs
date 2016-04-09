using System;

namespace ObservableEntitiesLightTracking
{
    internal class EntityEntryEventArgs : EventArgs
    {
        internal EntityEntryEventArgs(OEEntityEntry entityEntry)
        {
            EntityEntry = entityEntry;
        }

        internal OEEntityEntry EntityEntry { get; set; }
    }
}
