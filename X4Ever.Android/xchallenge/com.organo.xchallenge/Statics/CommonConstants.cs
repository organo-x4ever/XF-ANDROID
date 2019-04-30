namespace com.organo.xchallenge.Statics
{
    public static class CommonConstants
    {
        public static string YES => "yes";
        public static string NO => "no";
        public static string ON => "on";
        public static string OFF => "off";
        public static string ENABLE => "enable";
        public static string DISABLE => "disable";

        public static string NOVALUE => "no value";


        public static string SPACE => "\n";

        public static string EMAIL_VALIDATION_REGEX =>
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static string Message => "message";
        public static string DATE_FORMAT_MMM_d_yyyy => "{0:MMM d, yyyy}";
    }
}