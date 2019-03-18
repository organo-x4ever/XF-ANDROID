namespace com.organo.xchallenge.Statics
{
    public class MetaConstants
    {
        public static string LABEL => MetaEnum.UserMeta.ToString();
        public static string ADDRESS => MetaEnum.address.ToString();
        public static string AGE => MetaEnum.age.ToString();
        public static string CITY => MetaEnum.city.ToString();
        public static string COUNTRY => MetaEnum.country.ToString();
        public static string GENDER => MetaEnum.gender.ToString();
        public static string PRODUCT_PURCHASED => MetaEnum.OGXproductpurchased.ToString();
        public static string POSTAL_CODE => MetaEnum.postalcode.ToString();
        public static string STATE => MetaEnum.state.ToString();
        public static string WEIGHT_TO_LOSE => MetaEnum.weighttolose.ToString();
        public static string WEIGHT_TO_LOSE_UI => MetaEnum.weighttolose_ui.ToString();
        public static string WEIGHT_LOSS_GOAL => MetaEnum.weightlossgoal.ToString();
        public static string WEIGHT_LOSS_GOAL_UI => MetaEnum.weightlossgoal_ui.ToString();
        public static string WEIGHT_VOLUME_TYPE => TrackerEnum.weightvolumetype.ToString();
        public static string WHY_JOINING_CHALLENGE => MetaEnum.whyjoiningchallenge.ToString();
        public static string PROFILE_PHOTO => MetaEnum.profilephoto.ToString();
    }

    public enum MetaEnum
    {
        UserMeta,
        address,
        age,
        city,
        country,
        gender,
        OGXproductpurchased,
        postalcode,
        state,
        weighttolose,
        weighttolose_ui,
        weightlossgoal,
        weightlossgoal_ui,
        weightvolumetype,
        whyjoiningchallenge,
        profilephoto
    }
}