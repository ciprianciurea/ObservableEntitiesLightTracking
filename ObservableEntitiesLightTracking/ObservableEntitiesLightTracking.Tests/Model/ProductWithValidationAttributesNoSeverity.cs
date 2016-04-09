using System;
using System.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithValidationAttributesNoSeverity : ObservableObject
    {
        int _id;
        [Range(0.0, Double.MaxValue)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal _unitPrice;
        [Range(0.0, Double.MaxValue)]
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }
    }
}
