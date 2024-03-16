using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Core.Validators
{
    public static class ValidationExtensions
    {
        public static string Validate(this Exhibit exhibit)
        {
            string errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(exhibit.Title))
            {
                errorMessage += "Title is required.\n";
            }
            if (string.IsNullOrWhiteSpace(exhibit.Description))
            {
                errorMessage += "Description is required.\n";
            }
            if (exhibit.MaxCapacity < 1)
            {
                errorMessage += "Maximum # of people at once must be greater than 0.\n";
            }
            if (exhibit.InitialDuration < 1)
            {
                errorMessage += "Expected duration must be greater than 0.\n";
            }
            if (exhibit.InsuranceFormRequired == null)
            {
                errorMessage += "Is insurance form release required needs to be filled.\n";
            }
            if (exhibit.AgeRequired != null)
            {
                if (exhibit.AgeRequired == true)
                {
                    if (exhibit.AgeMinimum == null || exhibit.AgeMinimum < 1)
                    {
                        errorMessage += "Since the exhibit is marked as age required, a minimum age needs to be set\n";
                    }
                }
            }
            else
            {
                errorMessage += "Is age required needs to be filled.\n";
            }
            return errorMessage;
        }
    }
}
