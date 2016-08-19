using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class BaseItem : ObservableObject
    {
        int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _name;
        public virtual string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        private string _category;
        [IgnoreProperty]
        public string Category
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }
    }
}
