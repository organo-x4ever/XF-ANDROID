
namespace com.organo.x4ever.Models
{
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