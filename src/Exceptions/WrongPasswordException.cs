namespace AirsoftBattlefieldManagementSystemAPI.Exceptions
{
    public class WrongPasswordException : ValidationException
    {
        public WrongPasswordException(string message) : base(message)
        {
            
        }
    }
}
