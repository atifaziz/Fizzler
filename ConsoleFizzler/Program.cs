namespace ConsoleFizzler
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

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
            
            Func<ICommand> command;
            if (!commands.TryGetValue(name, out command))
                throw new ApplicationException("Invalid command.");

            return command().Run(args.Skip(1).ToArray());
           
        }

        static IEnumerable<string> Aliases(params string[] values)
        {
            return values;
        }
    }
}
