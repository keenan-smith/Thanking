using dnlib.DotNet;

namespace ThankingObfuscator.Renaming
{
    public static class Rules
    {

        /// <summary>
        /// Exclude types from renaming which do not match the criteria of renameable types.
        /// </summary>
        /// <param name="type">The type to check for being renameable.</param>
        /// <returns>Boolean which indicates if the type is renameable.</returns>
        public static bool IsRenameable(this TypeDef type)
        {
            if (type.IsRuntimeSpecialName ||
                type.IsSpecialName ||
                type.IsGlobalModuleType ||
                type.Name == "VoidCodeAttribute" ||
                type.InheritsFrom("System.Configuration.SettingsBase") ||
                type.IsImport || 
				type.Name == "Loader")
                return false;
            else
                return true;
        }


        /// <summary>
        /// Exclude methods from renaming which do not match the criteria of renameable methods.
        /// </summary>
        /// <param name="method">The method to check for being renameable.</param>
        /// <returns>Boolean which indicates if the method is renameable.</returns>
        public static bool IsRenameable(this MethodDef method)
        {
            if (method.IsRuntimeSpecialName ||
                method.IsRuntime ||
                method.IsConstructor ||
                method.HasOverrides ||
                method.IsVirtual ||
                method.IsAbstract ||
                method.Name.EndsWith("GetEnumerator") ||
                method.DeclaringType.IsDelegate() ||
                method.DeclaringType.IsComImport() && !method.HasAttribute("System.Runtime.InteropServices.DispIdAttribute") || 
				method.Name.Contains("Hook"))
                return false;
            else
                return true;
        }


        /// <summary>
        /// Exclude fields from renaming which do not match the criteria of renameable fields.
        /// </summary>
        /// <param name="field">The field to check for being renameable.</param>
        /// <returns>Boolean which indicates if the field is renameable.</returns>
        public static bool IsRenameable(this FieldDef field)
        {
            if (field.IsRuntimeSpecialName ||
                field.IsSpecialName ||
                field.DeclaringType.HasGenericParameters ||
                field.IsPinvokeImpl ||
                (field.DeclaringType.IsSerializable && !field.IsNotSerialized))
                return false;
            else
                return true;
        }


        /// <summary>
        /// Exclude properties from renaming which do not match the criteria of renameable properties.
        /// </summary>
        /// <param name="property">The property to check for being renameable.</param>
        /// <returns>Boolean which indicates if the property is renameable.</returns>
        public static bool IsRenameable(this PropertyDef property)
        {
            if (property.IsRuntimeSpecialName ||
                property.DeclaringType.Implements("System.ComponentModel.INotifyPropertyChanged") ||
                property.DeclaringType.Name.String.Contains("AnonymousType"))
                return false;
            else
                return true;
        }


        /// <summary>
        /// Exclude events from renaming which do not match the criteria of renameable events.
        /// </summary>
        /// <param name="@event">The event to check for being renameable.</param>
        /// <returns>Boolean which indicates if the event is renameable.</returns>
        public static bool IsRenameable(this EventDef @event)
        {
            if (@event.IsRuntimeSpecialName ||
                @event.IsSpecialName)
                return false;
            else
                return true;
        }


    }
}
