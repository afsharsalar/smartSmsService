namespace SmartSmsService.Model
{
    public abstract class BaseEntity
    {
        public string LastErrorMessage { get; protected set; }
        public abstract bool IsValid();
    }
}
