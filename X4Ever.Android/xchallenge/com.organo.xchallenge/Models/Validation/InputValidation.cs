using System;

namespace com.organo.xchallenge.Models.Validation
{
    public class InputValidation
    {
        public bool IsValid(string emailaddress)
        {
            try
            {
                //MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}