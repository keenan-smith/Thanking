using dnlib.DotNet;
using SymbolRenamer.SymbolRenaming;
using ThankingObfuscator.Protection.AntiTamper;

namespace ThankingObfuscator
{
    class Program
    {
        static void Main(string[] args)
        {
            ModuleDefMD module = ModuleDefMD.Load("Thanking.dll");
            Symbols.Run(module);
			EOFHelper.Run("Thanking_Obfuscated.dll", module);
        }
    }
}
