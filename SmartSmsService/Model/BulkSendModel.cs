using System.Xml.Serialization;

namespace SmartSmsService.Model
{
    [XmlRoot("body")]
    public class BulkSendSmsModel
    {
        [XmlElement("message")]
        public MessageModel Message { get; set; }

        public class MessageModel
        {
            [XmlText]
            public long SmsId { get; set; }
            [XmlAttribute("rc")]
            public int RecipientCount { get; set; }
            [XmlAttribute("ml")]
            public byte MessageLength { get; set; }
        }
    }
}
