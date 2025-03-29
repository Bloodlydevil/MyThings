using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MyThings.Extension
{
    public static class ExtensionReflection
    {
        /// <summary>
        /// Tyr To Get The Property If It Is Valid
        /// </summary>
        /// <param name="type">The Type In Which To Find Property</param>
        /// <param name="flags">The BindingFlag To Use To Find The Property</param>
        /// <param name="Path">The Property Name</param>
        /// <param name="property">The Property Found</param>
        /// <returns>Property Validity</returns>
        public static bool TryGetProperty(this Type type, BindingFlags flags, string Path, out PropertyInfo property)
        {
            property = type.GetProperty(Path, flags);
            return property != null;
        }
        /// <summary>
        /// Try To Get The Method If It Is Valid
        /// </summary>
        /// <param name="type">The Type In Which To Find The Method</param>
        /// <param name="flags">The BindingFlag To Use To Find The Method</param>
        /// <param name="Path">The Method Name</param>
        /// <param name="method">The Method Found</param>
        /// <returns>Method Validity</returns>
        public static bool TryGetMethod(this Type type, BindingFlags flags, string Path, out MethodInfo method)
        {
            method = type.GetMethod(Path, flags);
            return method != null;
        }
        /// <summary>
        /// Try To Get The Method Based On The Generics
        /// </summary>
        /// <param name="type">The Type In Which TO Search For Method</param>
        /// <param name="flags">The Flags With Witch To Serach In THE Type</param>
        /// <param name="Path">The Path Of tHe Method</param>
        /// <param name="Generics">The Genrics TO Watchout For</param>
        /// <param name="method">The Method Found</param>
        /// <returns>Method Found With The Given Parms</returns>
        public static bool TryGetMethod(this Type type,BindingFlags flags,string Path, string[] Generics,out MethodInfo method)
        {
            method = type.GetMethod(Path, flags);
            method = method.MakeGenericMethod(MethodGenericsSolver(method, Generics));
            return method != null;
        }
        /// <summary>
        ///  Try To Get The Field If It Is Valid
        /// </summary>
        /// <param name="type">The Type In Which To Find The Field</param>
        /// <param name="flags">The BindingFlag To Use To Find The Field</param>
        /// <param name="Path">The Field Name</param>
        /// <param name="field">The Field Found</param>
        /// <returns>Field Validity</returns>
        public static bool TryGetField(this Type type, BindingFlags flags, string Path, out FieldInfo field)
        {
            field = type.GetField(Path, flags);
            return field != null;
        }
        /// <summary>
        /// Convert The String Data To The Type Of Object
        /// </summary>
        /// <param name="type">The Type To Convert To</param>
        /// <param name="Data">The String tO Convert</param>
        /// <returns>The Converted Object</returns>
        public static object ConvertObject(this Type type, string Data)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            object temp = null;
            try
            {
                temp = converter.ConvertFromString(Data);
            }
            catch (Exception)
            {
                Debug.LogError($"The Data {Data} Was Not Provided in Correct Format  {type.Name}");
            }
            return temp;
        }
        /// <summary>
        /// Convert All The String To The Method Paramter
        /// </summary>
        /// <param name="method">The Method TO Feed Data</param>
        /// <param name="PathMembers">The Data To Feed</param>
        /// <returns>The Converted Data To Feed</returns>
        public static object[] MethodParameterSolver(this MethodInfo method, IEnumerable<string> PathMembers)
        {
            var Parameters = method.GetParameters();
            object[] result = new object[Parameters.Length];
            int i = 0;
            foreach (var param in PathMembers)
            {
                result[i] = ConvertObject(Parameters[i++].ParameterType, param);
            }
            return result;
        }
        /// <summary>
        /// Convert All The String To The Method Paramter
        /// </summary>
        /// <param name="method">The Method TO Feed Data</param>
        /// <param name="PathMembers">The Data To Feed</param>
        /// <returns>The Converted Data To Feed</returns>
        public static object[] MethodParameterSolver(this MethodInfo method, params string[] PathMembers)
        {
            var Parameters = method.GetParameters();
            if (Parameters.Length != PathMembers.Length)
            {
                Debug.LogError("The Parameters Size Does Not Match");
                return null;
            }
            object[] result = new object[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
            {
                result[i] = ConvertObject(Parameters[i].ParameterType, PathMembers[i]);
            }
            return result;
        }
        /// <summary>
        /// Convert All The String To Generics Type
        /// </summary>
        /// <param name="methodInfo">The Method To GEt THE Generics For</param>
        /// <param name="Generics">Teh Generics To GIve</param>
        /// <returns>The Generics</returns>
        public static Type[] MethodGenericsSolver(this MethodInfo methodInfo, params string[] Generics) 
        {
            if(Generics.Length!= methodInfo.GetGenericArguments().Length)
            {
                Debug.LogError("The Generics Size Does Not Match");
                return null;
            }
            Type[] result = new Type[Generics.Length];
            for(int i = 0;i < Generics.Length;i++)
            {
                result[i] =Type.GetType(Generics[i]);
            }
            return result;
        }
        /// <summary>
        /// Convert All The String To Generics Type
        /// </summary>
        /// <param name="methodInfo">The Method To GEt THE Generics For</param>
        /// <param name="Generics">Teh Generics To GIve</param>
        /// <returns>The Generics</returns>
        public static Type[] MethodGenericsSolver(this MethodInfo methodInfo, IEnumerable<string> Generics)
        {
            Type[] result = new Type[methodInfo.GetGenericArguments().Length];
            int i = 0;
            foreach (string s in Generics)
            {
                result[i++] = Type.GetType(s);
            }
            return result;
        }
        /// <summary>
        /// Get the Recursion Length (Debug purpose only)
        /// </summary>
        /// <param name="obj">the object in which to look for recursion</param>
        /// <returns>the recursion length</returns>
        public static int GetRecursionLength(this object obj)
        {
            int ans = 0;
            Type ObjectType = obj.GetType();
            HashSet<object> visited = new()
            {
                obj
            };
            try
            {
                FieldInfo ObjectFieldInfo =
                    ObjectType.GetFields(BindingFlags.Public| BindingFlags.NonPublic| BindingFlags.Instance)
                    .Where(i => i.FieldType.IsAssignableFrom(ObjectType))
                    .First();
                if (ObjectFieldInfo == null)
                    return -1;
                while (true)
                {
                    obj = ObjectFieldInfo.GetValue(obj);
                    if (obj == null)
                        break;
                    if (visited.Contains(obj))
                        break;
                    visited.Add(obj);
                    ans++;
                }
            }
            catch (Exception)
            {
                Debug.Log("Recursion not found");
            }
            return ans;
        }
        public static Type[] GetAllTypeInAssembly<type>()
        {
           return Assembly.GetExecutingAssembly().GetTypes().Where(i=>i.IsAssignableFrom(typeof(type))).ToArray();
        }
        /// <summary>
        /// Check If A Class Is Child Of Another Class
        /// </summary>
        /// <param name="Child">The Child Class</param>
        /// <param name="parent">The Parent Class</param>
        /// <returns>Is Child Or Not?</returns>
        public static bool IsChildClassOf(this Type Child, Type parent)
        {
            return parent.IsAssignableFrom(Child);
        }
    }
}
