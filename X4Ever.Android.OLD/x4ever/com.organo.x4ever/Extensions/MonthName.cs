namespace com.organo.x4ever
{
    public static class MonthName
    {
        public static string ToMonthName(this int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "January";
                    break;

                case 2:
                    monthName = "February";
                    break;

                case 3:
                    monthName = "March";
                    break;

                case 4:
                    monthName = "April";
                    break;

                case 5:
                    monthName = "May";
                    break;

                case 6:
                    monthName = "June";
                    break;

                case 7:
                    monthName = "July";
                    break;

                case 8:
                    monthName = "August";
                    break;

                case 9:
                    monthName = "September";
                    break;

                case 10:
                    monthName = "October";
                    break;

                case 11:
                    monthName = "November";
                    break;

                case 12:
                    monthName = "December";
                    break;

                default:
                    monthName = "";
                    break;
            }
            return monthName;
        }

        public static string ToMonthShortName(this int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "Jan";
                    break;

                case 2:
                    monthName = "Feb";
                    break;

                case 3:
                    monthName = "Mar";
                    break;

                case 4:
                    monthName = "Apr";
                    break;

                case 5:
                    monthName = "May";
                    break;

                case 6:
                    monthName = "Jun";
                    break;

                case 7:
                    monthName = "Jul";
                    break;

                case 8:
                    monthName = "Aug";
                    break;

                case 9:
                    monthName = "Sep";
                    break;

                case 10:
                    monthName = "Oct";
                    break;

                case 11:
                    monthName = "Nov";
                    break;

                case 12:
                    monthName = "Dec";
                    break;

                default:
                    monthName = "";
                    break;
            }
            return monthName;
        }

        public static string ToMonthShortNameCapital(this int month)
        {
            string monthName = "";
            switch (month)
            {
                case 1:
                    monthName = "Jan";
                    break;

                case 2:
                    monthName = "Feb";
                    break;

                case 3:
                    monthName = "Mar";
                    break;

                case 4:
                    monthName = "Apr";
                    break;

                case 5:
                    monthName = "May";
                    break;

                case 6:
                    monthName = "Jun";
                    break;

                case 7:
                    monthName = "Jul";
                    break;

                case 8:
                    monthName = "Aug";
                    break;

                case 9:
                    monthName = "Sep";
                    break;

                case 10:
                    monthName = "Oct";
                    break;

                case 11:
                    monthName = "Nov";
                    break;

                case 12:
                    monthName = "Dec";
                    break;

                default:
                    monthName = "";
                    break;
            }
            return monthName.ToUpper();
        }
    }
}