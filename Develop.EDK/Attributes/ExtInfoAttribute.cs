using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Develop.EDK.Attributes
{
    /// <summary>
    /// An attribute used to say that a field/property is an extension info
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ExtInfoAttribute : Attribute
    {
        public ExtInfoType Type { get; private set; }

        public ExtInfoAttribute(ExtInfoType type) => Type = type;
    }

    /// <summary>
    /// The type of the info
    /// </summary>
    public enum ExtInfoType
    {
        /// <summary>
        /// The extension name (type: <see cref="System.String"/>)
        /// </summary>
        Name,
        /// <summary>
        /// The extension author (type: <see cref="System.String"/>)
        /// </summary>
        Author,
        /// <summary>
        /// The extension version (type: <see cref="System.Double"/>)
        /// </summary>
        Version
    }
}
