using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using com.organo.xchallenge.Controls;
using com.organo.xchallenge.Droid;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

[assembly:ExportRenderer(typeof(EntryWithImage),typeof(EntryWithImageRenderer))]

namespace com.organo.xchallenge.Droid
{
    public class EntryWithImageRenderer : EntryRenderer
    {
        private readonly Context _context;
        EntryWithImage element;
        private Xamarin.Forms.Image image { get; set; }
        private Bitmap bitmap { get; set; }

        public EntryWithImageRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (EntryWithImage) this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                var bitmapImage = GetDrawable(element.Image, element.Command);
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(bitmapImage, null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, bitmapImage, null);
                        break;
                }
            }

            editText.CompoundDrawablePadding = 25;
            Control.Background.SetColorFilter(element.LineColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage, Action action = null)
        {
            //Bitmap bitmap = ((BitmapDrawable)image.getDrawable()).getBitmap();

            byte[] imageData;
            var assembly = GetType();
            var resource = assembly.Namespace + ".Resources.drawable." + imageEntryImage;
            Stream stream = assembly.Assembly.GetManifestResourceStream(resource);

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                imageData = ms.ToArray();
            }

            image = new Image();
            image.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
            if (action != null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                image.GestureRecognizers.Add(new TapGestureRecognizer((sender, e) => { action?.Invoke(); }));
#pragma warning restore CS0618 // Type or member is obsolete

                // Set image as bitmap for ImageView

                //imageView.GestureRecognizers.Add(new TapGestureRecognizer());

                //imageView.SetImageBitmap(bitmap);
                //imageView.Click += (sender, e) => { action?.Invoke(); };

                //bitmap = imageView.GetDrawingCache(true);
                //// Create the token source.
                //CancellationTokenSource cts = new CancellationTokenSource();

                //// Pass the token to the cancelable operation.
                //var handler = new ImageLoaderSourceHandler();
                //var response = handler.LoadImageAsync(image.Source, _context, cts.Token).GetAwaiter();
                //bitmap = response.GetResult();
                GetBitmap(ImageSource.FromStream(() => new MemoryStream(imageData)));
                var bitmap1 = bitmap;
            }
            else
            {
                bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            }

            //int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            //var drawable = ContextCompat.GetDrawable(this.Context, resID);
            //var bitmap = ((BitmapDrawable) drawable).Bitmap;

            return new BitmapDrawable(Resources,
                Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }

        //private async void GetBitmap(object obj)
        //{
        //    CancellationToken token = (CancellationToken) obj;
        //    var handler = new ImageLoaderSourceHandler();
        //    bitmap = await handler.LoadImageAsync(image.Source, _context, token);
        //}


        async void GetBitmap(Xamarin.Forms.ImageSource image)
        {
            bitmap = await GetImageFromImageSource(image, Forms.Context);
        }

        private async Task<Bitmap> GetImageFromImageSource(ImageSource imageSource, Context context)
        {
            IImageSourceHandler handler;

            if (imageSource is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (imageSource is StreamImageSource)
            {
                handler = new StreamImagesourceHandler(); // sic
            }
            else if (imageSource is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler(); // sic
            }
            else
            {
                throw new NotImplementedException();
            }

            return await handler.LoadImageAsync(imageSource, context);
        }
    }
}