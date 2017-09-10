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
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Fizzler;

    #endregion

    internal sealed class ExplainCommand : Command
    {
        public ExplainCommand(IConfigurationRoot configuration) :
            base(configuration) {}

        protected override int OnRun(string[] args)
        {
            if (!args.Any())
                throw new ApplicationException("Missing CSS selector.");

            var generator = new HumanReadableSelectorGenerator();
            Parser.Parse(args[0], generator);
            Console.WriteLine(generator.Text);
            return 0;
        }
    }
}