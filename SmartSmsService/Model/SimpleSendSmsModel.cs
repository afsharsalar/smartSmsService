using System.Xml.Serialization;

namespace SmartSmsService.Model
{
    [XmlRoot(ElementName = "recipient")]
    public class SimpleSendSmsModel
    {
        [XmlText]
        public long SmsId { get; set; }

        [XmlAttribute("mobile")]
        public long Mobile { get; set; }

        [XmlAttribute("ml")]
        public byte MessageLength { get; set; }

        [XmlAttribute("status")]
        public short StatusId { get; set; }
    }
}
