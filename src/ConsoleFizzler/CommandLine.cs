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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Mannex;


    #endregion

    static class CommandLine
    {
        private static readonly char[] _argSeparators = new[] { ':', '=' };

        /// <summary>
        /// Updates object from command-line arguments in a response file.
        /// </summary>
        /// <remarks>
        /// Line in the response file that begin with a hash (#) are
        /// considered as comments.
        /// </remarks>

        public static string[] ParseFileTo(string path, object target)
        {
            return ParseFileTo(path, target, null);
        }

        /// <summary>
        /// Updates object from command-line arguments in a response file
        /// <paramref name="properties" /> describes the target object.
        /// </summary>
        /// <remarks>
        /// Line in the response file that begin with a hash (#) are
        /// considered as comments.
        /// </remarks>

        public static string[] ParseFileTo(string path, object target, PropertyDescriptorCollection properties)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (target == null) throw new ArgumentNullException("target");

            var lines = File.ReadAllLines(path)
                            .Where(line => line.Length > 0 && line[0] != '#'); // except comments
            
            var args = CommandLineToArgs(string.Join(" ", lines.ToArray()));
            
            return ParseTo(args, target, null);
        }

        /// <summary>
        /// Updates object from command-line arguments.
        /// </summary>
        
        public static string[] ParseTo(IEnumerable<string> args, object target)
        {
            return ParseTo(args, target, null);
        }

        /// <summary>
        /// Updates object from command-line arguments where
        /// <paramref name="properties" /> describes the target object.
        /// </summary>

        public static string[] ParseTo(IEnumerable<string> args, object target, PropertyDescriptorCollection properties)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (args == null) throw new ArgumentNullException("args");

            var tails = new List<string>();

            var e = args.GetEnumerator();
            while (e.MoveNext())
            {
                var arg = e.Current;

                if (arg.Length == 0) // empty arg?
                {
                    tails.Add(arg);
                    continue;
                }

                // Get argument name, Unix or DOS style.

                string name;

                if (arg[0] == '/') // DOS style
                {
                    name = arg.Substring(1);
                }
                else if (arg.Length > 1 && arg[0] == '-' && arg[1] == '-') // Unix style
                {
                    if (arg.Length == 2) // comment
                        break;
                    name = arg.Substring(2);
                }
                else
                {
                    tails.Add(arg); // anonymous argument
                    continue;
                }

                // Is the argument name and value paired in one?
                // Allows `arg=value` or `arg:value` style.

                var paired = name.IndexOfAny(_argSeparators) > 0;
                var pair = name.Split(_argSeparators, (n, v) => new { Name = n, Value = v });

                if (paired)
                    name = pair.Name; // Break the name out of the pair

                // Get setting property from name.

                var propertyName = name.Replace("-", " ")
                        .ToTitleCaseInvariant()
                        .Replace(" ", string.Empty);

                if (properties == null)
                    properties = TypeDescriptor.GetProperties(target);

                var property = properties.Find(propertyName, true);

                if (property == null)
                    throw new FormatException(string.Format("Unknown command-line argument: " + name));

                // Process argument based on property type.

                var type = property.PropertyType;
                object value;

                if (type == typeof(bool)) // Boolean?
                {
                    value = true; // flag-style
                }
                else
                {
                    // If value was paired with name then break out the value
                    // from the pair other read it from the next argument.

                    if (paired)
                    {
                        value = pair.Value;
                    }
                    else
                    {
                        if (!e.MoveNext())
                            throw new FormatException("Missing value for command-line argument: " + name);

                        value = e.Current;
                    }

                    // If property is of another type than string and it
                    // support conversion then do that now.

                    if (type != typeof(string))
                    {
                        var converter = property.Converter ?? TypeDescriptor.GetConverter(type);

                        if (!converter.CanConvertFrom(typeof(string)))
                            throw new FormatException("Unsupported command-line argument:" + name);

                        value = converter.ConvertFromString(value.ToString());
                    }
                }

                property.SetValue(target, value);
            }

            return tails.ToArray();
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        private static string[] CommandLineToArgs(string commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);
            if (argv == IntPtr.Zero)
                throw new Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }

        /// <summary>
        /// Returns a sequence of args from a sequence of pair where each
        /// key is prefixed with a switch token classic for the platform.
        /// </summary>
        public static IEnumerable<string> ToArgs(IEnumerable<KeyValuePair<string, string>> args)
        {
            return ToArgs(args, null);
        }

        /// <summary>
        /// Returns a sequence of args from a sequence of pair where each
        /// key is prefixed with <paramref name="switchToken"/>.
        /// </summary>
        public static IEnumerable<string> ToArgs(IEnumerable<KeyValuePair<string, string>> args, string switchToken)
        {
            if (args == null) throw new ArgumentNullException("args");

            if (string.IsNullOrEmpty(switchToken))
            {
                var platform = Environment.OSVersion.Platform;
                switchToken = platform == PlatformID.Unix ? "--" : "/";
            }

            return ArgsFromImpl(args, switchToken);
        }

        private static IEnumerable<string> ArgsFromImpl(IEnumerable<KeyValuePair<string, string>> args, 
            string switchToken)
        {
            return args.Select(e => new[] { switchToken + e.Key, e.Value })
                       .SelectMany(e => e);
        }
    }
}