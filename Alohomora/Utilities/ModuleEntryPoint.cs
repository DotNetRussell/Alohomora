using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alohomora
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class ModuleEntryPoint : System.Attribute
    {
        public string ModuleIconPath { get; set; }
    }
}
