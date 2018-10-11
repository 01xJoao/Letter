using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using LetterApp.Core.Services.Interfaces;
using LetterApp.iOS.Helpers;
using UIKit;

namespace LetterApp.iOS.Services
{
    public class PicturePicker : IPicturePicker
    {
        private TaskCompletionSource<string> _taskCompletionSource;
        private UIImagePickerController imagePicker;

        public Task<string> GetImageStreamSync(bool resize)
        {
            imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                imagePicker.NavigationBar.PrefersLargeTitles = true;

            imagePicker.NavigationBar.TintColor = Colors.White;

            // Set event handlers
            if (resize)
            {
                imagePicker.FinishedPickingMedia -= OnImagePickerFinishedPickingMedia;
                imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
            }
            else
            {
                imagePicker.FinishedPickingMedia -= DecreaseImageQuality;
                imagePicker.FinishedPickingMedia += DecreaseImageQuality;
            }

            imagePicker.Canceled -= OnImagePickerCancelled;
            imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentViewController(imagePicker, true, null);

            // Return Task object
            _taskCompletionSource = new TaskCompletionSource<string>();
            return _taskCompletionSource.Task;
        }

        private void DecreaseImageQuality(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                if(image.Size.Height > 1600)
                    image = MaxResizeImage(image, (int)image.Size.Height/2, (int)image.Size.Width/2);

                // Convert UIImage to .NET Stream object
                NSData data = image.AsJPEG(0.75f);
                //Stream stream = data.AsStream();
                var base64string = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);

                // Set the Stream as the completion of the Task
                _taskCompletionSource.SetResult(base64string);
            }
            else
            {
                _taskCompletionSource.SetResult(null);
            }

            imagePicker.DismissViewController(true, null);
        }

        private void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                image = MaxResizeImage(image, LocalConstants.Profile_PictureSize, LocalConstants.Profile_PictureSize);

                // Convert UIImage to .NET Stream object
                NSData data = image.AsJPEG(0.6f);
                //Stream stream = data.AsStream();
                var base64string = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);

                // Set the Stream as the completion of the Task
                _taskCompletionSource.SetResult(base64string);
            }
            else
            {
                _taskCompletionSource.SetResult(null);
            }

            imagePicker.DismissViewController(true, null);
        }

        private void OnImagePickerCancelled(object sender, EventArgs e)
        {
            _taskCompletionSource.SetResult(null);
            imagePicker.DismissViewController(true, null);
        }

        public UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new SizeF((float)width, (float)height));
            sourceImage.Draw(new RectangleF(0, 0, (float)width, (float)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public UIImage ResizeImage(UIImage sourceImage, float width, float height)
        {
            UIGraphics.BeginImageContext(new SizeF(width, height));
            sourceImage.Draw(new RectangleF(0, 0, width, height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return resultImage;
        }

        public Task<string> GetImageFilePath()
        {
            imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                imagePicker.NavigationBar.PrefersLargeTitles = true;

            imagePicker.NavigationBar.TintColor = Colors.White;

            imagePicker.FinishedPickingMedia -= GetImageUrl;
            imagePicker.FinishedPickingMedia += GetImageUrl;

            imagePicker.Canceled -= OnImagePickerCancelled;
            imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentViewController(imagePicker, true, null);

            _taskCompletionSource = new TaskCompletionSource<string>();
            return _taskCompletionSource.Task;
        }
        
        private void GetImageUrl(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;

            if (image != null)
            {
                image = MaxResizeImage(image, 720, 576);

                NSData data = image.AsJPEG(0.4f);
                string fileName = $"{DateTime.Now.Ticks}.jpeg";
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string pathImage = Path.Combine(path, fileName);

                if (data.Save(pathImage, NSDataWritingOptions.FileProtectionNone, out NSError error))
                {
                    if (error != null)
                        _taskCompletionSource.TrySetResult(null);

                    _taskCompletionSource.TrySetResult(pathImage);
                }
            }
            else
            {
                _taskCompletionSource.SetResult(null);
            }

            imagePicker.DismissViewController(true, null);
        }

        public Task<bool> SaveImageInFiles(string imageUrl)
        {
            var tcs = new TaskCompletionSource<bool>();

            UIImage image;

            using (var url = new NSUrl(imageUrl))
            using (var data = NSData.FromUrl(url))
                image = UIImage.LoadFromData(data);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            File.WriteAllBytes(path, image.AsJPEG().ToArray());

            return tcs.Task;
        }
    }
}
