using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Fizzler.Systems.WinForms
{
    /// <summary>
    /// Extension methods for <see cref="Control"/>.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Similar to <see cref="QuerySelectorAll" /> except it returns 
        /// only the first control matching the supplied selector strings.
        /// </summary>
        public static Control QuerySelector(this Control control, string selector)
        {
            return control.QuerySelectorAll(selector).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all controls from descendants of the starting control 
        /// that match any selector within the supplied selector strings. 
        /// </summary>
        public static IEnumerable<Control> QuerySelectorAll(this Control control, string selector)
        {
            var generator = new SelectorGenerator<Control>(new ControlOps());
            Parser.Parse(selector, generator);
            return generator.Selector(Enumerable.Repeat(control, 1));
        }

        /// <summary>
        /// Returns a collection of all properties on the control.
        /// </summary>
        public static PropertyDescriptorCollection Properties(this Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            return TypeDescriptor.GetProperties(control);
        }

        /// <summary>
        /// Attemps to locate a property on the object and returns its value.
        /// If the property does not exist then a null reference is returned.
        /// </summary>
        public static object FindPropertyValue(this Control control, string name)
        {
            return FindPropertyValue(control, name, null);
        }

        /// <summary>
        /// Attemps to locate a property on the object and returns its value.
        /// If the property does not exist then the value specified in 
        /// <paramref name="defaultValue"/> is returned instead.
        /// </summary>
        public static object FindPropertyValue(this Control control, string name, object defaultValue)
        {
            if (control == null) throw new ArgumentNullException("control");
            var property = TypeDescriptor.GetProperties(control).Find(name, true);
            return property == null ? defaultValue : property.GetValue(control);
        }

        /// <summary>
        /// Attemps to locate a property on the object and returns a string
        /// representation of its value. If the property does not exist then 
        /// an empty string is returned but never a null reference.
        /// </summary>
        internal static string FindPropertyValueString(this Control control, string name)
        {
            return Convert.ToString(control.FindPropertyValue(name) ?? string.Empty);
        }

        /// <summary>
        /// Returns a collection of all controls that precede this control
        /// in children of its parent.
        /// </summary>
        public static IEnumerable<Control> ControlsAfterSelf(this Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            var parent = control.Parent;
            if (parent != null)
            {
                var controls = parent.Controls;
                var i = controls.IndexOf(control) + 1;
                for (; i < controls.Count; i++)
                    yield return controls[i];
            }
        }

        /// <summary>
        /// Returns a collection of all controls that succeed this control
        /// in children of its parent.
        /// </summary>
        public static IEnumerable<Control> ControlsBeforeSelf(this Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            var parent = control.Parent;
            if (parent != null)
            {
                var controls = parent.Controls;
                var count = controls.IndexOf(control);
                for (var i = 0; i < count; i++)
                    yield return controls[i];
            }
        }

        /// <summary>
        /// Returns a collection of all controls that are the children and 
        /// grandchildren of this control.
        /// </summary>
        public static IEnumerable<Control> Descendants(this Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            return DescendantsImpl(control);
        }

        private static IEnumerable<Control> DescendantsImpl(Control control)
        {
            Debug.Assert(control != null);
            foreach (Control child in control.Controls)
            {
                yield return child;
                foreach (var descendant in child.Descendants())
                    yield return descendant;
            }
        }
    }
}
