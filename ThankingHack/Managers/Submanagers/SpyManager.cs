using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using Object = UnityEngine.Object;

namespace Thanking.Managers.Submanagers
{
    public class SpyManager
    {
        public static MethodInfo[] PreSpy;
        public static Type[] Components;
        public static MethodInfo[] PostSpy;
        
        public static void Load()
        {
            List<MethodInfo> Pre = new List<MethodInfo>();
            List<Type> Comps = new List<Type>();
            List<MethodInfo> Post = new List<MethodInfo>();
            
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

            for (int i = 0; i < Types.Length; i++)
            {
                Type Type = Types[i];
                
                if(Type.IsDefined(typeof(SpyComponentAttribute), false))
                    Comps.Add(Type);
                
                MethodInfo[] Methods = Types[i].GetMethods();

                for (int o = 0; o < Methods.Length; o++)
                {
                    MethodInfo Method = Methods[i];
                    
                    if(Method.IsDefined(typeof(OnSpyAttribute), false))
                        Pre.Add(Method);
                    
                    if(Method.IsDefined(typeof(OffSpyAttribute), false))
                        Post.Add(Method);
                }
            }

            PreSpy = Pre.ToArray();
            Components = Comps.ToArray();
            PostSpy = Post.ToArray();
        }

        public static void InvokePre()
        {
            for (int i = 0; i < PreSpy.Length; i++)
                PreSpy[i].Invoke(null, null);
        }

        public static void InvokePost()
        {
            for (int i = 0; i < PostSpy.Length; i++)
                PostSpy[i].Invoke(null, null);
        }

        public static void DestroyComponents()
        {
            for(int i = 0; i < Components.Length; i++)
                Object.Destroy(Loader.HookObject.GetComponent(Components[i]));
        }

        public static void AddComponents()
        {
            for (int i = 0; i < Components.Length; i++)
                Loader.HookObject.AddComponent(Components[i]);
        }
    }
}