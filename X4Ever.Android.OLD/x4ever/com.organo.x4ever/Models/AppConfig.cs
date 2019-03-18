namespace com.organo.x4ever.Models
{
    public class AppConfig
    {
        public AppConfig()
        {
            BaseUrl = string.Empty;
            AuthenticationUrl = string.Empty;
            ApplicationRequestHeader = string.Empty;
            ApplicationVersion = string.Empty;
            AcceptedTokenName = string.Empty;
            TokenHeaderName = string.Empty;
            TokenExpiryHeaderName = string.Empty;
            AccessTokenType = string.Empty;
            VideoUrl = string.Empty;
            AudioUrl = string.Empty;
            FileUploadUrl = string.Empty;
            UserImageUrl = string.Empty;
            WelcomeVideoUrl = string.Empty;
            DefaultLanguage = string.Empty;
            DefaultWeightVolume = string.Empty;
        }

        public string BaseUrl { get; set; }
        public string AuthenticationUrl { get; set; }
        public string ApplicationRequestHeader { get; set; }
        public string ApplicationVersion { get; set; }
        public string AcceptedTokenName { get; set; }
        public string TokenHeaderName { get; set; }
        public string TokenExpiryHeaderName { get; set; }
        public string AccessTokenType { get; set; }
        public string ExecutionTimeHeader { get; set; } = "ExecutionTime";
        public string VideoUrl { get; set; }
        public string AudioUrl { get; set; }
        public string FileUploadUrl { get; set; }
        public string UserImageUrl { get; set; }
        public string WelcomeVideoUrl { get; set; }
        public string DocumentUrl { get; set; }
        public string TestimonialPhotoUrl { get; set; }
        public string TestimonialVideoUrl { get; set; }
        public string DefaultLanguage { get; set; }
        public string DefaultWeightVolume { get; set; }
        public int WeightSubmitInterval { get; set; }
        public IntervalPeriodType WeightSubmitIntervalType { get; set; }
        public float TargetDateCalculation { get; set; }

        public double MINIMUM_CURRENT_WEIGHT { get; set; }
        public double MAXIMUM_CURRENT_WEIGHT { get; set; }
        public double MINIMUM_WEIGHT_LOSE { get; set; }
        public double MAXIMUM_WEIGHT_LOSE { get; set; }
        public short MINIMUM_AGE { get; set; }
        public short MAXIMUM_AGE { get; set; }
    }

    public enum IntervalPeriodType
    {
        Years,
        Months,
        Days,
        Hours,
        Minutes
    }
}