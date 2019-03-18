
using com.organo.xchallenge.Helpers;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using com.organo.xchallenge.Droid;
using com.organo.xchallenge.Models;

[assembly: Dependency(typeof(ImageResizerHelpers))]

namespace com.organo.xchallenge.Droid
{
    public class ImageResizerHelpers : IImageResizerHelpers
    {
        //private Bitmap resizedImage;
        byte[] dataBytes;

        public Stream GetStream(string fileName)
        {
            var assembly = GetType();
            var resource = assembly.Namespace + ".Resources.drawable." + fileName;
            return assembly.Assembly.GetManifestResourceStream(resource);
        }

        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int) width, (int) height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Png, 100, ms);
                return ms.ToArray();
            }
        }

        public byte[] ResizeImage(string fileName, float width, float height)
        {
            byte[] imageData;
            Stream stream = GetStream(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return ResizeImage(imageData, width, height);
        }

        public async Task<byte[]> ResizeImageAsync(byte[] imageData, float width, float height)
        {
            await Task.Run(() => { dataBytes = ResizeImage(imageData, width, height); });
            return dataBytes;
        }

        public async Task<byte[]> ResizeImageAsync(string fileName, float width, float height)
        {
            byte[] imageData;
            Stream stream = GetStream(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return await ResizeImageAsync(imageData, width, height);
        }

        public byte[] ResizeImage(byte[] imageData, ImageSize imageSize)
        {
            return ResizeImage(imageData, imageSize.Width, imageSize.Height);
        }

        public byte[] ResizeImage(string fileName, ImageSize imageSize)
        {
            byte[] imageData;
            Stream stream = GetStream(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return ResizeImage(imageData, imageSize.Width, imageSize.Height);
        }

        public async Task<byte[]> ResizeImageAsync(byte[] imageData, ImageSize imageSize)
        {
            return await ResizeImageAsync(imageData, imageSize.Width, imageSize.Height);
        }

        public async Task<byte[]> ResizeImageAsync(string fileName, ImageSize imageSize)
        {
            byte[] imageData;
            Stream stream = GetStream(fileName);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return await ResizeImageAsync(imageData, imageSize.Width, imageSize.Height);
        }

        public byte[] ResizeImage(ImageSize imageSize)
        {
            if (imageSize.ImageName == null)
                return null;
            byte[] imageData;
            Stream stream = GetStream(imageSize.ImageName);
            if (stream == null)
                return null;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return ResizeImage(imageData, imageSize.Width, imageSize.Height);
        }

        public async Task<byte[]> ResizeImageAsync(ImageSize imageSize)
        {
            byte[] imageData;
            Stream stream = GetStream(imageSize.ImageName);
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            return await ResizeImageAsync(imageData, imageSize.Width, imageSize.Height);
        }

        //Bitmap ImageFromByteArray(byte[] data)
        //{
        //    if (data == null)
        //    {
        //        return null;
        //    }

        //    Bitmap image;
        //    try
        //    {
        //        image = BitmapFactory.DecodeByteArray(data, 0, data.Length);
        //    }
        //    catch (System.Exception e)
        //    {
        //        Console.WriteLine("Image load failed: " + e.Message);
        //        return null;
        //    }

        //    return image;
        //}

        public byte[] ResizeImageFromRemote(ImageSize imageSize)
        {
            return ResizeImage(ImageToBytes(imageSize), imageSize);
        }

        public async Task<byte[]> ResizeImageFromRemoteAsync(ImageSize imageSize)
        {
            return await ResizeImageAsync(await ImageToBytesAsync(imageSize), imageSize);
        }

        public byte[] ImageToBytes(ImageSize imageSize)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageSize.ImageName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageSize.ImageName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int) imageFileLength);
            return imageData;
        }

        public async Task<byte[]> ImageToBytesAsync(ImageSize imageSize)
        {
            return await Task.Factory.StartNew(() =>
            {
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(imageSize.ImageName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageSize.ImageName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int) imageFileLength);
                return imageData;
            });
        }
    }
}