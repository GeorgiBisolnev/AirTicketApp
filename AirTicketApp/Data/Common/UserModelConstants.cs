namespace AirTicketApp.Data.Common
{
    public static class UserModelConstants
    {
        public const int MaxPassportNumLength = 20;
        public const int MaxFirstNameStringLenght = 50;
        public const int MaxLastNameStringLenght = 50;
        public const string PassportRegex = @"^[a-zA-Z0-9]{5,20}$";
        public const string PhoneNumberRegex = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
        
    }
}
