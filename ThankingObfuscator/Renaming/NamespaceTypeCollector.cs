using System.Collections.Generic;
using dnlib.DotNet;

namespace ThankingObfuscator.Renaming
{
    class NamespaceTypeCollector
    {


        public string NamespaceName { get; set; }
        public List<TypeDef> Types = new List<TypeDef>();

    }
}
