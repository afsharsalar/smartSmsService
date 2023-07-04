using SmartSmsService.Model;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SmartSmsService.Helper
{
    public static class XmsResponseXmlHelper
    {
        public static TEntity ParseData<TEntity>(this string xmsResponseData, Func<XElement, TEntity> bodyParser = null, XmlRootAttribute xmlRootAttribute = null)
        {
            if (string.IsNullOrEmpty(xmsResponseData)) throw new ArgumentNullException("xmsResponseData");
            XElement xElementResponseData;
            try
            {
                xElementResponseData = XElement.Parse(xmsResponseData);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Parse xmsresponse faild", exception);
            }
            var xElementCode = xElementResponseData.Element("code");
            if (xElementCode == null) throw new XmsResponseException(-1, "Not found code tag in xms response");
            var xAttribute = xElementCode.Attribute("id");
            if (xAttribute == null) throw new XmsResponseException(-1, "Not found code tag in xms response");
            if (xAttribute.Value != "0") throw new XmsResponseException(Convert.ToInt16(xAttribute.Value), xElementCode.Value);
            var xElementBody = xElementResponseData.Element("body");

            return bodyParser == null ? DefaultBodyParser<TEntity>(xElementBody, xmlRootAttribute) : bodyParser(xElementBody);
        }

        private static TEntity DefaultBodyParser<TEntity>(XElement xElementBody, XmlRootAttribute xmlRootAttribute = null)
        {
            if (xmlRootAttribute == null) xmlRootAttribute = new XmlRootAttribute("body");
            var xmlSerializer = new XmlSerializer(typeof(TEntity), xmlRootAttribute);
            if (xElementBody == null) throw new XmsResponseException(-2, "Not found body tag in xms response");
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                xElementBody.Save(stringWriter);
            }
            return (TEntity)xmlSerializer.Deserialize(new StringReader(builder.ToString()));
        }
    }
}
