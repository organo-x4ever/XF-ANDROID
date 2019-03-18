using com.organo.x4ever.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

//#if __IOS__
//using System.Drawing;
//using UIKit;
//using CoreGraphics;
//#endif

//#if __ANDROID__
//using Android.Graphics;
//#endif

//#if WINDOWS_UWP
//using System.Threading.Tasks;
//using Windows.Storage.Streams;
//using Windows.Graphics.Imaging;
//using System.Runtime.InteropServices.WindowsRuntime;
//#endif

namespace com.organo.x4ever.Helpers
{
    public static class ImageResizer
    {
        private static readonly IImageResizerHelpers ImageResizerHelpers;

        static ImageResizer()
        {
            ImageResizerHelpers = DependencyService.Get<IImageResizerHelpers>();
        }

        static byte[] Default(byte[] imageByte, ImageSize imageSize)
        {
            if (imageByte == null)
            {
                imageSize.ImageName = "no.png";
                imageByte = ImageResizerHelpers.ResizeImage(imageSize);
            }

            return imageByte;
        }

        public static ImageSource ResizeImage(string imageName)
        {
            var imageSize = App.Configuration.ImageSizes.FirstOrDefault(s => s.ImageID == imageName);
            var imageByte = Default(ImageResizerHelpers.ResizeImage(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static async Task<ImageSource> ResizeImageAsync(string imageName)
        {
            var imageSize = App.Configuration.ImageSizes.FirstOrDefault(s => s.ImageID == imageName);
            var imageByte = Default(await ImageResizerHelpers.ResizeImageAsync(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static ImageSource ResizeImage(string fileName, float width, float height)
        {
            var imageSize = new ImageSize()
            {
                Height = height,
                Width = width,
                ImageName = fileName
            };
            var imageByte = Default(ImageResizerHelpers.ResizeImage(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static async Task<ImageSource> ResizeImageAsync(string fileName, float width, float height)
        {
            var imageSize = new ImageSize()
            {
                Height = height,
                Width = width,
                ImageName = fileName
            };
            var imageByte = Default(await ImageResizerHelpers.ResizeImageAsync(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static ImageSource ResizeImage(string fileName, ImageSize imageSize)
        {
            if (fileName == null || fileName.Trim().Length == 0)
                return null;
            imageSize.ImageName = fileName;
            var imageByte = Default(ImageResizerHelpers.ResizeImage(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static async Task<ImageSource> ResizeImageAsync(string fileName, ImageSize imageSize)
        {
            imageSize.ImageName = fileName;
            var imageByte = Default(await ImageResizerHelpers.ResizeImageAsync(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static ImageSource ResizeImage(ImageSize imageSize)
        {
            var imageByte = Default(ImageResizerHelpers.ResizeImage(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static async Task<ImageSource> ResizeImageAsync(ImageSize imageSize)
        {
            var imageByte = Default(await ImageResizerHelpers.ResizeImageAsync(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static ImageSource ResizeImageFromRemote(ImageSize imageSize)
        {
            var imageByte = Default(ImageResizerHelpers.ResizeImageFromRemote(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        public static async Task<ImageSource> ResizeImageFromRemoteAsync(ImageSize imageSize)
        {
            var imageByte = Default(await ImageResizerHelpers.ResizeImageFromRemoteAsync(imageSize), imageSize);
            if (imageByte == null)
                return null;
            return ImageSource.FromStream(() => new MemoryStream(imageByte));
        }

        //#if __IOS__
        //        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        //        {
        //            UIImage originalImage = ImageFromByteArray(imageData);
        //            UIImageOrientation orientation = originalImage.Orientation;

        //            //create a 24bit RGB image
        //            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
        //                                                 (int)width, (int)height, 8,
        //                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
        //                                                 CGImageAlphaInfo.PremultipliedFirst))
        //            {

        //                RectangleF imageRect = new RectangleF(0, 0, width, height);

        //                // draw the image
        //                context.DrawImage(imageRect, originalImage.CGImage);

        //                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

        //                // save the image as a jpeg
        //                return resizedImage.AsJPEG().ToArray();
        //            }
        //        }

        //        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        //        {
        //            if (data == null)
        //            {
        //                return null;
        //            }

        //            UIKit.UIImage image;
        //            try
        //            {
        //                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Image load failed: " + e.Message);
        //                return null;
        //            }
        //            return image;
        //        }
        //#endif

        //#if __ANDROID__

        //		public static byte[] ResizeImageAndroid (byte[] imageData, float width, float height)
        //		{
        //			// Load the bitmap
        //			Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);
        //			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

        //			using (MemoryStream ms = new MemoryStream())
        //			{
        //				resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);
        //				return ms.ToArray ();
        //			}
        //		}

        //#endif

        //#if WINDOWS_UWP

        //        public static async Task<byte[]> ResizeImageWindows(byte[] imageData, float width, float height)
        //        {
        //            byte[] resizedData;

        //            using (var streamIn = new MemoryStream(imageData))
        //            {
        //                using (var imageStream = streamIn.AsRandomAccessStream())
        //                {
        //                    var decoder = await BitmapDecoder.CreateAsync(imageStream);
        //                    var resizedStream = new InMemoryRandomAccessStream();
        //                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
        //                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;
        //                    encoder.BitmapTransform.ScaledHeight = (uint)height;
        //                    encoder.BitmapTransform.ScaledWidth = (uint)width;
        //                    await encoder.FlushAsync();
        //                    resizedStream.Seek(0);
        //                    resizedData = new byte[resizedStream.Size];
        //                    await resizedStream.ReadAsync(resizedData.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);                  
        //                }                
        //            }

        //            return resizedData;
        //        }

        //#endif
    }

    //public class ValueProviderImplementation : IValueProvider
    //{
    //    public object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}