using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using MyThings.Extension;
using System.Linq;

namespace MyThings.Reflections
{
    
    /// <summary>
    /// A Class To Store The Path Of The Desired Object Data
    /// </summary>
    public class Path
    {
        #region Enum And Struct

        /// <summary>
        /// The Type Of Info Stored
        /// </summary>
        private enum InfoType
        {
            FieldInfo,
            PropertyInfo,
            MethodInfo,
        }
        /// <summary>
        /// A Strorage For Data Pointer
        /// </summary>
        private struct InfoStore
        {
            public InfoType Type;
            public int Index;
            public InfoStore(InfoType type, int index)
            {
                Type = type;
                Index = index;
            }
        }

        #endregion

        #region Delegate

        /// <summary>
        /// The Field Track To Feed The data when current track is a field
        /// </summary>
        /// <param name="data">The Object From which we extracted The Field</param>
        /// <param name="info">The current Field track</param>
        /// <returns>Was The Operation Successfull</returns>
        public delegate bool FieldTrack(ref object data, FieldInfo info);
        /// <summary>
        /// The Property Track To Feed The data when current track is a Property
        /// </summary>
        /// <param name="data">The Object From which we extracted The Property</param>
        /// <param name="info">The current Property track</param>
        /// <returns>Was The Operation Successfull</returns>
        public delegate bool PropertyTrack(ref object data, PropertyInfo info);
        /// <summary>
        /// The Field Track To Feed The data when current track is field
        /// </summary>
        /// <param name="data">The Object From which we extracted The Method</param>
        /// <param name="index">The Current Index For Method Paramters</param>
        /// <param name="info">The current Method track</param>
        /// <returns>Was The Operation Successfull</returns>
        public delegate bool MethodTrack(ref object data,ref int index, MethodInfo info);

        #endregion

        #region List

        /// <summary>
        /// All The fields in this Path
        /// </summary>
        private readonly List<FieldInfo> Fields = new();
        /// <summary>
        /// All The Properties In this Path
        /// </summary>
        private readonly List<PropertyInfo> Properties = new();
        /// <summary>
        /// All the Methods In This Path
        /// </summary>
        private readonly List<MethodInfo> Methods = new();
        /// <summary>
        /// All The Index Stored In This Path (As Long As The Path)
        /// </summary>
        private readonly List<InfoStore> Index = new();

        #endregion

