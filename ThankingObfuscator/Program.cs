using dnlib.DotNet;
using dnlib.DotNet.Writer;
using ThankingObfuscator.Protection.JunkCode;
using ThankingObfuscator.Renaming;

namespace ThankingObfuscator
{
    class Program
    {
		static void Main(string[] args)
		{
			ModuleDefMD module = ModuleDefMD.Load("Thanking.dll");
			Symbols.Run(module);
			MalformedMethodGenerator.GenMalformedMethods(module);

			ModuleWriterOptions opt = new ModuleWriterOptions()
			{
				Logger = DummyLogger.NoThrowInstance
			};
			module.Write("i.dll", opt);
        }
    }
}
