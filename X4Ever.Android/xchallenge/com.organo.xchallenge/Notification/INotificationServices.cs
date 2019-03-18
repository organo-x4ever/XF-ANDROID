namespace com.organo.xchallenge.Notification
{
    public interface INotificationServices
    {
        void Send(string title, string message);

        void Send(string title, string message, ActivityType activityType);

        void Send(string title, string message, ActivityType activityType, int notificationID);
    }

    public enum ActivityType
    {
        NONE,
        WEIGHT_SUBMISSION_REQUIRED
    }
}