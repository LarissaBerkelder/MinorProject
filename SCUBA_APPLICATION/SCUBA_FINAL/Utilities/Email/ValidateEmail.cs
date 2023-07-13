using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SCUBA_FINAL.Utilities.Email
{
    public class ValidateEmail
    {
        public bool IsValidEmail(string email)
        {
            // Regular expression pattern for basic email validation
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
