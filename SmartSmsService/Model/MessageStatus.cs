namespace SmartSmsService.Model
{
    public class MessageStatus
    {
        public long SmsId { get; set; }
        public short Status { get; set; }
        public byte MessageLength { get; set; }
        public int RecipientCount { get; set; }
        public int SentCount { get; set; }
        public DateTime StartDate { get; set; }
    }
}
