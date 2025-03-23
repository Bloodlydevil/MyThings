using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyThings.Extension;
using UnityEngine;
using static MyThings.Reflections.Path;


//com.unity.nuget.newtonsoft-json


using Newtonsoft.Json;

namespace MyThings.Reflections
{

    [DefaultExecutionOrder(-10000)]
    public class DataManager : ExtendableClass.Singleton_C<DataManager>
    {
        // complex data
        // Create Your Own Dev Console
        /// <summary>
        /// A Storage For Us To Do Rollback When We Find Struct (Struct Is Value Type So Data Needs TO be Written Back
        /// </summary>
        private class SingleReSet
        {
            /// <summary>
            /// The Parent Object
            /// </summary>
            public object ParentObject;
            /// <summary>
            /// The Child Object(The One Which Is Struct for Sure)
            /// </summary>
            public object ChildObject;
            /// <summary>
            /// The Way To Roll Back
            /// </summary>
            public Action<object, object> Roll;
            /// <summary>
            /// RollBack The Object
            /// </summary>
            public void RollBack() => Roll(ParentObject, ChildObject);
        }

        /// <summary>
        /// The Stack Used TO RollBack The Object
        /// </summary>
        private readonly Stack<SingleReSet> m_ReSetStack = new Stack<SingleReSet>();

        /// <summary>
        /// The Bindings To Look For Everywhere
        /// </summary>
        private static BindingFlags m_BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #region Private


        /// <summary>
        /// Try To Determine If The Given Class Is Valid Or Not
        /// </summary>
        /// <param name="Class">The Class TO Check</param>
        /// <param name="type">The Type Of Class tis is</param>
        /// <returns>Class Validity</returns>
        private bool TryGetClassType(string Class,out Type type)
        {
            type = Type.GetType(Class);
            if (type == null)
            {
                Debug.LogError("The Class Is Not A Valid Class");
                return false;
            }
            if (!type.IsSubclassOf(typeof(UnityEngine.Object)))// cannot find a class which is not a unity object
            {
                Debug.LogError("The Class Is Not A SubClass Of Unity Object");
                return false;// may need to remove
            }
            return true;
        }
        /// <summary>
        /// Get All The Mono In Game
        /// </summary>
        /// <returns>All The MonoBehaviour</returns>
        private MonoBehaviour[] GetAllObjectsOfMono()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
        /// <summary>
        /// Recursively check If ToCheck Is A SubClass Of Target or not
        /// </summary>
        /// <param name="ToCheck">The Object TO Check</param>
        /// <param name="Target">The Object TO find</param>
        /// <returns>If It Is Or Not</returns>
        private bool ISOfType(Type ToCheck,Type Target)
        {
            if(ToCheck == null) return false;
            if(ToCheck == Target) return true;
            return ISOfType(ToCheck.BaseType, Target);
        }
        /// <summary>
        /// RollBack The Object
        /// </summary>
        private void RollBack()
        {
            while (m_ReSetStack.Count != 0)
            {
                m_ReSetStack.Pop().RollBack();
            }
        }


        #region Getters

        #region Read

