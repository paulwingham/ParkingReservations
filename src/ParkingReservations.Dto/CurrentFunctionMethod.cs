namespace Paul.ParkingReservations.Dto;

public static class CurrentFunctionMethod
{
    public static string GetCaller(object caller, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "", [System.Runtime.CompilerServices.CallerLineNumber] int memberLine = 0)
    {
        return caller.GetType().Name + " - " + memberName + " - Line:" + memberLine;
    }
}