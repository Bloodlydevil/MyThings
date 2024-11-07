using System;
using System.ComponentModel;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace MyThings.Extension
{
    public static class ExtensionObject
    {

        /// <summary>
        /// This function prints all the parameters of the object
        /// </summary>
        /// <param name="ob">the object</param>
        /// <typeparam name="type">Th eType Of THe Object</typeparam>
        public static type PrintAll<type>(this type ob)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(ob))
            {
                try
                {
                    Debug.Log(property.Name + " = " + property.GetValue(ob));
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            foreach (FieldInfo Field in ob.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                try
                {
                    Debug.Log(Field.Name + " = " + Field.GetValue(ob));
                    if (Field.FieldType.IsArray)
                        ((Array)Field.GetValue(ob)).PrintAll();
                    
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            return ob;
        }
        public static Array PrintAll(this Array ob)
        {
            foreach(var objec in ob)
            {
                objec.PrintAll();
            }
            return ob;
        }
        public static type[] PrintAllA<type>(this type[] ob)
        {
            foreach (var objec in ob)
            {
                objec.PrintAll();
            }
            return ob;
        }
        /// <summary>
        /// Print The Object
        /// </summary>
        /// <param name="ob">The Object TO Print</param>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        public static type Print<type>(this type ob,string add=null)
        {
            Debug.Log(ob+ add);
            return ob;
        }
        /// <summary>
        /// Print all The Object
        /// </summary>
        /// <param name="ob">The Object TO Print</param>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        public static type[] PrintA<type>(this type[] ob, string add = null)
        {
            foreach(type a in ob)
            {
                Debug.Log(a + add);
            }
            return ob;
        }
    }
}
