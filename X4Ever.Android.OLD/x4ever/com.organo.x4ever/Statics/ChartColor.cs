using SkiaSharp;

namespace com.organo.x4ever.Statics
{
    public static class ChartColor
    {
        public static SKColor Get(int index)
        {
            var color = SKColor.Parse("#" + ColorShade._1);
            switch (index)
            {
                case 0:
                    color = SKColor.Parse("#" + ColorShade._0);
                    break;

                case 1:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 2:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 3:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 4:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 5:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 6:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 7:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 8:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 9:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 10:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 11:
                    color = SKColor.Parse("#" + GetString(index));
                    break;

                case 12:
                    color = SKColor.Parse("#" + GetString(index));
                    break;
            }

            return color;
        }

        public static string GetString(int index)
        {
            var color = ColorShade._1;
            if (index > 12 || index < 1)
                index = 1;
            switch (index)
            {
                case 1:
                    color = ColorShade._1;
                    break;

                case 2:
                    color = ColorShade._2;
                    break;

                case 3:
                    color = ColorShade._3;
                    break;

                case 4:
                    color = ColorShade._4;
                    break;

                case 5:
                    color = ColorShade._5;
                    break;

                case 6:
                    color = ColorShade._6;
                    break;

                case 7:
                    color = ColorShade._7;
                    break;

                case 8:
                    color = ColorShade._8;
                    break;

                case 9:
                    color = ColorShade._9;
                    break;

                case 10:
                    color = ColorShade._10;
                    break;

                case 11:
                    color = ColorShade._11;
                    break;

                case 12:
                    color = ColorShade._12;
                    break;
            }

            return color;
        }
    }
}