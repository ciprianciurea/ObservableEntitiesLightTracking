namespace ObservableEntitiesLightTracking
{
    public class OEModifiedPropertyInfo
    {
        public OEModifiedPropertyInfo(string name, object originalValue)
        {
            Name = name;
            OriginalValue = originalValue;
        }

        public string Name { get; private set; }
        public object OriginalValue { get; private set; }
    }
}
