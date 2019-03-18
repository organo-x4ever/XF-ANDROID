namespace com.organo.x4ever.Statics
{
    public static class CommonConstants
    {
        public static string YES => "yes";
        public static string NO => "no";
        public static string SPACE => "\n";

        public static string EMAIL_VALIDATION_REGEX =>
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static string Message => "message";
    }
}