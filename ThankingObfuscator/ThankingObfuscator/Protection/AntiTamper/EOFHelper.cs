using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ThankingObfuscator.Utilities;

namespace ThankingObfuscator.Protection.AntiTamper
{
	public static class EOFHelper
	{
		public static void Run(string filename, ModuleDefMD mod)
		{
			AddCall(mod);
			var opts = new ModuleWriterOptions(mod);
			opts.Logger = DummyLogger.NoThrowInstance;
			mod.Write(filename, opts);
			SHA256(filename);
		}
		
		public static void SHA256(string filePath)
		{
			//We get the md5 as byte, of the target
			byte[] md5bytes = System.Security.Cryptography.SHA256.Create().ComputeHash(System.IO.File.ReadAllBytes(filePath));
			//Let's use FileStream to edit the file's byte
			using (var stream = new FileStream(filePath, FileMode.Append))
				stream.Write(md5bytes, 0, md5bytes.Length);
		}


		public static void AddCall(ModuleDef module)
		{
			//We declare our Module, here we want to load the EOFAntitamp class, from AntiTamperEOF.exe
			ModuleDefMD typeModule = ModuleDefMD.Load(typeof(EOFAntiTamp).Module);
			//We find or create the .cctor method in <Module>, aka GlobalType, if it doesn't exist yet
			MethodDef cctor = module.GlobalType.FindOrCreateStaticConstructor();
			//We declare EOFAntitamp as a TypeDef using it's Metadata token (needed)
			TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(EOFAntiTamp).MetadataToken));
			//We use confuserEX InjectHelper class to inject EOFAntitamp class into our target, under <Module>
			IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, module.GlobalType, module);

			//We find the Initialize() Method in EOFAntitamp we just injected
			var init = (MethodDef)members.Single(method => method.Name == "Initialize");
			//We call this method using the Call Opcode
			cctor.Body.Instructions.Insert(0, Instruction.Create(dnlib.DotNet.Emit.OpCodes.Call, init));


			//We just have to remove .ctor method because otherwise it will
			//lead to Global constructor error (e.g [MD]: Error: Global item (field,method) must be Static. [token:0x06000002] / [MD]: Error: Global constructor. [token:0x06000002] )
			foreach (MethodDef md in module.GlobalType.Methods)
			{
				if (md.Name == ".ctor")
				{
					module.GlobalType.Remove(md);
					//Now we go out of this mess
					break;
				}
			}
		}
	}
}