        /// <summary>
        /// Get A FieldTrack Based On If It Is Rollback Or Not
        /// </summary>
        /// <param name="Rollback">rollBack Allowed Or Not</param>
        /// <returns>The FieldTrack</returns>
        private FieldTrack GetReadField(bool Rollback = false)
        {
            return Rollback switch
            {
                false => (ref object obj, FieldInfo fieldinfo) =>
                {
                    obj = fieldinfo.GetValue(obj);
                    if (obj == null)
                    {
                        Debug.LogError($"The Object Was Returned As Null from the Field {fieldinfo.FieldType}");
                        return false;
                    }
                    return true;
                },




                true => (ref object obj, FieldInfo fieldinfo) =>
                {
                    object temp = fieldinfo.GetValue(obj);
                    if (temp == null)
                    {
                        Debug.LogError($"The Object Was Returned As Null from the Field {fieldinfo.FieldType}");
                        return false;
                    }
                    if (fieldinfo.FieldType.IsValueType)
                    {
                        m_ReSetStack.Push(new SingleReSet
                        {
                            ParentObject = obj,
                            ChildObject = temp,
                            Roll = (o, j) => fieldinfo.SetValue(o, j)
                        });
                    }
                    obj = temp;
                    return true;
                }
            };
        }
        /// <summary>
        /// Get A PropertyTrack Based On If It Is Rollback Or Not
        /// </summary>
        /// <param name="Rollback">rollBack Allowed Or Not</param>
        /// <returns>The PropertyTrack</returns>
        private PropertyTrack GetReadProperty(bool Rollback = false)
        {
            return Rollback switch
            {
                false => (ref object obj, PropertyInfo propertyInfo) =>
                {
                    if (!propertyInfo.CanRead)
                    {
                        Debug.Log($"The Property Could Read Usage Was Not Found Same  {propertyInfo.PropertyType}");
                        return false;
                    }
                    obj = propertyInfo.GetValue(obj); ;
                    if (obj == null)
                    {
                        Debug.LogError($"The Object Was Returned As Null from the Property {propertyInfo.PropertyType}");
                        return false;
                    }
                    return true;
                },



                true => (ref object obj, PropertyInfo propertyInfo) =>
                {
                    if (!propertyInfo.CanRead)
                    {
                        Debug.Log($"The Property Could Read Usage Was Not Found Same  {propertyInfo.PropertyType}");
                        return false;
                    }
                    object temp = propertyInfo.GetValue(obj);
                    if (propertyInfo.PropertyType.IsValueType)
                    {
                        if (!propertyInfo.CanWrite)
                        {
                            Debug.LogError($"Roll Back Not Available For Property {propertyInfo.PropertyType}");
                        }
                        else
                        {
                            m_ReSetStack.Push(new SingleReSet
                            {
                                ParentObject = obj,
                                ChildObject = temp,
                                Roll = (o, j) => propertyInfo.SetValue(o, j)
                            });
                        }
                    }
                    obj = temp;
                    if (obj == null)
                    {
                        Debug.LogError($"The Object Was Returned As Null from the Property {propertyInfo.PropertyType}");
                        return false;
                    }
                    return true;
                }
            };
        }
        /// <summary>
        /// Get A MethodTrack 
        /// </summary>
        /// <param name="MethodInput">The Method Input To Feed From</param>
        /// <returnsThe MethodTrack></returns>
        private MethodTrack GetReadMethod(string[] MethodInput)
        {
            return (ref object obj, ref int index, MethodInfo methodInfo) =>
                {
                    if (methodInfo.ReturnType == null)
                    {
                        Debug.Log($"The Method Return Type Is Null {methodInfo.ReturnType}");
                        return false;
                    }
                    obj = methodInfo.Invoke(obj,
                        methodInfo.MethodParameterSolver(MethodInput.Skip(index).Take(methodInfo.GetParameters().Length)));
                    if (obj == null)
                    {
                        Debug.LogError($"The Object Was Returned As Null from the Method {methodInfo.Name}");
                        return false;
                    }
                    index += methodInfo.GetParameters().Length;
                    return true;
                };
        }

        #endregion

        #region Write

        /// <summary>
        /// get a FieldTrack Where We Write Data
        /// </summary>
        /// <param name="Value">The Value To Write</param>
        /// <returns>The FieldTrack</returns>
        private FieldTrack GetWriteField(string Value)
        {
            return (ref object obj, FieldInfo FieldInfo) =>
            {
                var PValue = FieldInfo.FieldType.ConvertObject(Value);
                FieldInfo.SetValue(obj, PValue);
                RollBack();
                return true;
            };
        }
        /// <summary>
        ///  get a PropertyTrack Where We Write Data
        /// </summary>
        /// <param name="Value">The Value To Write</param>
        /// <returns>The PropertyTrack</returns>
        private PropertyTrack GetWriteProperty(string Value)
        {
            return (ref object obj, PropertyInfo propertyInfo) =>
            {
                if (!propertyInfo.CanWrite)
                {
                    Debug.LogError("The Property Does Not Have A Setter");
                    return false;
                }
                var PValue = propertyInfo.PropertyType.ConvertObject(Value);
                propertyInfo.SetValue(obj, PValue);
                RollBack();
                return true;
            };
        }
        /// <summary>
        /// get a MethodTrack Where We Write Data
        /// </summary>
        /// <param name="Value">The Value To Write</param>
        /// <returns>The MethodTrack</returns>
        private MethodTrack GetWriteMethod(string[] Value)
        {
            return (ref object obj, ref int index, MethodInfo MethodInfo) =>
            {
                var ValueInputer = MethodInfo.MethodParameterSolver(Value);
                try
                {
                    MethodInfo.Invoke(obj, ValueInputer);
                    RollBack();
                }
                catch (Exception ex)
                {
                    Debug.LogError("The Function Gave Error");
                    Debug.LogError(ex.Message);
                    return false;
                }
                return true;
            };
        }

