using System;
using System.Configuration;

namespace ConsoleFizzler
{
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