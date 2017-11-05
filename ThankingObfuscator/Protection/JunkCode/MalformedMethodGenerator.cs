using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThankingObfuscator.Renaming;

namespace ThankingObfuscator.Protection.JunkCode
{
	public static class MalformedMethodGenerator
	{
		public static void GenMalformedMethods(ModuleDefMD Module)
		{
			Random rand = new Random();
			
			foreach (TypeDef type in Module.Types)
			{
				for (int i = 0; i < rand.Next(20, 50); i++)
				{
					MethodDefUser user = new MethodDefUser(Symbols.GenerateName());

					CilBody body = new CilBody();
					user.Body = body;

					for (int j = 0; j < rand.Next(30, 50); j++)
					{
						OpCode code = OpCodes.OneByteOpCodes[rand.Next(0, OpCodes.OneByteOpCodes.Length)];
						if (code != OpCodes.Ret && code.OperandType == OperandType.InlineNone)
							user.Body.Instructions.Add(Instruction.Create(code));
					}

					type.Methods.Insert(rand.Next(0, type.Methods.Count), user);
				}
			}
		}
	}
}
