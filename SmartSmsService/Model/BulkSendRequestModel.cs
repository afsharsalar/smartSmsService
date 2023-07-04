using System.Xml.Linq;

namespace SmartSmsService.Model
{
    public class BulkSendSmsRequestModel : BaseEntity
    {
        public string Message { get; set; }
        public string Originator { get; set; }
        public RequestSendModel SendModel { get; set; }
        public RequestSendDate SendDate { get; set; }
        public ICollection<string> Recipients { get; set; }

        public IEnumerable<XElement> RecipientXmlTags
        {
            get
            {
                foreach (var recipient in Recipients)
                {
                    object content;
                    try
                    {
                        content = XElement.Parse(recipient);
                    }
                    catch (Exception)
                    {
                        content = long.Parse(recipient);
                    }
                    yield return new XElement("recipient", content);
                }
            }
        }

        public class RequestSendModel
        {
            /// <summary>
            /// Minute
            /// </summary>
            public byte Interval { get; set; }

            /// <summary>
            /// x1000
            /// </summary>
            public int Count { get; set; }
        }

        public class RequestSendDate
        {
            public DateTime StartDate { get; set; }
            public DateTime ExpireDate { get; set; }

            /// <summary>
            /// Hour
            /// </summary>
            public byte StartAllowTime { get; set; }

            /// <summary>
            /// Hour
            /// </summary>
            public byte EndAllowTime { get; set; }
        }

        public override bool IsValid()
        {
            LastErrorMessage = string.Empty;
            if (string.IsNullOrEmpty(Message))
                LastErrorMessage = "Message cannot be empty or null";
            else if (SendDate != null)
            {
                if (SendDate.StartAllowTime == 0 && SendDate.EndAllowTime == 0)
                {
                    SendDate.StartAllowTime = 7;
                    SendDate.StartAllowTime = 22;
                }

                SendDate.StartAllowTime = (byte)((SendDate.StartAllowTime % 24));
                SendDate.EndAllowTime = (byte)((SendDate.EndAllowTime % 24));

                if (SendDate.EndAllowTime == 0) SendDate.EndAllowTime = 24;

                if (SendDate.StartDate < new DateTime(2013, 1, 1))
                    LastErrorMessage = "StartDate is not valid date";
                else if (SendDate.StartDate > SendDate.ExpireDate)
                    LastErrorMessage = "StartDate > ExpireDate";
                else if (SendDate.StartAllowTime >= SendDate.EndAllowTime)
                    LastErrorMessage = "StartAllowTime > EncAllowTime";
            }
            else if (Recipients == null)
                LastErrorMessage = "Recipients cannot be null";
            else if (Recipients.ToList().Any(recipient =>
            {
                try
                {
                    XElement.Parse(recipient);
                }
                catch (Exception)
                {
                    try
                    {
                        long.Parse(recipient);
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                }
                return false;
            }))
                LastErrorMessage = "Recipients is not valid values";
            if (!string.IsNullOrEmpty(LastErrorMessage)) return false;
            return true;

        }
    }
}
