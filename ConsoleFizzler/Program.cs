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
    using System.Diagnostics;
    using System.Linq;
    using Mannex.Collections.Generic;
    using CommandNames = System.Collections.Generic.KeyValuePair<
            /* key   */ System.Func<ICommand>,
            /* value */ System.Collections.Generic.IEnumerable<string>>;

    #endregion

    internal static class Program
    {
        internal static int Main(string[] args)
        {
            try
            {
                return Run(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Trace.TraceError(e.ToString());
                return 100;
            }
        }

        static int Run(string[] args)
        {
            if (args.Length == 0)
                throw new ApplicationException("Missing command.");

            var commands = new[] 
            {
                new CommandNames(() => new SelectCommand(), Aliases("select", "sel")),
                new CommandNames(() => new ExplainCommand(), Aliases("explain", "describe", "desc")),
            }
            .SelectMany(e => e.Value.Select(v => new KeyValuePair<string, Func<ICommand>>(v, e.Key)))
            .ToDictionary(e => e.Key, e => e.Value);

            var name = args[0];
            
            var command = commands.Find(name);
            if (command == null)
                throw new ApplicationException("Invalid command.");

            return command().Run(args.Skip(1).ToArray());
           
        }

        static IEnumerable<string> Aliases(params string[] values)
        {
            return values;
        }
    }
}
