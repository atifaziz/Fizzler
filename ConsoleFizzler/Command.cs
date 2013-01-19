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
    using System.Configuration;

    internal abstract class Command : ICommand
    {
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

            if (prefix.Length > 0)
                prefix = prefix + ".";

            var settings = ConfigurationManager.AppSettings;
            var args = CommandLine.ToArgs(settings.Narrow(prefix).Pairs());
            CommandLine.ParseTo(args, this);
        }
     
        protected abstract int OnRun(string[] args);
    }
}