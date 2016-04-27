namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductPoco
    {
        int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }
    }
}
