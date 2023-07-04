using System.Xml.Linq;

namespace SmartSmsService.Model
{
    public class BulksSendBatchModel
    {
        public int NodeId { get; set; }
        public int? StartRow { get; set; }
        public int Count { get; set; }
        public int? PrefixNumber { get; set; }
        public byte? StartAge { get; set; }
        public byte? EndAge { get; set; }
        public byte? Gender { get; set; }
        public byte? MobileType { get; set; }

        static readonly Func<BulksSendBatchModel, List<XElement>> MCIGenderSeparation =
            model =>
            {
                var nodes = new List<XElement>();
                if (model.StartAge.HasValue) nodes.Add(new XElement("sa", model.StartAge));
                if (model.EndAge.HasValue) nodes.Add(new XElement("ea", model.EndAge));
                if (!model.Gender.HasValue) model.Gender = 2;
                if (!model.MobileType.HasValue) model.MobileType = 2;
                nodes.Add(new XElement("g", model.Gender));
                nodes.Add(new XElement("t", model.MobileType));
                return nodes;
            };

        public override string ToString()
        {

            return new XElement("r",
                    new XAttribute("id", "zone"),
                    new XElement("n", NodeId),
                    new XElement("c", Count),
                    StartRow.HasValue ? new XElement("sr", StartRow) : null,
                    PrefixNumber.HasValue ? new XElement("p", PrefixNumber) : null,
                    NodeId >= 1000000 && NodeId <= 1320000 ? MCIGenderSeparation(this) : null)
                .ToString();
        }
    }
}
