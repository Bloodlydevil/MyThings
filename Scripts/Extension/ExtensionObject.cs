using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

namespace MyThings.Extension
{
    public static class ExtensionObject
    {
        /// <summary>
        /// Print The string in Color
        /// </summary>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        /// <param name="ob">The Object</param>
        /// <param name="color">The Color To Use</param>
        private static void ColoredPrint<type>(this type ob, Color? color)
        {
            if (color.HasValue)
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(color.Value)}>{ob}</color>");
            else
                Debug.Log(ob);
        }
        /// <summary>
        /// This function prints all the parameters of the object
        /// </summary>
        /// <param name="ob">the object</param>
        /// <typeparam name="type">Th eType Of THe Object</typeparam>
        public static type PrintAll<type>(this type ob,Color? color=null)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(ob))
            {
                try
                {
                    ColoredPrint(property.Name + " = " + property.GetValue(ob), color);
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
                    ColoredPrint(Field.Name + " = " + Field.GetValue(ob),color);
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
        /// <summary>
        /// Print All The Data
        /// </summary>
        /// <param name="ob">The Array Object</param>
        /// <param name="color">The Color</param>
        /// <returns></returns>
        public static Array PrintAll(this Array ob, Color? color = null)
        {
            foreach(var objec in ob)
            {
                objec.PrintAll(color);
            }
            return ob;
        }
        /// <summary>
        /// Print All The Elements In The Object in The Array
        /// </summary>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        /// <param name="ob">The Object</param>
        /// <param name="color">The Color Of The Object</param>
        /// <returns></returns>
        public static type[] PrintAllA<type>(this type[] ob, Color? color = null)
        {
            foreach (var objec in ob)
            {
                objec.PrintAll(color);
            }
            return ob;
        }
        /// <summary>
        /// Print The Object
        /// </summary>
        /// <param name="ob">The Object TO Print</param>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        public static type Print<type>(this type ob,string add=null, Color? color = null)
        {
            ColoredPrint(ob + add, color);
            return ob;
        }
        /// <summary>
        /// Print all The Objects
        /// </summary>
        /// <param name="ob">The Object TO Print</param>
        /// <typeparam name="type">The Type Of The Object</typeparam>
        public static type[] PrintA<type>(this type[] ob, string add = null, Color? color = null)
        {
            foreach(type a in ob)
            {
                Print(a, add, color);
            }
            return ob;
        }
        /// <summary>
        /// Print all The Objects
        /// </summary>
        /// <typeparam name="Collection">The Collection To Print</typeparam>
        /// <typeparam name="type">The Type Of The Collection</typeparam>
        /// <param name="ob">The Objects</param>
        /// <param name="add">The String To Add</param>
        /// <param name="color">The Color To Use</param>
        /// <returns></returns>
        public static Collection PrintA<Collection,type>(this Collection ob,string add=null, Color? color = null) where Collection :ICollection<type>
        {
            foreach (var item in ob)
            {
                item.Print(add, color);
            }
            return ob;
        }
    }
}
