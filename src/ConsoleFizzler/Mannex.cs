#region License, Terms and Author(s)
//
// Mannex - Extension methods for .NET
// Copyright (c) 2009 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Mannex
{
    #region Imports

    using System;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>

    static partial class StringExtensions
    {
        /// <summary>
        /// Splits a string into a pair using a specified character to 
        /// separate the two.
        /// </summary>
        /// <remarks>
        /// Neither half in the resulting pair is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, char separator, Func<string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return SplitRemoving(str, str.IndexOf(separator), 1, resultFunc);
        }

        /// <summary>
        /// Splits a string into three parts using any of a specified set of 
        /// characters to separate the three.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>
        
        public static T Split<T>(this string str, char separator, Func<string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separator, (a, rest) => rest.Split(separator, (b, c) => resultFunc(a, b, c)));
        }

        /// <summary>
        /// Splits a string into four parts using any of a specified set of 
        /// characters to separate the four.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, char separator, Func<string, string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separator, (a, b, rest) => rest.Split(separator, (c, d) => resultFunc(a, b, c, d)));
        }

        /// <summary>
        /// Splits a string into a pair using any of a specified set of 
        /// characters to separate the two.
        /// </summary>
        /// <remarks>
        /// Neither half in the resulting pair is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, char[] separators, Func<string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");

            return separators == null || separators.Length == 0
                 ? resultFunc(str, string.Empty)
                 : SplitRemoving(str, str.IndexOfAny(separators), 1, resultFunc);
        }

        /// <summary>
        /// Splits a string into three parts using any of a specified set of 
        /// characters to separate the three.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, char[] separators, Func<string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separators, (a, rest) => rest.Split(separators, (b, c) => resultFunc(a, b, c)));
        }

        /// <summary>
        /// Splits a string into four parts using any of a specified set of 
        /// characters to separate the four.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, char[] separators, Func<string, string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separators, (a, b, rest) => rest.Split(separators, (c, d) => resultFunc(a, b, c, d)));
        }

        /// <summary>
        /// Splits a string into a pair using a specified string to 
        /// separate the two. An aditional parameter specifies comparison 
        /// rules used to find the separator string.
        /// </summary>
        /// <remarks>
        /// Neither half in the resulting pair is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, string separator, StringComparison comparison, Func<string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return SplitRemoving(str, str.IndexOf(separator, comparison), separator.Length, resultFunc);
        }

        /// <summary>
        /// Splits a string into three parts using a specified string to 
        /// separate the three.  An aditional parameter specifies comparison 
        /// rules used to find the separator string.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>
        
        public static T Split<T>(this string str, string separator, StringComparison comparison, Func<string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separator, comparison, (a, rest) => rest.Split(separator, comparison, (b, c) => resultFunc(a, b, c)));
        }

        /// <summary>
        /// Splits a string into four parts using a specified string to 
        /// separate the four. An aditional parameter specifies comparison 
        /// rules used to find the separator string.
        /// </summary>
        /// <remarks>
        /// None of the resulting parts is ever <c>null</c>.
        /// </remarks>

        public static T Split<T>(this string str, string separator, StringComparison comparison, Func<string, string, string, string, T> resultFunc)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (resultFunc == null) throw new ArgumentNullException("resultFunc");
            return str.Split(separator, comparison, (a, b, rest) => rest.Split(separator, comparison, (c, d) => resultFunc(a, b, c, d)));
        }

        /// <summary>
        /// Splits a string into a pair by removing a portion of the string.
        /// </summary>
        /// <remarks>
        /// Neither half in the resulting pair is ever <c>null</c>.
        /// </remarks>
        
        static T SplitRemoving<T>(string str, int index, int count, Func<string, string, T> resultFunc)
        {
            Debug.Assert(str != null);
            Debug.Assert(count > 0);
            Debug.Assert(resultFunc != null);

            var a = index < 0 
                  ? str 
                  : str.Substring(0, index);
            
            var b = index < 0 || index + 1 >= str.Length 
                  ? string.Empty 
                  : str.Substring(index + count);
            
            return resultFunc(a, b);
        }
    }
}

#region License, Terms and Author(s)
//
// Mannex - Extension methods for .NET
// Copyright (c) 2009 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Mannex.Collections.Generic
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for pairing keys and values as 
    /// <see cref="KeyValuePair{TKey,TValue}"/>.
    /// </summary>

    static partial class PairingExtensions
    {
        /// <summary>
        /// Pairs a value with a key.
        /// </summary>

        public static KeyValuePair<TKey, TValue> AsKeyTo<TKey, TValue>(this TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}

#region License, Terms and Author(s)
//
// Mannex - Extension methods for .NET
// Copyright (c) 2009 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Mannex.Collections.Generic
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// Extension methods for <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>

    static partial class DictionaryExtensions
    {
        /// <summary>
        /// Finds the value for a key, returning the default value for
        /// <typeparamref name="TValue"/> if the key is not present.
        /// </summary>

        [DebuggerStepThrough]
        public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return Find(dict, key, default(TValue));
        }

        /// <summary>
        /// Finds the value for a key, returning a given default value if the
        /// key is not present.
        /// </summary>

        [DebuggerStepThrough]
        public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default)
        {
            if (dict == null) throw new ArgumentNullException("dict");
            TValue value;
            return dict.TryGetValue(key, out value) ? value : @default;
        }
    }
}