        private void Clear()
        {
            Fields.Clear();
            Properties.Clear();
            Methods.Clear();
            Index.Clear();
        }
        /// <summary>
        /// Travel Through The Path
        /// </summary>
        /// <param name="start">The Place To Start From</param>
        /// <param name="end">The End To Stop At</param>
        /// <param name="obj">The Object That Has TO Go Throught the Path</param>
        /// <param name="fieldTrack">What To do When We Encounter A Field In Path</param>
        /// <param name="propertyTrack">What To do When We Encounter A Property In Path</param>
        /// <param name="methodTrack">What To do When We Encounter A Method In Path</param>
        /// <returns>Was The Task Successfull</returns>
        private bool TravelPath(int start, int end, ref object obj, FieldTrack fieldTrack, PropertyTrack propertyTrack, MethodTrack methodTrack)
        {
            int index = 0;
            for (int i = start < 0 ? 0 : start; i < end; i++)
            {
                switch (Index[i].Type)
                {
                    case InfoType.FieldInfo:
                        if (!fieldTrack(ref obj, Fields[Index[i].Index]))
                        {
                            Debug.LogError("Could Not Travel The Path Due To Some Error");
                            return false;
                        }
                        break;
                    case InfoType.PropertyInfo:
                        if (!propertyTrack(ref obj, Properties[Index[i].Index]))
                        {
                            Debug.LogError("Could Not Travel The Path Due To Some Error");
                            return false;
                        }
                        break;
                    case InfoType.MethodInfo:
                        if (!methodTrack(ref obj, ref index, Methods[Index[i].Index]))
                        {
                            Debug.LogError("Could Not Travel The Path Due To Some Error");
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        #region Public

        /// <summary>
        /// Build Path Based On THe Parameter Given
        /// </summary>
        /// <param name="type">The Base Type To Start From</param>
        /// <param name="flags">The Binding Flags To Use While Finding Path</param>
        /// <param name="Paths">The Paths To Make</param>
        /// <returns>If The Task Was Successful</returns>
        public bool BuildPath(Type type,BindingFlags flags, string[] Paths, string[] Generics)
        {
            if (type == null)
            {
                Debug.LogError($"Type Was Given As Null So Cant Build Anything  {Paths}");
                return false;
            }
            Clear();
            int index = 0;
            for (int i = 0; i < Paths.Length; i++)
            {
                if (type == null)
                {
                    Debug.LogError($"Type Was Returned As Null For  {GetLastName()}  ");
                    return false;
                }
                if (type.TryGetField(flags, Paths[i], out FieldInfo field))
                {
                    Index.Add(new(InfoType.FieldInfo, Fields.Count));
                    Fields.Add(field);
                    type = field.FieldType;
                }
                else if (type.TryGetProperty(flags, Paths[i], out PropertyInfo property))
                {
                    Index.Add(new(InfoType.PropertyInfo, Properties.Count));
                    Properties.Add(property);
                    type = property.PropertyType;
                }
                else if (type.TryGetMethod(flags, Paths[i], out MethodInfo method))
                {
                    
                    if (method.IsGenericMethod)
                    {
                        method =method.MakeGenericMethod(method.MethodGenericsSolver(Generics.Skip(index).Take(method.GetGenericArguments().Length)));
                        index += method.GetGenericArguments().Length;
                    }
                    Index.Add(new(InfoType.MethodInfo, Methods.Count));
                    Methods.Add(method);
                    type = method.ReturnType;
                }
                else
                {
                    Debug.LogError($"Could Not Find Any Suitable Thing For  {type} for this path=> {Paths[i]}");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Travel Through The Path
        /// </summary>
        /// <param name="obj">The Object That Has TO Go Throught the Path</param>
        /// <param name="fieldTrack">What To do When We Encounter A Field In Path</param>
        /// <param name="propertyTrack">What To do When We Encounter A Property In Path</param>
        /// <param name="methodTrack">What To do When We Encounter A Method In Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TravelPath(ref object obj, FieldTrack fieldTrack, PropertyTrack propertyTrack, MethodTrack methodTrack)
        {
            return TravelPath(0, Index.Count, ref obj, fieldTrack, propertyTrack, methodTrack);
        }
        /// <summary>
        /// Travel throught The Path But Don't Reach The Destination
        /// </summary>
        /// <param name="obj">The Object That Has TO Go Throught the Path</param>
        /// <param name="fieldTrack">What To do When We Encounter A Field In Path</param>
        /// <param name="propertyTrack">What To do When We Encounter A Property In Path</param>
        /// <param name="methodTrack">What To do When We Encounter A Method In Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TravelPathLeavingDestination(ref object obj, FieldTrack fieldTrack, PropertyTrack propertyTrack, MethodTrack methodTrack)
        {
            return TravelPath(0, Index.Count - 1, ref obj, fieldTrack, propertyTrack, methodTrack);
        }
        /// <summary>
        /// Travel Only Throught Destination
        /// </summary>
        /// <param name="obj">The Object That Has TO Go Throught the Path</param>
        /// <param name="fieldTrack">What To do When We Encounter A Field In Path</param>
        /// <param name="propertyTrack">What To do When We Encounter A Property In Path</param>
        /// <param name="methodTrack">What To do When We Encounter A Method In Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool ReachDestination(ref object obj, FieldTrack fieldTrack, PropertyTrack propertyTrack, MethodTrack methodTrack)
        {
            return TravelPath(Index.Count-1, Index.Count, ref obj, fieldTrack,propertyTrack, methodTrack);
        }
        /// <summary>
        /// Get The name Of The Last Track
        /// </summary>
        /// <returns>Last Track Name</returns>
        public string GetLastName()
        {
            return Index[^1].Type switch
            {
                InfoType.FieldInfo => Fields[^1].Name,
                InfoType.PropertyInfo => Properties[^1].Name,
                InfoType.MethodInfo => Methods[^1].Name,
                _ => null,
            };
        }
        /// <summary>
        /// Get The return Type Of Last Track
        /// </summary>
        /// <returns>The Return Type Of Last Track</returns>
        public Type GetLastType()
        {
            return Index[^1].Type switch
            {
                InfoType.FieldInfo => Fields[^1].FieldType,
                InfoType.PropertyInfo => Properties[^1].PropertyType,
                InfoType.MethodInfo => Methods[^1].ReturnType,
                _ => null,
            };
        }

        #endregion
    }
}