using dnlib.DotNet;
using SymbolRenamer.SymbolRenaming;

namespace ThankingObfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            ModuleDefMD module = ModuleDefMD.Load(args[0]);
            Symbols.Run(module);
            module.Write(args[0] + "_Renamed");
        }
    }
}
