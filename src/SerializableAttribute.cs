#if NETSTANDARD1_0

namespace Fizzler
{
    using Attribute = System.Attribute;
    using AttributeUsageAttribute = System.AttributeUsageAttribute;
    using static System.AttributeTargets;

    [AttributeUsage(Class | Struct | Enum | Delegate)]
    sealed class SerializableAttribute : Attribute {}
}

#endif
