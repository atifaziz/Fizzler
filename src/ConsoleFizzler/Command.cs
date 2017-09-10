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
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Mannex.Collections.Generic;

    internal abstract class Command : ICommand
    {
        readonly IConfigurationRoot _configuration;

        protected Command(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public int Run(string[] args)
        {
            LoadConfiguration();
            var tail = CommandLine.ParseTo(args, this);
            return OnRun(tail);
        }

        protected void LoadConfiguration()
        {
            LoadConfiguration(GetType().Name);
        }

        /// <summary>
        /// Maps application settings into command line arguments.
        /// </summary>
        /// <param name="prefix"></param>
        protected virtual void LoadConfiguration(string prefix)
        {
            if (prefix == null) throw new ArgumentNullException("prefix");

            var config =
                from e in _configuration.GetSection(prefix).AsEnumerable()
                where e.Value != null
                select e.Key.Substring(prefix.Length + 1).AsKeyTo(e.Value);

            var args = CommandLine.ToArgs(config);
            CommandLine.ParseTo(args, this);
        }
     
        protected abstract int OnRun(string[] args);
    }
}