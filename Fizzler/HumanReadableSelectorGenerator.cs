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
        /// Initializes the text.
        /// </summary>
        public virtual void OnInit()
		{
			Text = null;
		}

		/// <summary>
		/// Gets the generated human-readable description text.
		/// </summary>
		public string Text
		{
			get { return _text; }
			private set { _text = value; }
		}

        /// <summary>
        /// Generates human-readable for a selector in a group.
        /// </summary>
        public virtual void OnSelector()
		{
			if (string.IsNullOrEmpty(Text))
				Text = "Select";
			else
				Text += ". Combined with previous, select";
		}

		/// <summary>
		/// Concludes the text.
		/// </summary>
		public virtual void OnClose()
		{
			Text = Text.Trim();
			Text += ".";
		}

		/// <summary>
		/// Adds to the generated human-readable text.
		/// </summary>
		protected void Add(string selector)
		{
			if (selector == null) throw new ArgumentNullException("selector");
			Text += selector;
		}

        /// <summary>
        /// Generates human-readable text of this type selector.
        /// </summary>
        public void Type(string type)
		{
			Add(string.Format(" <{0}>", type));
		}

        /// <summary>
        /// Generates human-readable text of this universal selector.
        /// </summary>
        public void Universal()
		{
			Add(" any element");
		}

        /// <summary>
        /// Generates human-readable text of this ID selector.
        /// </summary>
        public void Id(string id)
		{
			Add(string.Format(" with an ID of '{0}'", id));
		}

        /// <summary>
        /// Generates human-readable text of this class selector.
        /// </summary>
        void ISelectorGenerator.Class(string clazz)
		{
			Add(string.Format(" with class '{0}'", clazz));
		}

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeExists(string name)
		{
            Add(string.Format(" which has attribute {0} defined", name));
		}

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeExact(string name, string value)
		{
            Add(string.Format(" which has attribute {0} with a value of '{1}'", name, value));
		}

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeIncludes(string name, string value)
		{
            Add(string.Format(" which has a attribute {0} that includes the word '{1}'", name, value));
		}

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeDashMatch(string name, string value)
		{
			Add(" which has attribute {0} with a hyphen separated value matching '{1}'");
		}

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributePrefixMatch(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value begins with '{1}'", name, value));
        }

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeSuffixMatch(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value ends with '{1}'", name, value));
        }

        /// <summary>
        /// Generates human-readable text of this attribute selector.
        /// </summary>
        public void AttributeSubstring(string name, string value)
	    {
            Add(string.Format(" which has attribute {0} whose value contains '{1}'", name, value));
        }

        /// <summary>
        /// Generates human-readable text of this pseudo-class selector.
        /// </summary>
        public void FirstChild()
		{
			Add(" where the element is the first child");
		}

        /// <summary>
        /// Generates human-readable text of this pseudo-class selector.
        /// </summary>
        public void LastChild()
		{
			Add(" where the element is the last child");
		}

        /// <summary>
        /// Generates human-readable text of this pseudo-class selector.
        /// </summary>
        public void NthChild(int a, int b)
		{
			Add(string.Format(" where the element has {0}n+{1}-1 sibling before it", a, b));
		}

        /// <summary>
        /// Generates human-readable text of this pseudo-class selector.
        /// </summary>
        public void OnlyChild()
		{
			Add(" where the element is the only child");
		}

        /// <summary>
        /// Generates human-readable text of this pseudo-class selector.
        /// </summary>
        public void Empty()
		{
			Add(" where the element is empty");
		}

        /// <summary>
        /// Generates human-readable text of this combinator.
        /// </summary>
        public void Child()
		{
			Add(" whose child element is");
		}

        /// <summary>
        /// Generates human-readable text of this combinator.
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
        /// Generates human-readable text of this combinator.
        /// </summary>
        public void Adjacent()
		{
            Add(" whose next sibling is");
		}

		/// <summary>
		/// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
		/// which separates two sequences of simple selectors. The elements represented
		/// by the two sequences share the same parent in the document tree and the
		/// element represented by the first sequence precedes (not necessarily
		/// immediately) the element represented by the second one.
		/// </summary>
		public void GeneralSibling()
		{
			Add(" which has siblings that are");
		}
	}
}