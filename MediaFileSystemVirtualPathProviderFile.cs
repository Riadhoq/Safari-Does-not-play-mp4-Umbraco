using System;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace FEE.Domain.Media
{
    public class MediaFileSystemVirtualPathProviderFile : VirtualFile
    {
        private readonly Func<Stream> _stream;

        public MediaFileSystemVirtualPathProviderFile(string virtualPath, Func<Stream> stream) : base(virtualPath)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            _stream = stream;
        }

        public override bool IsDirectory => false;

        public override Stream Open()
        {
            if (HttpContext.Current != null)
            {
                var path = HttpContext
                    .Current
                    .Request
                    .FilePath;

                if (path.Contains(".mp4"))
                {
                    HttpContext
                        .Current
                        .Response
                        .AppendHeader("Accept-Ranges", "bytes");
                }
            }
            return _stream();
        }
    }
}
