using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymbolRenamer.SymbolRenaming
{
    class NamespaceTypeCollector
    {


        public string NamespaceName { get; set; }
        public List<TypeDef> Types = new List<TypeDef>();

    }
}
