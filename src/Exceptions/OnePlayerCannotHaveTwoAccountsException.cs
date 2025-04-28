namespace AirsoftBattlefieldManagementSystemAPI.Exceptions;

public class OnePlayerCannotHaveTwoAccountsException : Exception
{
    public OnePlayerCannotHaveTwoAccountsException(string message) : base(message)
    {
        
    }
}