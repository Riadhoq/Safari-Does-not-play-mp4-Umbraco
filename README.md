# Safari-Does-not-play-mp4-Umbraco
Safari Does not play mp4 Umbraco

# Courtesy of: https://github.com/mikebull

On one of our sites, we've come across an issue where if we're hosting mp4 files on S3, the file will load fine in Safari with the AWS URL, but not from the VPP we've set up.

After inspecting the response, AWS adds the following line to the header:

Accept-Ranges: bytes

It would seem that Safari has an issue where it looks to load the file in parts, and if it cannot, it won't bother to render the video at all. The video loads fine over the VPP in all other browsers, but Safari on the Mac.

The same issue has been raised for one of the other Umbraco providers that targets Azure, so I've improvised with the following code in FileSystemVirtualPathProviderFile.cs to add the header to the response.

public override Stream Open()
{
    if (HttpContext.Current != null)
    {
        var path = HttpContext
            .Current
            .Request
            .FilePath;

        if (path.Contains("mp4"))
        {
            HttpContext
                .Current
                .Response
                .AppendHeader("Accept-Ranges", "bytes");
        }
    }

    return _stream();
}
With this code in place, we've managed to get mp4's to render. If you're okay with me doing so, I'm happy to add this code in a pull request.
