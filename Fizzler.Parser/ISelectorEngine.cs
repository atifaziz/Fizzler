using System.Collections.Generic;
using Fizzler.Parser.Document;

namespace Fizzler.Parser
{
	public interface ISelectorEngine
	{
		/// <summary>
		/// Select from the IDocument which was used to initialise the engine.
		/// </summary>
		/// <remarks>Implementors should ensure that their constructor supplies something to select against.</remarks>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		IList<IDocumentNode> Select(string selectorChain);

		/// <summary>
		/// Select from the passed IDocument.
		/// </summary>
		/// <param name="document"></param>
		/// <param name="selectorChain"></param>
		/// <returns></returns>
		IList<IDocumentNode> Select(IDocument document, string selectorChain);
	}
}