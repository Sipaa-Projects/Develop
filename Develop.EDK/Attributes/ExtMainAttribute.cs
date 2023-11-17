using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Develop.EDK.Attributes
{
    /// <summary>
    /// An attribute used by Develop to get the entry point of an extension
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExtMainAttribute : Attribute
    {
        public ExtMainAttribute() { }
    }
}
