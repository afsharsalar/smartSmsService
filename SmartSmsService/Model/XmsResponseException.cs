namespace SmartSmsService.Model
{
    public class XmsResponseException : ApplicationException
    {
        public short Code { get; private set; }
        public string Description { get; private set; }

        public XmsResponseException(short code, string description)
            : base(string.Format("This is WebService Application Error\n{0}", description))
        {
            Code = code;
            Description = description;
        }
    }
}
