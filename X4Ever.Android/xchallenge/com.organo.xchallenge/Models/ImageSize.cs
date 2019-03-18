
namespace com.organo.xchallenge.Models
{

    //public class ImageSize
    //{
    //    public ImageSize()
    //    {
    //        TOP_BAR_LOGO = string.Empty;
    //        TOP_BAR_MENU = string.Empty;
    //        TOP_BAR_CLOSE = string.Empty;
    //        MAIN_PAGE_LOGO = string.Empty;
    //        MAIN_PAGE_XCHALLENGE_LOGO = string.Empty;
    //        MENU_PAGE_USER_IMAGE = string.Empty;
    //        USER_PROFILE_BADGE_ICON = string.Empty;
    //        MILESTONE_ACHEIVEMENT_BADGE_ICON = string.Empty;
    //        BADGE_HINT_WINDOW = string.Empty;
    //        BADGE_HINT_WINDOW_CLOSE = string.Empty;
    //        ENTRY_EMAIL_ICON = string.Empty;
    //        ENTRY_LOCK_ICON = string.Empty;
    //        MENU_ITEM_ICON = string.Empty;
    //        GENDER_IMAGE = string.Empty;
    //        UPLOAD_CAMERA_IMAGE = string.Empty;
    //        MEAL_PLAN_PAGE_MEAL_IMAGE = string.Empty;
    //        AUDIO_PLAYER_PAGE_COMMAND_IMAGE = string.Empty;
    //        VIDEO_PLAYER_PAGE_COMMAND_IMAGE = string.Empty;
    //        VIDEO_PLAYER_PAGE_EXPAND_LIST_IMAGE = string.Empty;
    //        VIDEO_PLAYER_PAGE_NOTE_PLAY_IMAGE = string.Empty;
    //        PICTURE_GALLERY_IMAGE = string.Empty;
    //        USER_SETTING_TAB_ICON = string.Empty;
    //        CHECKBOX_ICON = string.Empty;
    //        TESTIMONIAL_PERSON_IMAGE = string.Empty;
    //        BADGE_HINT_ICON = string.Empty;
    //        COUNTRY_FLAG_ICON = string.Empty;
    //        MEAL_PLAN_PAGE_MEAL_HEADER = string.Empty;
    //        WORKOUT_OPTIONS_IMAGE = string.Empty;
    //        WORKOUT_EXPAND_COLLAPSE_IMAGE = string.Empty;
    //        WORKOUT_PLAY_ICON = string.Empty;
    //        WORKOUT_VIDEO_WINDOW = string.Empty;
    //        PAGE_IMAGE_SIGN_UP = string.Empty;
    //        PAGE_IMAGE_PRODUCTS = string.Empty;
    //        PAGE_IMAGE_EARN_REWARDS = string.Empty;
    //        PAGE_IMAGE_LOSE_WEIGHT = string.Empty;
    //        PAGE_IMAGE_T_SHIRT = string.Empty;
    //        PAGE_IMAGE_T_SHIRTS_BUNDLE = string.Empty;
    //        PAGE_IMAGE_LINE = string.Empty;
    //        PAGE_IMAGE_BULLET = string.Empty;
    //        IMAGE_EYE_PASSWORD = string.Empty;
    //    }

    //    // Images Height and Width
    //    public string TOP_BAR_LOGO { get; set; }
    //    public string TOP_BAR_MENU { get; set; }
    //    public string TOP_BAR_CLOSE { get; set; }
    //    public string MAIN_PAGE_LOGO { get; set; }
    //    public string MAIN_PAGE_XCHALLENGE_LOGO { get; set; }
    //    public string MENU_PAGE_USER_IMAGE { get; set; }
    //    public string USER_PROFILE_BADGE_ICON { get; set; }
    //    public string MILESTONE_ACHEIVEMENT_BADGE_ICON { get; set; }
    //    public string BADGE_HINT_WINDOW { get; set; }
    //    public string BADGE_HINT_WINDOW_CLOSE { get; set; }
    //    public string ENTRY_EMAIL_ICON { get; set; }
    //    public string ENTRY_LOCK_ICON { get; set; }
    //    public string MENU_ITEM_ICON { get; set; }
    //    public string GENDER_IMAGE { get; set; }
    //    public string UPLOAD_CAMERA_IMAGE { get; set; }
    //    public string MEAL_PLAN_PAGE_MEAL_IMAGE { get; set; }
    //    public string AUDIO_PLAYER_PAGE_COMMAND_IMAGE { get; set; }
    //    public string VIDEO_PLAYER_PAGE_COMMAND_IMAGE { get; set; }
    //    public string VIDEO_PLAYER_PAGE_EXPAND_LIST_IMAGE { get; set; }
    //    public string VIDEO_PLAYER_PAGE_NOTE_PLAY_IMAGE { get; set; }
    //    public string PICTURE_GALLERY_IMAGE { get; set; }
    //    public string USER_SETTING_TAB_ICON { get; set; }
    //    public string CHECKBOX_ICON { get; set; }
    //    public string TESTIMONIAL_PERSON_IMAGE { get; set; }
    //    public string BADGE_HINT_ICON { get; set; }
    //    public string COUNTRY_FLAG_ICON { get; set; }
    //    public string MEAL_PLAN_PAGE_MEAL_HEADER { get; set; }
    //    public string WORKOUT_OPTIONS_IMAGE { get; set; }
    //    public string WORKOUT_EXPAND_COLLAPSE_IMAGE { get; set; }
    //    public string WORKOUT_PLAY_ICON { get; set; }
    //    public string WORKOUT_VIDEO_WINDOW { get; set; }
    //    public string PAGE_IMAGE_SIGN_UP { get; set; }
    //    public string PAGE_IMAGE_PRODUCTS { get; set; }
    //    public string PAGE_IMAGE_EARN_REWARDS { get; set; }
    //    public string PAGE_IMAGE_LOSE_WEIGHT { get; set; }
    //    public string PAGE_IMAGE_T_SHIRT { get; set; }
    //    public string PAGE_IMAGE_T_SHIRTS_BUNDLE { get; set; }
    //    public string PAGE_IMAGE_LINE { get; set; }
    //    public string PAGE_IMAGE_BULLET { get; set; }
    //    public string IMAGE_EYE_PASSWORD { get; set; }
    //}

    public interface IImageSize
    {
        string ImageID { get; set; }
        string ImageName { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        bool IsDynamic { get; set; }
    }

    public class ImageSize : IImageSize
    {
        public ImageSize()
        {
            IsDynamic = false;
            ImageID = string.Empty;
            ImageName = string.Empty;
            Width = 50;
            Height = 50;
        }

        public string ImageID { get; set; }
        public string ImageName { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public bool IsDynamic { get; set; }
    }
}