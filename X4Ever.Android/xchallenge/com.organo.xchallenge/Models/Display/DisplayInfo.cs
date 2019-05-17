
namespace com.organo.xchallenge
{
    public class DeviceDisplayInfo
    {
        public DeviceDisplayInfo(int width, int height, float density, int orientation, int rotation, float? xdpi,
            float? ydpi)
        {
            Width = width;
            Height = height;
            Density = density;
            Orientation = orientation;
            Rotation = rotation;
            Xdpi = xdpi != null ? (float) xdpi : 0;
            Ydpi = ydpi != null ? (float) ydpi : 0;
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public float Density { get; set; }
        public int Orientation { get; set; }
        public int Rotation { get; set; }
        public float Xdpi { get; }
        public float Ydpi { get; }
    }
}