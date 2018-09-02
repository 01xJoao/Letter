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
        private TaskCompletionSource<string> taskCompletionSource;
        private UIImagePickerController imagePicker;

        public Task<string> GetImageStreamSync()
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
            imagePicker.FinishedPickingMedia -= OnImagePickerFinishedPickingMedia;
            imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;

            imagePicker.Canceled -= OnImagePickerCancelled;
            imagePicker.Canceled += OnImagePickerCancelled;

            // Present UIImagePickerController;
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentModalViewController(imagePicker, true);

            // Return Task object
            taskCompletionSource = new TaskCompletionSource<string>();
            return taskCompletionSource.Task;
        }

        private void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage image = args.EditedImage ?? args.OriginalImage;
            if (image != null)
            {
                image = MaxResizeImage(image, LocalConstants.Profile_PictureHeight, LocalConstants.Profile_PictureHeight);

                // Convert UIImage to .NET Stream object
                NSData data = image.AsJPEG(0.6f);
                //Stream stream = data.AsStream();
                var base64string = data.GetBase64EncodedString(NSDataBase64EncodingOptions.SixtyFourCharacterLineLength);

                // Set the Stream as the completion of the Task
                taskCompletionSource.SetResult(base64string);
            }
            else
            {
                taskCompletionSource.SetResult(null);
            }

            imagePicker.DismissModalViewController(true);
        }

        private void OnImagePickerCancelled(object sender, EventArgs e)
        {
            taskCompletionSource.SetResult(null);
            imagePicker.DismissModalViewController(true);
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
    }
}
