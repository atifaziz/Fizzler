#region Copyright and License
// 
// Fizzler - CSS Selector Engine for Microsoft .NET Framework
// Copyright (c) 2009 Atif Aziz, Colin Ramsay. All rights reserved.
// 
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 3 of the License, or (at your option) 
// any later version.
// 
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
// 
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
// 
#endregion

namespace VisualFizzler
{
    using System;
    using System.Net.Mime;

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