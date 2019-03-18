namespace com.organo.x4ever.ViewModels.Profile
{
    public enum ChartType
    {
        DonutChart,
        LineChart,
        PieChart,
        PointChart,
        RadarChart,
        RadialGaugeChart
    }

    public static class ChartDisplay
    {
        public static string Get(ChartType chartType)
        {
            switch (chartType)
            {
                case ChartType.DonutChart:
                    return DonutChart;

                case ChartType.LineChart:
                    return LineChart;

                case ChartType.PieChart:
                    return PieChart;

                case ChartType.PointChart:
                    return PointChart;

                case ChartType.RadarChart:
                    return RadarChart;

                case ChartType.RadialGaugeChart:
                    return RadialGaugeChart;

                default:
                    return "Chart";
            }
        }

        public static string DonutChart => "Donut Chart";
        public static string LineChart => "Line Chart";
        public static string PieChart => "Pie Chart";
        public static string PointChart => "Point Chart";
        public static string RadarChart => "Radar Chart";
        public static string RadialGaugeChart => "Radial Gauge Chart";
    }
}