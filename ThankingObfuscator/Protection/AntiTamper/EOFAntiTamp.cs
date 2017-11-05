using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThankingObfuscator.Protection.AntiTamper
{
	class EOFAntiTamp
	{
		static void Initialize()
		{
			string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;

			Stream stream = new StreamReader(assemblyLocation).BaseStream;
			BinaryReader reader = new BinaryReader(stream);
			string realMd5 = null, newMd5 = null;
			newMd5 = BitConverter.ToString(System.Security.Cryptography.SHA256.Create().ComputeHash(reader.ReadBytes(System.IO.File.ReadAllBytes(assemblyLocation).Length - 32)));
			stream.Seek(-32, SeekOrigin.End);
			realMd5 = BitConverter.ToString(reader.ReadBytes(32));
			if (newMd5 != realMd5)
				throw new BadImageFormatException();
		}
	}
}
