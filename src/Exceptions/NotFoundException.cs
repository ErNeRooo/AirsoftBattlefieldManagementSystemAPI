namespace AirsoftBattlefieldManagementSystemAPI.Exceptions
{
    public class NotFoundException : ValidationException
    {
        public NotFoundException(string message) : base(message)
        {
            
        }
    }
}
