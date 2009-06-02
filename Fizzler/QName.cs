using System;

namespace Fizzler
{
    /// <summary>
    /// Represent a type or attribute name.
    /// </summary>
    [Serializable]
    public struct QName
    {
        /// <summary>
        /// Represent an empty (invalid) instance.
        /// </summary>
        public static readonly QName Empty;

        /// <summary>
        /// Gets the namespace part of this instance.
        /// </summary>
        public string Namespace { get; private set; }
        
        /// <summary>
        /// Gets the local name part of this instance.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes this instance with a name and its namespace.
        /// </summary>
        public QName(string ns, string name) : this()
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("name");

            Namespace = ns;
            Name = name;
        }

        /// <summary>
        /// Creates a <see cref="QName"/> that represents a name
        /// from either the default or any namespace in a target
        /// document, depending on whether a default namespace is
        /// in effect or not.
        /// </summary>
        public static QName Namespaceless(string name)
        {
            return new QName(null, name);
        }

        /// <summary>
        /// Creates a <see cref="QName"/> that represents a name
        /// from any namespace (including one without one)
        /// in a target document.
        /// </summary>
        public static QName AnyNamespace(string name)
        {
            return new QName("*", name);
        }

        /// <summary>
        /// Creates a <see cref="QName"/> that represents a name
        /// without a namespace in a target document.
        /// </summary>
        public static QName EmptyNamespace(string name)
        {
            return new QName(string.Empty, name);
        }

        /// <summary>
        /// Indicates whether this object is <see cref="Empty"/> or not.
        /// </summary>
        public bool IsEmpty
        {
            get { return Name == null; }
        }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// from either the default or any namespace in a target
        /// document, depending on whether a default namespace is
        /// in effect or not.
        /// </summary>
        public bool IsNamespaceless
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException();
                return Namespace == null;
            }
        }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// from any namespace (including one without one)
        /// in a target document.
        /// </summary>
        public bool IsAnyNamespace
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException();
                return !IsNamespaceless 
                    && Namespace.Length == 1 
                    && Namespace[0] == '*';
            }
        }

        /// <summary>
        /// Indicates whether this instance represents a name
        /// without a namespace in a target document.
        /// </summary>
        public bool IsEmptyNamespace
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException();
                return !IsNamespaceless 
                    && Namespace.Length == 0;
            }
        }

        /// <summary>
        /// Indicates whether this instance has a specific namespace
        /// or not.
        /// </summary>
        public bool IsNamespaceSpecified
        {
            get
            {
                if (IsEmpty) throw new InvalidOperationException();
                return !IsNamespaceless && !IsAnyNamespace;
            }
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is QName && Equals((QName) obj);
        }

        /// <summary>
        /// Indicates whether this instance and another are equal.
        /// </summary>
        public bool Equals(QName other)
        {
            return Namespace == other.Namespace && Name == other.Name;
        }
        /*
        /// <summary>
        /// Indicates whether this instance and another are equal.
        /// </summary>
        public bool Match(string ns, string name, string defaultNamespace)
        {
            if (IsEmpty) throw new InvalidOperationException();
            
            var comparer = StringComparer.Ordinal;

            if (IsNamespaceless)
            {
                if (string.IsNullOrEmpty(defaultNamespace))
                    return AnyNamespace(Name).Match(ns, name, defaultNamespace);

                if (!comparer.Equals(ns, defaultNamespace))
                    return false;
            }
            else if (IsEmptyNamespace)
            {
                if (!string.IsNullOrEmpty(ns))
                    return false;
            }
            else if (!IsAnyNamespace)
            {
                if (!comparer.Equals(Namespace, ns))
                    return false;
            }

            return comparer.Equals(Name, name);
        }
        */
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return IsEmpty 
                 ? 0
                 : IsNamespaceless 
                 ? Name.GetHashCode() 
                 : Namespace.GetHashCode() ^ Name.GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return IsEmpty 
                 ? string.Empty 
                 : Namespace + (IsNamespaceless ? null : "|") + Name;
        }
    }
}
