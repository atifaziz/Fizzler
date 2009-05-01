using System;
using System.Linq;
using System.Windows.Forms;

namespace Fizzler.Systems.WinForms
{
    /// <summary>
    /// An <see cref="INodeOps{TNode}"/> implementation for <see cref="Control"/>
    /// from Windows Forms.
    /// </summary>
    public class ControlOps : INodeOps<Control>
    {
        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#type-selectors">type selector</a>,
        /// which represents an instance of the control type in the control tree. 
        /// </summary>
        /// <remarks>
        /// This selector only checks on the name portion of the control's 
        /// run-time type. If the <paramref name="type"/> contains dashes 
        /// then they are used to make stricter checks across the namespace 
        /// as well. For example, if <paramref name="type"/> is 
        /// <c>button</c> then it will match against all controls whose 
        /// run-time type name is <c>button</c>, regardless of case and 
        /// namespace. Suppose, however, there are two controls of type 
        /// <c>Foo.Bar.Button</c> and <c>Baz.Qux.Button</c>. A type 
        /// selection for <c>button</c> will yield instances of both. 
        /// A type selection for <c>bar-button</c> or <c>foo-bar-button</c>
        /// on the other hand will yield instance of <c>Foo.Bar.Button</c>.
        /// In essence, <paramref name="type"/> represent the <em>tail</em>.
        /// </remarks>
        public virtual Selector<Control> Type(string type)
        {
            return TypeEndsWith(type.Split('-').Reverse().ToArray());
        }

        private static Selector<Control> TypeEndsWith(string[] names)
        {
            return controls => controls.Where(
                c => c.GetType().FullName
                      .Split('.')
                      .Reverse()
                      .Take(names.Length)
                      .SequenceEqual(names, StringComparer.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#universal-selector">universal selector</a>,
        /// any single control in the control tree. 
        /// </summary>
        public virtual Selector<Control> Universal()
        {
            return controls => controls;
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#Id-selectors">ID selector</a>,
        /// which represents a control instance that has an identifier that 
        /// matches the identifier in the ID selector.
        /// </summary>
        public virtual Selector<Control> Id(string id)
        {
            return controls => controls.Where(c => c.Name == id);
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#class-html">class selector</a>,
        /// which is an alternative <see cref="INodeOps{TNode}.AttributeIncludes"/> when 
        /// representing the <c>class</c> attribute. 
        /// </summary>
        public virtual Selector<Control> Class(string clazz)
        {
            return AttributeIncludes("class", clazz);
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents a control with the given property <paramref name="name"/>
        /// whatever the values of the property.
        /// </summary>
        public virtual Selector<Control> AttributeExists(string name)
        {
            return controls => controls.Where(c => c.Properties().Find(name, true) != null);
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents a control with the given property <paramref name="name"/>
        /// and whose value is exactly <paramref name="value"/>.
        /// </summary>
        public virtual Selector<Control> AttributeExact(string name, string value)
        {
            return controls => controls.Where(c => c.FindPropertyValueString(name) == value);
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents a control with the given property <paramref name="name"/>
        /// and whose value is a whitespace-separated list of words, one of 
        /// which is exactly <paramref name="value"/>.
        /// </summary>
        public virtual Selector<Control> AttributeIncludes(string name, string value)
        {
            return controls => controls.Where(c => c.FindPropertyValueString(name).Split().Contains(value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents a control with the given property <paramref name="name"/>,
        /// its value either being exactly <paramref name="value"/> or beginning 
        /// with <paramref name="value"/> immediately followed by "-" (U+002D).
        /// </summary>
        public virtual Selector<Control> AttributeDashMatch(string name, string value)
        {
            return controls => controls.Where(c => c.FindPropertyValueString(name).Split('-').Contains(value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value begins with the prefix <paramref name="value"/>.
        /// </summary>
        public Selector<Control> AttributePrefixMatch(string name, string value)
        {
            return controls => controls.Where(c => c.FindPropertyValueString(name).StartsWith(value));
        }

        /// <summary>
        /// Generates an <a href="http://www.w3.org/TR/css3-selectors/#attribute-selectors">attribute selector</a>
        /// that represents an element with the attribute <paramref name="name"/> 
        /// whose value ends with the suffix <paramref name="value"/>.
        /// </summary>
        public Selector<Control> AttributeSuffixMatch(string name, string value)
        {
            return controls => controls.Where(c => c.FindPropertyValueString(name).EndsWith(value));
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents a control that is the first child of some other control.
        /// </summary>
        public virtual Selector<Control> FirstChild()
        {
            return controls => controls.Where(c => !c.ControlsBeforeSelf().Any());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents a control that is the last child of some other control.
        /// </summary>
        public virtual Selector<Control> LastChild()
        {
            return controls => controls.Where(c => !c.ControlsAfterSelf().Any());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents a control that is the N-th child of some other control.
        /// </summary>
        public virtual Selector<Control> NthChild(int position)
        {
            return controls => from c in controls
                               let parent = c.Parent
                               where parent != null
                               let siblings = parent.Controls
                               where siblings.IndexOf(c) == position - 1
                               select c;
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents a control that has a parent control and whose parent 
        /// control has no other children.
        /// </summary>
        public virtual Selector<Control> OnlyChild()
        {
            return controls => controls.Where(c => !c.ControlsBeforeSelf().Concat(c.ControlsAfterSelf()).Any());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#pseudo-classes">pseudo-class selector</a>,
        /// which represents a control that has no children at all.
        /// </summary>
        public virtual Selector<Control> Empty()
        {
            return controls => controls.Where(c => c.Controls.Count == 0);
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a childhood relationship between two controls.
        /// </summary>
        public virtual Selector<Control> Child()
        {
            return controls => controls.SelectMany(c => c.Controls.Cast<Control>());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents a relationship between two controls where one control is an 
        /// arbitrary descendant of some ancestor control.
        /// </summary>
        public virtual Selector<Control> Descendant()
        {
            return controls => controls.SelectMany(c => c.Descendants());
        }

        /// <summary>
        /// Generates a <a href="http://www.w3.org/TR/css3-selectors/#combinators">combinator</a>,
        /// which represents elements that share the same parent in the document tree and 
        /// where the first control immediately precedes the second control.
        /// </summary>
        public virtual Selector<Control> Adjacent()
        {
            return controls => controls.SelectMany(c => c.ControlsAfterSelf().Take(1));
        }
    }
}
