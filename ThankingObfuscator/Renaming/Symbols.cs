using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ThankingObfuscator.Renaming
{
    class Symbols
    {

        /// <summary>
        /// What kind of symbols should be used for renaming.
        /// </summary>
        private static SymbolStyle symbolStyle = SymbolStyle.FuckYou;

        /// <summary>
        /// Supported symbol styles.
        /// </summary>
        private enum SymbolStyle
        {
            Unreadable = 0,
            Chinese = 1,
            Emojis = 2,
			FuckYou = 3
        }

        /// <summary>
        /// Random instance to generate random integer values.
        /// </summary>
        private static Random rnd = new Random();

        /// <summary>
        /// List of gnerated names to collect them and prevent duplicates
        /// </summary>
        private static readonly List<string> generatedStrings = new List<string>();

        /// <summary>
        /// The counter for the renamed symbols.
        /// </summary>
        private static int renamedSymbols = 0;

        /// <summary>
        /// Generate a random string from one of the symbol charsets.
        /// </summary>
        /// <returns>Random string.</returns>
        public static string GenerateName()
        {

            // The charset which holds the characters allowed for renaming 
            string[] charMap = null;

            // StringBuilder to generate a string from the charMap
            StringBuilder stringBuilder;

            // Choose the selected renaming style.
            switch (symbolStyle)
            {
                case SymbolStyle.Unreadable:
                        charMap = "̽˱˲`ʭˆ˺˩˨˥˭ʺ˾˫̪ ˽̟ !;˪╒╨ױ̄̚".ToCharArray().Select(a => a.ToString()).ToArray();
                        break;
                case SymbolStyle.Chinese:
                    charMap = "這是驚人的我喜歡看到人們的笑容美國死亡淨混淆他媽的佩爾松低音蟾蜍你是如何科網喜歡這個地方他媽的".ToCharArray().Select(a => a.ToString()).ToArray();
                    break;
                case SymbolStyle.Emojis:
                    charMap = "🖕😍😘".ToCharArray().Select(a => a.ToString()).ToArray(); //🖕 "😀😃😄😁😆😅😂🤣☺️😊😇🙂🙃😉😌😍😘😗😙😚😋😜😝😛🤑🤗🤓😎🤡🤠😏😒😞😔😟😕🙁😣😖😫😩😤😠😡😶😐😑😯😦😧😮😲😵😳😱😨😰😢😥🤤😭😓😪😴🙄🤔🤥😬🤐🤢🤧😷🤒🤕😈👿👹👺💩👻💀☠️👽👾🤖🎃😺😸😹😻😼😽🙀😿😾👐🙌👏🙏🤝👍👎👊✊🤛🤜🤞✌️🤘👌👈👉👆👇☝️✋🤚🖐🖖👋🤙💪🖕💅🖖💄💋👄👅👂👃👣👁👀🗣👤👥👶👦👧👨👩👱‍🌎🌍🌏🌕🌖🌗🌘🌑🌒🌓🌔🌚🌝🤩🤨🤯🤪🤬🤮🤫🤭🧐🧒🧑🧓🧕".ToCharArray();
                    break;
				case SymbolStyle.FuckYou:
					charMap = new string[] { "Fuck", "You" };
					break;
            }

            // Generate a 50-entry long string
            stringBuilder = new StringBuilder();
            do
            {
                for (int i = 0; i < 100; i++)
                    stringBuilder.Append(charMap[rnd.Next(0, charMap.Length)]);
            } while (generatedStrings.Contains(stringBuilder.ToString()));

            // Add generated string to the colelction.
            generatedStrings.Add(stringBuilder.ToString());

            // Increase renamedSymbol counter
            renamedSymbols++;

            // return the generated string
            return stringBuilder.ToString();
        }

        /// <summary>
        /// List to collect namespaces and the types of each namespace
        /// </summary>
        private static List<NamespaceTypeCollector> namespaceTypeColelctor = new List<NamespaceTypeCollector>();

        /// <summary>
        /// Store the namespace and the underlaying types in the NamespaceTypeCollector-list
        /// </summary>
        /// <param name="type">Type to store.</param>
        private static void StoreNamespace(TypeDef type)
        {
            bool foundNs = false;
            foreach (NamespaceTypeCollector ns in namespaceTypeColelctor)
                if (ns.NamespaceName == type.Namespace)
                {
                    foundNs = true;
                    ns.Types.Add(type);
                    break;
                }
            if (!foundNs)
            {
                NamespaceTypeCollector newNsStore = new NamespaceTypeCollector() { NamespaceName = type.Namespace };
                newNsStore.Types.Add(type);
                namespaceTypeColelctor.Add(newNsStore);
            }
        }

        /// <summary>
        /// Perform the symbol renaming on the specified module
        /// </summary>
        /// <param name="module">The module to rename.</param>
        public static void Run(ModuleDefMD module)
        {
            foreach (TypeDef type in module.GetTypes())
            {
                // Rename properties
                foreach (PropertyDef property in type.Properties)
					if (property.IsRenameable())
						property.Name = GenerateName();

				// Rename methods
				foreach (MethodDef method in type.Methods)
					if (method.IsRenameable())
						method.Name = GenerateName();

				// Rename fields
				foreach (FieldDef field in type.Fields)
					if (field.IsRenameable())
						field.Name = GenerateName();

				// Rename events
				foreach (EventDef @event in type.Events)
					if (@event.IsRenameable())
						@event.Name = GenerateName();
				
				// Rename types
				if (type.IsRenameable())
                {
                    // Handle resources & Resource types
                    CheckAssociatedResourceTypes(type);
                    type.Name = GenerateName();
                    RenameResourceManager(type);

					type.Namespace = GenerateName();
				}
            }

            // Rename resources
            RenameResources();

            // Log results
            Console.WriteLine(string.Format("Renamed {0} symbols.", renamedSymbols));

        }

        /// <summary>
        /// Holds all resources and the according type.
        /// </summary>
        private static List<ResourceTypeCollector> storedResources = new List<ResourceTypeCollector>();

        /// <summary>
        /// Holds all namespaces and the underlaying types.
        /// </summary>
        private static List<NamespaceTypeStore> storedNameSpaces = new List<NamespaceTypeStore>();

        /// <summary>
        /// Check if a typename (and namespacename) equals a a resourcename
        /// </summary>
        /// <param name="type">The type to check.</param>
        private static void CheckAssociatedResourceTypes(TypeDef type)
        {
            foreach (var resource in type.Module.Resources)
            {
                if (resource.ResourceType != ResourceType.Embedded)
                    continue;
                if (string.IsNullOrEmpty(new Regex(string.Format("^{0}.{1}\\.resources$", type.Namespace, type.Name)).Match(resource.Name).Value))
                    continue;
                storedResources.Add(new ResourceTypeCollector() { Type = type, Resource = resource as EmbeddedResource });
            }
        }
     
        /// <summary>
        /// Rename resource objects.
        /// </summary>
        private static void RenameResources()
        {
            foreach (var res in storedResources)
            {
                res.Resource.Name = string.Format("{0}.{1}.resources", res.Type.Namespace, res.Type.Name);
            }
        }

        /// <summary>
        /// Rename the Resourcemanager type
        /// </summary>
        /// <param name="type">The Resourcemanager type.</param>
        public static void RenameResourceManager(TypeDef type)
        {
            TypeRef resourceManagerRef = new TypeRefUser(type.Module, "System.Resources", "ResourceManager", type.Module.CorLibTypes.AssemblyRef);

            foreach (var property in type.Properties)
            {
                if (property.PropertySig.RetType.TypeName != "ResourceManager")
                    continue;

                //TODO: Replace FullName comparison
                if (property.PropertySig.RetType.FullName != resourceManagerRef.FullName)
                    continue;

                if (property.GetMethod == null || property.GetMethod.Name != "get_ResourceManager" || property.GetMethod.ReturnType.FullName != resourceManagerRef.FullName)
                    continue;

                if (!property.GetMethod.HasBody || !property.GetMethod.Body.HasInstructions) continue;

                foreach (var instruction in property.GetMethod.Body.Instructions.Where(i => i.OpCode == OpCodes.Ldstr)) 
                {
                    string newName = instruction.Operand.ToString();

                    foreach (var embeddedResource in type.Module.Resources)
                    {
                        if (embeddedResource.Name == string.Format("{0}.resources", instruction.Operand))
                        {
                            newName = GenerateName();
                            embeddedResource.Name = string.Format("{0}.resources", newName);
                        }
                    }

                    instruction.Operand = newName;
                }
            }
        }


    }
}
