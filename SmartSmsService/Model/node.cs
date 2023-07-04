using System.Xml.Serialization;

namespace SmartSmsService.Model
{
    [XmlRoot("node")]
    public class node
    {
        [XmlElement("id")]
        public int Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("mc")]
        public int MobileCount { get; set; }
    }
}
