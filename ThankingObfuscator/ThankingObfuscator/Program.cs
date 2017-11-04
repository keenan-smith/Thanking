using dnlib.DotNet;
using SymbolRenamer.SymbolRenaming;

namespace ThankingObfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            ModuleDefMD module = ModuleDefMD.Load("Thanking.dll");
            Symbols.Run(module);
            module.Write("Thanking_Obfuscated.dll");
        }
    }
}
