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

namespace ConsoleFizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    #endregion

    static class NameValueCollectionExtensions
    {
        public static IEnumerable<string> KeysByPrefix(this NameValueCollection collection, string prefix)
        {
            return KeysByPrefix(collection, prefix, StringComparison.InvariantCultureIgnoreCase);
        }

        public static IEnumerable<string> KeysByPrefix(this NameValueCollection collection, string prefix, StringComparison comparison)
        {
            if(collection == null) throw new ArgumentNullException("collection");

            return string.IsNullOrEmpty(prefix) 
                 ? collection.Keys.Cast<string>() 
                 : KeysByPrefixImpl(collection, prefix, comparison);
        }

        private static IEnumerable<string> KeysByPrefixImpl(NameValueCollection collection, string prefix, StringComparison comparison)
        {
            return from string key in collection.AllKeys
                   where key != null 
                      && key.Length > prefix.Length 
                      && key.StartsWith(prefix, comparison)
                   select key;
        }

        public static IEnumerable<KeyValuePair<string, string>> Pairs(this NameValueCollection collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            return collection.Keys.Cast<string>()
                             .Select(key => new KeyValuePair<string, string>(key, collection[key]));
        }

        public static NameValueCollection Narrow(this NameValueCollection collection, string prefix)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            if (string.IsNullOrEmpty(prefix))
                return collection;

            var result = new NameValueCollection();
            var entries = from key in collection.KeysByPrefix(prefix)
                          let values = collection.GetValues(key)
                          where values != null
                          select new { Key = key.Substring(prefix.Length), Values = values };

            foreach (var entry in entries)
            {
                foreach(var value in entry.Values)
                    result.Add(entry.Key, value);
            }

            return result;
        }
    }
}
