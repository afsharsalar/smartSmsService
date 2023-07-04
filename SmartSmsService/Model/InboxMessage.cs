namespace SmartSmsService.Model
{
    public class InboxMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public long SmsId { get; set; }
        public string Message { get; set; }
    }
}
