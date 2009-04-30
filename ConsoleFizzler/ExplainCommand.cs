using System;
using System.Linq;
using Fizzler;

namespace ConsoleFizzler
{
    internal sealed class ExplainCommand : Command
    {
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