using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LetterApp.iOS.Helpers
{
    public static class ImageHelper
    {
        public static Task<Stream> GetStreamFromImageByte(CancellationToken ct, string image)
        {
            byte[] imageInBytes = Convert.FromBase64String(image);
            TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>();
            tcs.TrySetResult(new MemoryStream(imageInBytes));
            return tcs.Task;
        }
    }
}
