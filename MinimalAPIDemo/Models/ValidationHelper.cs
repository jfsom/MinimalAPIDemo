using System.ComponentModel.DataAnnotations;

namespace MinimalAPIDemo.Models
{
    public static class ValidationHelper
    {
        // TryValidate method performs validation on a generic model object.
        // 'T' represents any type which means this method can be used with any model.
        public static bool TryValidate<T>(T model, out List<ValidationResult> validationResults)
        {
            // Create a ValidationContext for the model, which contains information about the model's type 
            // and any additional metadata
            var validationContext = new ValidationContext(model, null, null);

            // Initialize the list to store validation results (errors, if any)
            validationResults = new List<ValidationResult>();

            // Perform the validation using the Validator class, which uses reflection to find
            // and validate the properties of the model based on data annotations.
            // This method returns true if the model passes all validation rules; otherwise, false.
            // The 'true' parameter specifies that all properties should be validated.
            return Validator.TryValidateObject(model, validationContext, validationResults, true);
        }
    }
}
