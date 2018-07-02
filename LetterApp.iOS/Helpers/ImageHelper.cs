using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LetterApp.iOS.Helpers
{
    public static class ImageHelper
    {
        public static string Image;

        public static Task<Stream> GetStreamFromImageByte(CancellationToken ct)
        {
            //Here you set your bytes[] (image)
            byte[] imageInBytes = Convert.FromBase64String(Image);

            //Since we need to return a Task<Stream> we will use a TaskCompletionSource>
            TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>();

            tcs.TrySetResult(new MemoryStream(imageInBytes));

            return tcs.Task;
        }
    }
}
