#if NET45
using System.Data.Entity.Validation;
using System.Text;
using System.Linq;

namespace FrameworkExtKit.Core.Data.Entity.Validation {
    public static class DbEntityValidationException_Ext {

        ///
        /// <summary>
        /// Get detailed validation error messages, mainly used for debuggin purpose
        /// </summary>
        /// <remarks>.Net 4.5+ only</remarks>
        public static string GetValidationErrorDetails (this DbEntityValidationException exception) {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(string.Format("{0} Invalid Entities: \n\n", exception.EntityValidationErrors.Count()));

            foreach (var validationResult in exception.EntityValidationErrors) {
                if (validationResult.IsValid == false) {
                    builder.AppendLine(validationResult.Entry.Entity.GetType().ToString());

                    foreach (var error in validationResult.ValidationErrors) {
                        builder.AppendLine(error.ErrorMessage);
                    }
                    builder.AppendLine("");
                }
            }
            return builder.ToString();
        }
    }
}
#endif