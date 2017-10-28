using dnlib.DotNet;
using NoisetteCore.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoisetteCore.Protection.Renaming
{
    public class RenamingProtection
    {
        public ModuleDefMD _module;

        public List<string> UsedNames;

		public Random Rand;

        public RenamingProtection(ModuleDefMD module)
        {
            UsedNames = new List<string>();
            _module = module;
			Rand = new Random();
        }

        public void RenameModule()
        {
            Obfuscation.RenameAnalyzer RA = new Obfuscation.RenameAnalyzer(_module);
            RA.PerformAnalyze();
        }

        public string GenerateNewName(RenamingProtection RP)
        {
			asdf:
			string str = "";
			int num = Rand.Next(50, 200);

			for (int i = 0; i < num; i++)
				str += (Rand.Next(0, 2) == 1 ? "Fuck" : "You");

			if (!RP.UsedNames.Contains(str))
			{
				RP.UsedNames.Add(str);
				return str;
			}
			else
				goto asdf;
        }
	}
}