using System;

namespace Fizzler
{
	/// <summary>
    /// An <see cref="ISelectorGenerator"/> implementation that generates
    /// human-readable description of the selector.
	/// </summary>
	public class HumanReadableSelectorGenerator : ISelectorGenerator
	{
		private int _chainCount;
        private string _text;

		/// <summary>
		/// Delimits the initialization of a generation.
		/// </summary>
		public virtual void OnInit()
		{
			Text = null;
		}

		/// <summary>
		/// Gets the selector implementation.
		/// </summary>
		/// <remarks>
		/// If the generation is not complete, this property returns the 
		/// last generated selector.
		/// </remarks>
		public string Text
		{
			get { return _text; }
			private set { _text = value; }
		}

		/// <summary>
		/// Delimits a selector generation in a group of selectors.
		/// </summary>
		public virtual void OnSelector()
		{
			if (string.IsNullOrEmpty(Text))
				Text = "Select";
			else
				Text += ". Combined with previous, select";
		}

		/// <summary>
		/// Delimits the closing/conclusion of a generation.
		/// </summary>
		public virtual void OnClose()
		{
			Text = Text.Trim();
			Text += ".";
		}

		/// <summary>
		/// Adds a generated selector.
		/// </summary>
		protected void Add(string selector)
		{
			if (selector == null) throw new ArgumentNullException("selector");
			Text += selector;
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#type-selectors">type selector</a>,
		/// which represents an instance of the element type in the document tree. 
		/// </summary>
		public void Type(string type)
		{
			Add(string.Format(" <{0}>", type));
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#universal-selector">universal selector</a>,
		/// any single element in the document tree in any namespace 
		/// (including those without a namespace) if no default namespace 
		/// has been specified for selectors. 
		/// </summary>
		public void Universal()
		{
			Add(" any element");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#Id-selectors">ID selector</a>,
		/// which represents an element instance that has an identifier that 
		/// matches the identifier in the ID selector.
		/// </summary>
		public void Id(string id)
		{
			Add(string.Format(" with an ID of '{0}'", id));
		}

		void ISelectorGenerator.Class(string clazz)
		{
			Add(string.Format(" with class '{0}'", clazz));
		}

		/// <summary>
		/// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
		/// that represents an element with the given attribute <paramref name="name"/>
		/// whatever the values of the attribute.
		/// </summary>
		public void AttributeExists(string name)
		{
            Add(string.Format(" which has attribute {0} defined", name));
		}

		/// <summary>
		/// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
		/// that represents an element with the given attribute <paramref name="name"/>
		/// and whose value is exactly <paramref name="value"/>.
		/// </summary>
		public void AttributeExact(string name, string value)
		{
            Add(string.Format(" which has attribute {0} with a value of '{1}'", name, value));
		}

		/// <summary>
		/// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
		/// that represents an element with the given attribute <paramref name="name"/>
		/// and whose value is a whitespace-separated list of words, one of 
		/// which is exactly <paramref name="value"/>.
		/// </summary>
		public void AttributeIncludes(string name, string value)
		{
            Add(string.Format(" which has a attribute {0} that includes the word '{1}'", name, value));
		}

		/// <summary>
		/// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
		/// that represents an element with the given attribute <paramref name="name"/>,
		/// its value either being exactly <paramref name="value"/> or beginning 
		/// with <paramref name="value"/> immediately followed by "-" (U+002D).
		/// </summary>
		public void AttributeDashMatch(string name, string value)
		{
			Add(" which has attribute {0} with a hyphen separated value matching '{1}'");
		}

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value begins with the prefix <paramref name="value"/>.
        /// </summary>
        public void AttributePrefixMatch(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value begins with '{1}'", name, value));
        }

	    /// <summary>
	    /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
	    /// that represents an element with the attribute <paramref name="name"/> 
	    /// whose value ends with the prefix <paramref name="value"/>.
	    /// </summary>
	    public void AttributeSuffixMatch(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value ends with '{1}'", name, value));
        }

	    /// <summary>
	    /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
	    /// that represents an element with the attribute <paramref name="name"/> 
	    /// whose value contains at least one instance of the substring <paramref name="value"/>.
	    /// </summary>
	    public void AttributeSubstring(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value contains '{1}'", name, value));
        }

	    /// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that is the first child of some other element.
		/// </summary>
		public void FirstChild()
		{
			Add(" where the element is the first child");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that is the last child of some other element.
		/// </summary>
		public void LastChild()
		{
			Add(" where the element is the last child");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that is the N-th child of some other element.
		/// </summary>
		public void NthChild(int a, int b)
		{
			Add(string.Format(" where the element has {0}n+{1}-1 sibling before it", a, b));
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that has a parent element and whose parent 
		/// element has no other element children.
		/// </summary>
		public void OnlyChild()
		{
			Add(" where the element is the only child");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
		/// which represents an element that has no children at all.
		/// </summary>
		public void Empty()
		{
			Add(" where the element is empty");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents a childhood relationship between two elements.
		/// </summary>
		public void Child()
		{
			Add(" whose child element is");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents a relationship between two elements where one element is an 
		/// arbitrary descendant of some ancestor element.
		/// </summary>
		public void Descendant()
		{
			if (_chainCount > 0)
			{
				Add(", that in turn has a descendant that is");
			}
			else
			{
				Add(" whose descendant is");
				_chainCount++;
			}
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which represents elements that share the same parent in the document tree and 
		/// where the first element immediately precedes the second element.
		/// </summary>
		public void Adjacent()
		{
            Add(" whose immediately preceding sibling is");
		}
	}
}