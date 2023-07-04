using System.Xml.Linq;
using SmartSmsService.Model;

namespace SmartSmsService.Helper
{
    public static class XmsRequestXmlHelper
    {
        private const string XmsRequestXml =
            @"
        <xmsrequest>
            <userid>{0}</userid>
            <password>{1}</password>
            <action>{2}</action>
            {3}
        </xmsrequest>
        ";

        public static string CreateRequest(XmsRequestEntity xmsRequestEntity)
        {
            return XElement.Parse(string.Format(XmsRequestXml, xmsRequestEntity.UserId,
                xmsRequestEntity.Password,
                xmsRequestEntity.Action,
                xmsRequestEntity.Body)).ToString();
        }
    }
}
