using SmartSmsService.Helper;
using SmartSmsService.Model;

using SmsWebService;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace SmartSmsService.Service
{
    public static class SmsWebService
    {
        public static async Task<CreditModel> GetCredit(int userId, string password)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "getcredit"
            };
            CreditModel creditModel = null;
            if (requestEntity.IsValid())
            {
                string requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                creditModel = (await XmsRequest(requestData)).ParseData<CreditModel>();
            }
            else
            {
                // TODO: This is your business code
            }

            return creditModel;
        }

        public static async Task<List<SimpleSendSmsModel>> SimpleSend(
            int userId,
            string password,
            IEnumerable<SimpleSmsSendRequestModel> simpleSmsSendRequestModels)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smssend",
                Body = new XElement("body",
                    new XElement("type", "oto"),
                    simpleSmsSendRequestModels
                        .Select(simpleSendSms =>
                        {
                            var recipient = new XElement(
                                "recipient",
                                new XAttribute("mobile",
                                    simpleSendSms.Mobile),
                                string.IsNullOrEmpty(
                                    simpleSendSms.Originator)
                                    ? null
                                    : new XAttribute("originator",
                                        simpleSendSms.Originator),
                                simpleSendSms.SendDate != null
                                    ? new XAttribute("senddate",
                                        simpleSendSms.SendDate.Value)
                                    : null,
                                HttpUtility.HtmlEncode(
                                    simpleSendSms.Message));
                            return recipient;
                        })).ToString()
            };
            List<SimpleSendSmsModel> simpleSendSmsModel = null;
            if (requestEntity.IsValid())
            {
                string requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                simpleSendSmsModel = (await XmsRequest(requestData)).ParseData(SimpleSendParseResponseBodyXml);
            }
            else
            {
                // TODO: This is your business code
            }

            return simpleSendSmsModel;
        }

        public static async Task<BulkSendSmsModel> BulkSend(
            int userId,
            string password,
            BulkSendSmsRequestModel bulkSendRequestModel)
        {
            if (!bulkSendRequestModel.IsValid())
                return null;
            string body = new XElement("body",
                new XElement("type", "otm"),
                new XElement("message",
                    new XAttribute("originator", bulkSendRequestModel.Originator),
                    bulkSendRequestModel.Message),
                bulkSendRequestModel.SendModel != null
                    ? new XElement("sendmodel",
                        new XAttribute("interval",
                            bulkSendRequestModel.SendModel.Interval),
                        new XAttribute("count", bulkSendRequestModel.SendModel.Count))
                    : null,
                bulkSendRequestModel.SendDate != null
                    ? new XElement("senddate",
                        new XElement("start",
                            bulkSendRequestModel.SendDate.StartDate),
                        new XElement("expire",
                            bulkSendRequestModel.SendDate.ExpireDate),
                        new XElement("allowtime",
                            string.Format("{0}-{1}",
                                bulkSendRequestModel.SendDate.StartAllowTime,
                                bulkSendRequestModel.SendDate.EndAllowTime)))
                    : null,
                bulkSendRequestModel.RecipientXmlTags
            ).ToString();

            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smssend",
                Body = body
            };
            BulkSendSmsModel bulkSendModel = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                bulkSendModel = (await XmsRequest(requestData)).ParseData<BulkSendSmsModel>();
            }
            else
            {
                // TODO: This is your business code
            }

            return bulkSendModel;
        }

        public static async Task<List<node>> GetTreeNodes(int userId, string password, int treeNodeId = 0)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "treenodes",
                Body = string.Format("<body><node id=\"{0}\" /></body>", treeNodeId)
            };
            List<node> treeNodesModel = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                treeNodesModel = (await XmsRequest(requestData)).ParseData<List<node>>();
            }
            else
            {
                // TODO: business logic
            }

            return treeNodesModel;
        }

        public static async Task<int> GetTreeNodeCount(int userId, string password, BulksSendBatchModel model)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "nodecount",
                Body = string.Format("<body>{0}</body>", model)
            };
            var treeNodeCount = 0;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                treeNodeCount = (await XmsRequest(requestData))
                    .ParseData(body =>
                    {
                        var xElement = body.Element("count");
                        return xElement != null ? Convert.ToInt32(xElement.Value) : 0;
                    });
            }
            else
            {
                // TODO: business logic
            }

            return treeNodeCount;
        }

        public static async Task<List<MessageStatus>> GetMessagesStatus(int userId, string password,
            IEnumerable<long> messagesId)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smsstatus",
                Body = string.Format("<body>{0}</body>",
                    string.Join("\n", messagesId.Select(msgId =>
                        new XElement("message", msgId).ToString())))
            };

            List<MessageStatus> statuses = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                statuses = (await XmsRequest(requestData))
                    .ParseData(body => body.Elements("message").Select(MessageStatus)).ToList();
            }
            else
            {
                // TODO: Business logic code
            }

            return statuses;
        }

        public static async Task<List<MessageId>> GetMessagesId(int userId, string password, IEnumerable<long> doersId)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smsstatus",
                Body = string.Format("<body>{0}</body>",
                    string.Join("\n", doersId.Select(doerId =>
                        new XElement("d", doerId).ToString())))
            };

            List<MessageId> ids = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                ids = (await XmsRequest(requestData)).ParseData(MessageIds);
            }
            else
            {
                // TODO: Business logic code
            }

            return ids;
        }

        public static async Task<List<InboxMessage>> GetInboxMessage(int userId, string password, long startRow = -1,
            int count = 50)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smsreceive",
                Body = new XElement("body",
                    startRow == -1 ? null : new XElement("startrow", startRow),
                    new XElement("count", count)).ToString()
            };

            List<InboxMessage> results = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                results = (await XmsRequest(requestData)).ParseData(InboxMessage);
            }
            else
            {

            }

            return results;
        }

        public static async Task<List<SmsBulkDetail>> GetBulkDetail(int userId, string password, long smsId,
            byte pageIndex, BulkDetailPageType bulkDetailPageType)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "smsbulkdetail",
                Body = new XElement("body",
                    new XElement("pagetype", (byte)bulkDetailPageType),
                    new XElement("pageindex", pageIndex <= 0 ? 1 : pageIndex),
                    new XElement("smsid", smsId)).ToString()
            };
            List<SmsBulkDetail> results = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                results = (await XmsRequest(requestData)).ParseData(SmsBulkDetail);
            }
            else
            {

            }

            return results;
        }

        public static async Task<Dictionary<short, string>> GetCodeDescription(int userId, string password,
            IEnumerable<short> statusIds)
        {
            var requestEntity = new XmsRequestEntity
            {
                UserId = userId,
                Password = password,
                Action = "getcodedescription",
                Body = new XElement("body",
                    statusIds.Select(statusId => new XElement("code", new XAttribute("id", statusId)))).ToString()
            };
            Dictionary<short, string> results = null;
            if (requestEntity.IsValid())
            {
                var requestData = XmsRequestXmlHelper.CreateRequest(requestEntity);
                results = (await XmsRequest(requestData))
                    .ParseData(body => body.Elements("code")
                        .ToDictionary(p =>
                        {
                            var xAttribute = p.Attribute("id");
                            return xAttribute != null ? Convert.ToInt16(xAttribute.Value) : (short)0;
                        }, q => q.Value));
            }
            else
            {
                // TODO:    
            }

            return results;
        }


        private static List<SmsBulkDetail> SmsBulkDetail(XElement body)
        {
            var results = new List<SmsBulkDetail>();
            body.Elements("r")
                .ToList()
                .ForEach(r =>
                {
                    var smsBulkDetail = new SmsBulkDetail();
                    var xAttribute = r.Attribute("m");
                    if (xAttribute != null) smsBulkDetail.Mobile = xAttribute.Value;
                    xAttribute = r.Attribute("status");
                    if (xAttribute != null)
                        smsBulkDetail.StatusId = Convert.ToInt16(xAttribute.Value);
                    xAttribute = r.Attribute("did");
                    if (xAttribute != null)
                        smsBulkDetail.DeliveredId = Convert.ToInt64(xAttribute.Value);
                    xAttribute = r.Attribute("dd");
                    if (xAttribute != null)
                        smsBulkDetail.DeliveredDate = Convert.ToDateTime(xAttribute.Value);
                    results.Add(smsBulkDetail);
                });
            return results;
        }

        private static List<InboxMessage> InboxMessage(XElement body)
        {
            var results = new List<InboxMessage>();
            body.Elements("message").ToList()
                .ForEach(msg =>
                {
                    var inboxMessage = new InboxMessage();
                    var xAttribute = msg.Attribute("from");
                    if (xAttribute != null) inboxMessage.From = xAttribute.Value;
                    xAttribute = msg.Attribute("to");
                    if (xAttribute != null) inboxMessage.To = xAttribute.Value;
                    xAttribute = msg.Attribute("date");
                    if (xAttribute != null) inboxMessage.Date = Convert.ToDateTime(xAttribute.Value);
                    xAttribute = msg.Attribute("id");
                    if (xAttribute != null) inboxMessage.SmsId = Convert.ToInt64(xAttribute.Value);
                    inboxMessage.Message = msg.Value;
                    results.Add(inboxMessage);
                });
            return results;
        }

        private static List<MessageId> MessageIds(XElement body)
        {
            var messageIds = new List<MessageId>();
            body.Elements("r").ToList()
                .ForEach(r =>
                {
                    var messageId = new MessageId();
                    var xAttribute = body.Attribute("sid");
                    if (xAttribute != null)
                        messageId.SmsId = Convert.ToInt64(xAttribute.Value);
                    xAttribute = body.Attribute("d");
                    if (xAttribute != null)
                        messageId.DoerId = Convert.ToInt64(xAttribute.Value);
                    messageIds.Add(messageId);
                });
            return messageIds;
        }

        private static MessageStatus MessageStatus(XElement xElementMessage)
        {
            var messageStatus = new MessageStatus();
            var xAttribute = xElementMessage.Attribute("id");
            if (xAttribute != null)
                messageStatus.SmsId = Convert.ToInt64(xAttribute.Value);
            xAttribute = xElementMessage.Attribute("status");
            if (xAttribute != null)
                messageStatus.Status = Convert.ToInt16(xAttribute.Value);
            xAttribute = xElementMessage.Attribute("messagelength");
            if (xAttribute != null)
                messageStatus.MessageLength = Convert.ToByte(xAttribute.Value);
            xAttribute = xElementMessage.Attribute("recipientcount");
            if (xAttribute != null)
                messageStatus.RecipientCount = Convert.ToInt32(xAttribute.Value);
            xAttribute = xElementMessage.Attribute("sentcount");
            if (xAttribute != null)
                messageStatus.SentCount = Convert.ToInt32(xAttribute.Value);
            xAttribute = xElementMessage.Attribute("startdate");
            if (xAttribute != null)
                messageStatus.StartDate = Convert.ToDateTime(xAttribute.Value);

            return messageStatus;
        }

        

        private static async Task<string> XmsRequest(string requestData)
        {

            SmsSoapClient smsSoap = null;
            string responseData;
            try
            {
                smsSoap = new SmsSoapClient(SmsSoapClient.EndpointConfiguration.SmsSoap12);
                var result = await smsSoap.XmsRequestAsync(requestData);

                responseData = Convert.ToString(result.Body.XmsRequestResult);
            }
            catch (Exception)
            {
                // TODO: Log this error for help(from me to you)
                if (smsSoap != null) await smsSoap.CloseAsync();
                throw;
            }

            return responseData;
        }

        private static List<SimpleSendSmsModel> SimpleSendParseResponseBodyXml(XElement xElementBody)
        {
            return xElementBody.Elements("recipient")
                .Select(xElementRecipient =>
                {
                    var model = new SimpleSendSmsModel { SmsId = Convert.ToInt64(xElementRecipient.Value) };
                    var xAttributeMobile = xElementRecipient.Attribute("mobile");
                    if (xAttributeMobile != null)
                        model.Mobile = Convert.ToInt64(xAttributeMobile.Value);
                    var xAttributeMessageLength = xElementRecipient.Attribute("ml");
                    if (xAttributeMessageLength != null)
                        model.MessageLength = Convert.ToByte(xAttributeMessageLength.Value);
                    var xAttributeStatus = xElementRecipient.Attribute("status");
                    if (xAttributeStatus != null)
                        model.StatusId = Convert.ToInt16(xAttributeStatus.Value);
                    return model;
                }).ToList();
        }
    }
}
