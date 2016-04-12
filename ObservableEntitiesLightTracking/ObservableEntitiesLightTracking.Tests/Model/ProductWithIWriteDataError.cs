using ObservableEntitiesLightTracking.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithIWriteDataError : ObservableObject, IWriteDataErrorInfo
    {
        int _id;
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
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        public ICollection<ValidationResultWithSeverityLevel> ValidationResults { get; set; }

        public void AddValidationErrors(ICollection<ValidationResultWithSeverityLevel> validationResults)
        {
            ValidationResults = validationResults;
        }

        public void AddPropertyValidationErrors(string propertyName, ICollection<ValidationResultWithSeverityLevel> validationResults)
        {
            var itemsToRemove = ValidationResults.Where(p => p.MemberNames.Contains(propertyName));
            foreach (var item in itemsToRemove)
                ValidationResults.Remove(item);
        }
    }
}