        #endregion

        #region False

        /// <summary>
        /// Get A false Property Track
        /// </summary>
        /// <returns>Property Track</returns>
        private PropertyTrack GetFalseProperty()
        {
            return (ref object obj, PropertyInfo propertyInfo) =>
            {
                return false;
            };
        }
        /// <summary>
        /// Get A False Method Property Track
        /// </summary>
        /// <returns>Method Track</returns>
        private MethodTrack getFalseMethod()
        {
            return (ref object obj, ref int index, MethodInfo methodInfo) =>
            {
                return false;
            };
        }

        #endregion

        private FieldTrack GetEventTrack(string[] Value)
        {
            return (ref object obj, FieldInfo eventInfo) =>
            {
                var eventDelegate = (MulticastDelegate)eventInfo.GetValue(obj);
                if (eventDelegate == null)
                {
                    Debug.LogError("The Given Object Is Not Proper Event");
                    return false;
                }
                var Data=eventDelegate.GetMethodInfo().MethodParameterSolver(Value);
                foreach (var handler in eventDelegate.GetInvocationList())
                {
                    handler.Method.Invoke(handler.Target, Data);
                }
                return true;
            };
        }

        #endregion

        #endregion


        #region Public


        /// <summary>
        /// Try To Get The First Class We Find Of The Required Type
        /// </summary>
        /// <param name="Class">The Class To Find</param>
        /// <param name="single">Teh Object We Find</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryGetObject(string Class, out MonoBehaviour single)
        {
            if (!TryGetClassType(Class, out Type type))
            {
                Debug.LogError("Could Not Determine A Valid Class");
                single = null;
                return false;
            }
            single = GetAllObjectsOfMono().First(mono => ISOfType(mono.GetType(), type));

            if (single == null)
            {
                Debug.LogError("Could Not Find The Object");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try To Get The Object Based On The given Parameters
        /// </summary>
        /// <param name="Single">The object</param>
        /// <param name="Class">The class to Find In The Game</param>
        /// <param name="IPathString">The Path To Use To find The object</param>
        /// <param name="IValue">The Value To USe TO Compare The Object</param>
        /// <param name="IMethodvalue">The Method Value TO Give Wen Required</param>
        /// <param name="DataSeparator">The Seprator Used To Get The Tracks</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryGetObject(out MonoBehaviour Single, string Class, string IPathString, string IValue, string[] IMethodvalue = default, string[] GenericM=default, string DataSeparator = ".")
        {
            if (!TryGetClassType(Class, out Type type))
            {
                Debug.LogError("Could Not Determine A Valid Class");
                Single = null;
                return false;
            }
            var objects = GetAllObjectsOfMono().Where(mono => ISOfType(mono.GetType(), type));

            string[] IPaths = IPathString.Split(DataSeparator);

            Path IPath = new Path();
            if (!IPath.BuildPath(type, m_BindingFlags, IPaths, GenericM))
            {
                Debug.LogError($"Could Not Build PAth For The Given Identifier ");
                Debug.LogError($"Class=> {Class}");
                Debug.LogError($"Identifier Path =>{IPathString}");
                Single = null;
                return false;
            }


            FieldTrack IField = GetReadField();
            PropertyTrack IProperty = GetReadProperty();
            MethodTrack IMethod = GetReadMethod(IMethodvalue);
            string IValueObj = JsonConvert.SerializeObject(IValue);
            if (IValueObj == null)
            {
                Debug.LogError($"Could Not Generate Object BAsed On The Given Identifier Value for  {IValue}  ");
                Single = null;
                return false;
            }
            Single = null;
            foreach (var obje in objects)
            {
                object obj = obje;
                IPath.TravelPath(ref obj, IField, IProperty, IMethod);
                if (JsonConvert.SerializeObject(obj) == IValueObj)
                {
                    Single = obje;
                    break;
                }
            }

            if (Single == null)
            {
                Debug.LogError("Could Not Find The Given object Based On The Info Given");
                return false;
            }
            return true;
        }

        #region Push


        /// <summary>
        /// Push Value In The Object In the Path
        /// </summary>
        /// <param name="obj">The Object To Push Data In</param>
        /// <param name="path">The Path In Which To Sed The Data</param>
        /// <param name="value">Teh Data To Push</param>
        /// <param name="MethodGivePrams">The Data To Feed Method When We Incounter It</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPushValue(object obj, Path path, string[] value, string[] MethodGivePrams = default)
        {

            if (!path.TravelPathLeavingDestination(ref obj, GetReadField(true), GetReadProperty(true), GetReadMethod(MethodGivePrams)))
            {
                Debug.LogError("Could Not Travel On The Path For THe Given Object");
                return false;
            }
            if (!path.ReachDestination(ref obj, GetWriteField(value as object as string), GetWriteProperty(value as object as string), GetWriteMethod(value)))
            {
                Debug.LogError("Could Not Reach The Destination For The Given Object");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Push Value In The Object In the Path
        /// </summary>
        /// <param name="single">The object</param>
        /// <param name="DataPath">The Path To Take TO Push The Value</param>
        /// <param name="value">The Value To Push</param>
        /// <param name="MethodGivePrams">Teh Value To Inject Into Methods</param>
        /// <param name="DataSeparator">Teh Data Separator Used</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPushValue(MonoBehaviour single,string DataPath, string[] value, string[] GenericM = default, string[] MethodGivePrams = default, string DataSeparator=".")
        {
            string[] Paths = DataPath.Split(DataSeparator);

            Path Path = new Path();
            if (!Path.BuildPath(single.GetType(), m_BindingFlags, Paths,GenericM))
            {
                Debug.LogError("Could Not Build PAth For The Given Identifier ");
                Debug.LogError($"Class=> {single.GetType()}");
                Debug.LogError($"Identifier Path =>{DataPath}");
                return false;
            }

            if (!TryPushValue(single, Path, value, MethodGivePrams))
            {
                Debug.LogError("Could Not Set The Value For The Object");
                return false;
            }
            return true;
        }
        /// <summary>
        ///  Try to Push The Value To The Object If Possible
        /// </summary>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Push The Data In</param>
        /// <param name="Value">The Value Which We Have TO Push</param>
        /// <param name="MethodGiveParams">The Value To Give To Method While Pushing Data To The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPushValue(string Class, string DataPath, string[] Value, string[] GenericM = default, string[] MethodGiveParams = default, string DataSeparator = ".")
        {
            if (!TryGetObject(Class, out MonoBehaviour single))
            {
                Debug.LogError("Could Not Accuire A Single Object To Get The Value");
                return false;
            }
            if (!TryPushValue(single, DataPath, Value,GenericM, MethodGiveParams, DataSeparator))
            {
                Debug.LogError("Could Not Push The Value");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try to Push The Value To The Object If Possible
        /// </summary>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="IPathString">The Path To Use To Find The Unique Object</param>
        /// <param name="IValue">The Unique Value Used TO Find The Object</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Push The Data In</param>
        /// <param name="value">The Value Which We Have TO Push</param>
        /// <param name="IMethodvalue">The Values To Give The Method Which Finding The REquired Object</param>
        /// <param name="MethodParams">The Value To Give To Method While Pushing Data To The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPushValue(string Class, string IPathString, string IValue, string DataPath, string[] value
            , string[] GenericM = default, string[] IMethodvalue = default, string[] MethodParams = default, string DataSeparator = ".")
        {
            if (!TryGetObject(out MonoBehaviour matchedObject, Class, IPathString, IValue, IMethodvalue,GenericM , DataSeparator))
            {
                Debug.LogError("Could Not Locate The Object With The Given Parameter");
                return false;
            }
            if (!TryPushValue(matchedObject, DataPath, value, GenericM, MethodParams, DataSeparator))
            {
                Debug.LogError("Could Not Push The Value");
                return false;
            }
            return true;
        }


        #endregion

        #region Pop


        /// <summary>
        /// Pop Value From The Given Object
        /// </summary>
        /// <param name="ReturnValue">The Result Of The Pop</param>
        /// <param name="single">The Object To Pop The Value From</param>
        /// <param name="DataPath">The Path To Be Taken</param>
        /// <param name="MethodGivePrams">The Values To Feed To Method Along The Path</param>
        /// <param name="DataSeparator">The Seperator used</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPopValue(out object ReturnValue, MonoBehaviour single,string DataPath, string[] GenericM = default, string[] MethodGivePrams = default, string DataSeparator = ".")
        {
            string[] DataPaths = DataPath.Split(DataSeparator);

            Path path = new Path();

            if (!path.BuildPath(single.GetType(), m_BindingFlags, DataPaths, GenericM))
            {
                Debug.LogError("Could Not Biuld Path For The Data Given");
                Debug.LogError($" Class=>  {single.GetType()} ");
                Debug.LogError($"  DataPath  =>{DataPath}");
                ReturnValue = null;
                return false;
            }

            if (!TryPopValue(out ReturnValue, single, path, MethodGivePrams))
            {
                Debug.LogError("Could Not Pop The Value For The Given Object");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Pop Value From The Given Object
        /// </summary>
        /// <param name="obj">The Object To Pop The Value From</param>
        /// <param name="path">The Path From Which TO Pop The Value</param>
        /// <param name="Result">The Result Of The Pop</param>
        /// <param name="MethodGivePrams">The Values To Feed To Method Along The Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPopValue(out object Result, object obj,Path path,string[] MethodGivePrams=default)
        {
            if (!path.TravelPath(ref obj, GetReadField(true), GetReadProperty(true), GetReadMethod(MethodGivePrams)))
            {
                Result = null;
                Debug.LogError("Could Not Travel On The Path For THe Given Object");
                return false;
            }
            Result = obj;
            return true;
        }
       
        /// <summary>
        /// Try To Get THe Value Of THe Object While Walking Along The Path
        /// </summary>
        /// <param name="ReturnValue">The Object Poped From The Path</param>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="IPathString">The Path To Use To Find The Unique Object</param>
        /// <param name="IValue">The Unique Value Used TO Find The Object</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Poo The Data From</param>
        /// <param name="IMethodvalue">The Values To Give The Method Which Finding The REquired Object</param>
        /// <param name="MethodParams">The Value To Give To Method While Poping Data From The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPopValue(out object ReturnValue, string Class, string IPathString, string IValue, string DataPath, string[] GenericM = default,
            string[] IMethodvalue = default,string[] MethodParams = default, string DataSeparator = ".")
        {
            if (!TryGetObject(out MonoBehaviour matchedObject, Class, IPathString, IValue, IMethodvalue,GenericM, DataSeparator))
            {
                Debug.LogError("Could Not Locate The Object With The Given Parameter");
                ReturnValue = null;
                return false;
            }
            if (!TryPopValue( out ReturnValue, matchedObject, DataPath, GenericM, MethodParams, DataSeparator))
            {
                Debug.LogError("Could Not Pop The Value");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try To Get THe Value Of THe Object While Walking Along The Path
        /// </summary>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Pop The Data From</param>
        /// <param name="ReturnValue">The Object Poped From The Path</param>
        /// <param name="MethodGiveParams">The Value To Give To Method While Poping Data From The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        /// <returns>Was The Task Successfull</returns>
        public bool TryPopValue(out object ReturnValue, string Class, string DataPath, string[] GenericM = default, string[] MethodGiveParams = default, string DataSeparator = ".")
        {
            
            if(!TryGetObject(Class, out MonoBehaviour single))
            {
                Debug.LogError("Could Not Accuire A Single Object To Get The Value");
                ReturnValue = null;
                return false;
            }
            if(!TryPopValue(out ReturnValue,single, DataPath,GenericM, MethodGiveParams, DataSeparator))
            {
                Debug.LogError("Could Not Pop The Value");
                return false;
            }
            return true;
        }


        #endregion

        #region Event

        /// <summary>
        /// Try To Call The Event
        /// </summary>
        /// <param name="obj">The Object In Which TO Search The Event</param>
        /// <param name="path">The Path TO Reach The Event</param>
        /// <param name="value">The Value TO Put In While Calling Event</param>
        /// <param name="MethodGivePrams">The Data TO Feed The Methods Along The Way</param>
        /// <returns>Was The TAsk Successful</returns>
        public bool TryCallEvent(object obj, Path path, string[] value, string[] MethodGivePrams = default)
        {
            if (!path.TravelPathLeavingDestination(ref obj, GetReadField(true), GetReadProperty(true), GetReadMethod(MethodGivePrams)))
            {
                Debug.LogError("Could Not Travel On The Path For THe Given Object");
                return false;
            }
            if (!path.ReachDestination(ref obj, GetEventTrack(value),GetFalseProperty(),getFalseMethod()))
            {
                Debug.LogError("Could Not Reach The Destination For The Given Object");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try To Call The Event
        /// </summary>
        /// <param name="single">The Object TO Search In</param>
        /// <param name="DataPath">The Path TO TAke</param>
        /// <param name="value">The Value TO Put In While Calling Event</param>
        /// <param name="GenericM">The Generics To Use Along THe Way</param>
        /// <param name="MethodGivePrams">The Data TO Feed The Methods Along The Way</param>
        /// <param name="DataSeparator">The Seperator</param>
        /// <returns>Was The TAsk Successful</returns>
        public bool TryCallEvent(MonoBehaviour single, string DataPath, string[] value, string[] GenericM = default, string[] MethodGivePrams = default, string DataSeparator = ".")
        {
            string[] Paths = DataPath.Split(DataSeparator);

            Path Path = new Path();
            if (!Path.BuildPath(single.GetType(), m_BindingFlags, Paths, GenericM))
            {
                Debug.LogError("Could Not Build PAth For The Given Identifier ");
                Debug.LogError($"Class=> {single.GetType()}");
                Debug.LogError($"Identifier Path =>{DataPath}");
                return false;
            }

            if (!TryCallEvent(single, Path, value, MethodGivePrams))
            {
                Debug.LogError("Could Not Call The Event For The Object");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try To Call The Event
        /// </summary>
        /// <param name="Class">The Class To Search In</param>
        /// <param name="DataPath">The Path TO TAke</param>
        /// <param name="Value">The Value TO Put In While Calling Event</param>
        /// <param name="GenericM">The Generics To Use Along THe Way</param>
        /// <param name="MethodGiveParams">The Data TO Feed The Methods Along The Way</param>
        /// <param name="DataSeparator">The Seperator</param>
        /// <returns>Was The TAsk Successful</returns>
        public bool TryCallEvent(string Class, string DataPath, string[] Value, string[] GenericM = default, string[] MethodGiveParams = default, string DataSeparator = ".")
        {
            if (!TryGetObject(Class, out MonoBehaviour single))
            {
                Debug.LogError("Could Not Accuire A Single Object To Get The Value");
                return false;
            }
            if (!TryCallEvent(single, DataPath, Value, GenericM, MethodGiveParams, DataSeparator))
            {
                Debug.LogError("Could Not CAll The Event For The Value");
                return false;
            }
            return true;
        }


        #endregion

        #region Print


        /// <summary>
        /// Print The Data Found At The End Of THe PAth
        /// </summary>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Pop The Data From</param>
        /// <param name="MethodGiveParams">The Value To Give To Method While Poping Data From The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        public void PrintValue(string Class, string DataPath, string[] GenericM = default, string[] MethodGiveParams = default, string DataSeparator = ".")
        {
            if(TryPopValue(out object ReturnValue, Class, DataPath,GenericM, MethodGiveParams,DataSeparator))
            {
                Debug.Log(ReturnValue);
            }
        }
        /// <summary>
        ///  Print The Data Found At The End Of THe PAth
        /// </summary>
        /// <param name="Class">The Class to Start the Path From (It Must Be A MonoBehaviour)</param>
        /// <param name="IPathString">The Path To Use To Find The Unique Object</param>
        /// <param name="IValue">The Unique Value Used TO Find The Object</param>
        /// <param name="DataPath">The Path Of Where We HAve TO Pop The Data From</param>
        /// <param name="IMethodvalue">The Values To Give The Method Which Finding The REquired Object</param>
        /// <param name="MethodParams">The Value To Give To Method While Poping Data From The Object</param>
        /// <param name="DataSeparator">The Seperator Used TO BreakDown The Path</param>
        public void PrintValue( string Class, string IPathString, string IValue, string DataPath, string[] GenericM = default,
            string[] IMethodvalue = default, string[] MethodParams = default, string DataSeparator = ".")
        {
            if (TryPopValue(out object ReturnValue, Class, IPathString,IValue, DataPath,GenericM,IMethodvalue, MethodParams, DataSeparator))
            {
                Debug.Log(ReturnValue);
            }
        }


        #endregion

        #endregion
    }
}