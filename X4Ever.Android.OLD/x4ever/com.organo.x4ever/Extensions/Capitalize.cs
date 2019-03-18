namespace com.organo.x4ever.Extensions
{
    public static class Capitalize
    {
        public static string ToCapital(this string text, char splitter = ' ')
        {
            string value = string.Empty;
            try
            {
                string[] texts = text.Split(splitter);
                int count = 0;
                foreach (var t in texts)
                {
                    count++;
                    value = string.Format("{0}{1}",
                                            value,
                                            string.Format("{0}{1}{2}",
                                                            t.Substring(0, 1).ToUpper(),
                                                            t.Substring(1, t.Length - 1),
                                                            (texts.Length > count ? splitter.ToString() : string.Empty)
                                                            )
                                        );
                }
            }
            catch
            {
                value = text;
            }
            return value;
        }
    }
}