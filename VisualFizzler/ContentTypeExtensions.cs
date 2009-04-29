using System;
using System.Net.Mime;

namespace VisualFizzler
{
    internal static class ContentTypeExtensions
    {
        public static bool IsPlainText(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            return EqualsOrdinalIgnoreCase(MediaTypeNames.Text.Plain, contentType.MediaType);
        }

        public static bool IsText(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            return EqualsOrdinalIgnoreCase("text", GetMediaBaseType(contentType));
        }

        public static bool IsHtml(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            return EqualsOrdinalIgnoreCase(MediaTypeNames.Text.Html, contentType.MediaType);
        }

        public static bool IsImage(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            return EqualsOrdinalIgnoreCase("image", GetMediaBaseType(contentType));
        }

        public static string GetMediaBaseType(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            var mediaType = contentType.MediaType;
            return mediaType.Substring(0, mediaType.IndexOf('/'));
        }

        public static string GetMediaSubType(this ContentType contentType)
        {
            if (contentType == null) throw new ArgumentNullException("contentType");
            var mediaType = contentType.MediaType;
            return mediaType.Substring(mediaType.IndexOf('/') + 1);
        }

        private static bool EqualsOrdinalIgnoreCase(string left, string right)
        {
            return left.Equals(right, StringComparison.OrdinalIgnoreCase);
        }
    }
}