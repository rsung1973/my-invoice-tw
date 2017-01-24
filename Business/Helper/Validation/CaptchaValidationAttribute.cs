using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace Business.Helper.Validation
{
    public class CaptchaValidationAttribute : ValidationAttribute
    {
        private readonly string _encryptedCode;
        public CaptchaValidationAttribute(string encryptedCode)
        {
            _encryptedCode = encryptedCode;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_encryptedCode);
            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _encryptedCode)
                );
            }
            String codeValue = property.GetValue(validationContext.ObjectInstance, null) as String;

            if(!String.IsNullOrEmpty(codeValue))
            {
                string captcha = Encoding.Default.GetString(AppResource.Instance.Decrypt(Convert.FromBase64String(codeValue)));
                if (String.Compare(captcha, (String)value, true) == 0)
                {
                    return null;
                }
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}