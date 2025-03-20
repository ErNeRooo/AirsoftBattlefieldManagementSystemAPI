namespace AirsoftBattlefieldManagementSystemAPI.Exceptions
{
    public class NotASingleAvailableJoinCodeException : ValidationException
    {
        public NotASingleAvailableJoinCodeException(string message) : base(message)
        {
            
        }
    }
}
