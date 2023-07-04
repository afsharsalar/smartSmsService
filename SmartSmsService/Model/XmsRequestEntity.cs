using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSmsService.Model
{
    public class XmsRequestEntity : BaseEntity
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string Action { get; set; }
        public string Body { get; set; }

        public override bool IsValid()
        {
            LastErrorMessage = string.Empty;
            if (UserId < 0)
                LastErrorMessage = "UserId is not valid";
            else if (string.IsNullOrEmpty(Password))
                LastErrorMessage = "Password cannot be null or empty";
            else if (string.IsNullOrEmpty(Action))
                LastErrorMessage = "Action cannot be null or empty";
            if (!string.IsNullOrEmpty(LastErrorMessage)) return false;
            Body = Body ?? string.Empty;
            return true;
        }
    }
}
