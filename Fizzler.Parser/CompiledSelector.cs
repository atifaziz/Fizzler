using System;
using System.Collections;

namespace Fizzler.Parser
{
    /// <summary>
    /// Represent a compiled selector.
    /// </summary>
    [Serializable]
    public sealed class CompiledSelector
    {
        private readonly object _selector;
        private readonly ISelectorEngine2 _engine;

        internal CompiledSelector(object selector, ISelectorEngine2 engine)
        {
            _selector = selector;
            _engine = engine;
        }

        /// <summary>
        /// Reutrns a collection of item matching this selector from the
        /// given contxt
        /// </summary>
        public IEnumerable Select(object context)
        {
            return _engine.Select(context, _selector);
        }
    }
}