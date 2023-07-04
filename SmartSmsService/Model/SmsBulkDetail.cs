namespace SmartSmsService.Model
{
    public class SmsBulkDetail
    {
        public string Mobile { get; set; }
        public short StatusId { get; set; }
        public long DeliveredId { get; set; }
        public DateTime DeliveredDate { get; set; }
    }
}
