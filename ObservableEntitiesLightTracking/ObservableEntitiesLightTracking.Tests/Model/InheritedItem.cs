using System.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class InheritedItem : BaseItem
    {
        [Required]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        string _description;
        [Required]
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
    }
}
