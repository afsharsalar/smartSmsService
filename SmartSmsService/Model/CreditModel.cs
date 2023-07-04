using System.Xml.Serialization;

namespace SmartSmsService.Model
{
    [XmlRoot(ElementName = "body")]
    public class CreditModel
    {
        [XmlElement]
        public decimal SendCredit { get; set; }

        [XmlElement]
        public decimal RecieveCredit { get; set; }
    }

    [XmlRoot(Namespace = "", ElementName = "recipient")]
    public class SimpleSmsSendRequestModel
    {
        [XmlText]
        public string Message { get; set; }

        [XmlAttribute("originator")]
        public string Originator { get; set; }

        [XmlAttribute("mobile")]
        public long Mobile { get; set; }

        [XmlAttribute("senddate")]
        public DateTime? SendDate { get; set; }
    }
}
