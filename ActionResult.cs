using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL
{
    public struct ActionResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } // Entity Validation

        public object ReturnData { get; set; }
        public List<System.Data.Entity.Validation.DbValidationError> Errors { get; } // Property Validation
        // شما در 
        // struct
        // زمانی می توانید از 
        // Ctor
        // استفاده کنید که در بدنه آن تمامی خصوصیات را مقدار اولیه بدهید
        public ActionResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Errors = new List<System.Data.Entity.Validation.DbValidationError>();
            ReturnData = null;
        }

    }
}
