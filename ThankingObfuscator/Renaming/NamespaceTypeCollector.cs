using System.Collections.Generic;
using dnlib.DotNet;

namespace SymbolRenamer.SymbolRenaming
{
    class NamespaceTypeCollector
    {


        public string NamespaceName { get; set; }
        public List<TypeDef> Types = new List<TypeDef>();

    }
}
