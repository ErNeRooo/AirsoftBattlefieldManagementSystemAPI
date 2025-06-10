namespace AirsoftBattlefieldManagementSystemAPI.Exceptions;

public class PlayerAlreadyInsideRoomException : Exception
{
    public PlayerAlreadyInsideRoomException(string message) : base(message)
    {
        
    }
}