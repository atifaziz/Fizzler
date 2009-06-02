using System;

namespace Fizzler
{
    /// <summary>
    /// An <see cref="ISelectorGenerator"/> implementation that delegates
    /// to two other <see cref="ISelectorGenerator"/> objects, which
    /// can be useful for doing work in a single pass.
    /// </summary>
    public sealed class SelectorGeneratorTee : ISelectorGenerator
    {
        /// <summary>
        /// Gets the first generator used to initialize this generator.
        /// </summary>
        public ISelectorGenerator Primary { get; private set; }

        /// <summary>
        /// Gets the second generator used to initialize this generator.
        /// </summary>
        public ISelectorGenerator Secondary { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SelectorGeneratorTee"/>
        /// with the two other <see cref="ISelectorGenerator"/> objects
        /// it delegates to.
        /// </summary>
        public SelectorGeneratorTee(ISelectorGenerator primary, ISelectorGenerator secondary)
        {
            if (primary == null) throw new ArgumentNullException("primary");
            if (secondary == null) throw new ArgumentNullException("secondary");

            Primary = primary;
            Secondary = secondary;
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnInit()
        {
            Primary.OnInit();
            Secondary.OnInit();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnClose()
        {
            Primary.OnClose();
            Secondary.OnClose();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnSelector()
        {
            Primary.OnSelector();
            Secondary.OnSelector();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Type(QName type)
        {
            Primary.Type(type);
            Secondary.Type(type);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Universal()
        {
            Primary.Universal();
            Secondary.Universal();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Id(string id)
        {
            Primary.Id(id);
            Secondary.Id(id);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Class(string clazz)
        {
            Primary.Class(clazz);
            Secondary.Class(clazz);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeExists(QName name)
        {
            Primary.AttributeExists(name);
            Secondary.AttributeExists(name);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeExact(QName name, string value)
        {
            Primary.AttributeExact(name, value);
            Secondary.AttributeExact(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeIncludes(QName name, string value)
        {
            Primary.AttributeIncludes(name, value);
            Secondary.AttributeIncludes(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeDashMatch(QName name, string value)
        {
            Primary.AttributeDashMatch(name, value);
            Secondary.AttributeDashMatch(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributePrefixMatch(QName name, string value)
        {
            Primary.AttributePrefixMatch(name, value);
            Secondary.AttributePrefixMatch(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeSuffixMatch(QName name, string value)
        {
            Primary.AttributeSuffixMatch(name, value);
            Secondary.AttributeSuffixMatch(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void AttributeSubstring(QName name, string value)
        {
            Primary.AttributeSubstring(name, value);
            Secondary.AttributeSubstring(name, value);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void FirstChild()
        {
            Primary.FirstChild();
            Secondary.FirstChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void LastChild()
        {
            Primary.LastChild();
            Secondary.LastChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void NthChild(int a, int b)
        {
            Primary.NthChild(a, b);
            Secondary.NthChild(a, b);
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void OnlyChild()
        {
            Primary.OnlyChild();
            Secondary.OnlyChild();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Empty()
        {
            Primary.Empty();
            Secondary.Empty();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Child()
        {
            Primary.Child();
            Secondary.Child();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Descendant()
        {
            Primary.Descendant();
            Secondary.Descendant();
        }

        /// <summary>
        /// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
        /// </summary>
        public void Adjacent()
        {
            Primary.Adjacent();
            Secondary.Adjacent();
        }

		/// <summary>
		/// Delegates to <see cref="Primary"/> then <see cref="Secondary"/> generator.
		/// </summary>
		public void GeneralSibling()
		{
			Primary.GeneralSibling();
			Secondary.GeneralSibling();
		}
    }
}
