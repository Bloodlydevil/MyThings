using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using MyThings.ExtendableClass;
using System.Linq;

namespace MyThings.P_Pattern
{
    /// <summary>
    /// Inject An Attribute To The Class By The Provider
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method|AttributeTargets.Property)]// corently allows for field and methods
    public sealed class InjectAttribute : Attribute { }
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Property)]
    /// <summary>
    /// Provide The Attribute To Others Witing For It By The Use Of Inject(Must Impliment IDependencyProvider to be visible)
    /// </summary>
    public sealed class ProvideAttribute : Attribute 
    {
        public int Depth;

        public ProvideAttribute(int Depth=1) 
        {
            this.Depth = Depth;
        } 
    }



    /// <summary>
    /// The Provider
    /// </summary>
    public interface IDependencyProvider 
    {
        /// <summary>
        /// Should It Provide now or later on?
        /// </summary>
        public virtual bool PreProvide => true; 
    }
    /// <summary>
    /// The Class In Which TO Inject
    /// </summary>
    public interface IDependencyInjector 
    { 
        /// <summary>
        /// Should It Be Reinjected If Needs Arise?
        /// </summary>
        public virtual bool ReInjectAllowed => false;
        /// <summary>
        /// Should It Be Pre Provided Or Provided Later On
        /// </summary>
        public virtual bool PreProvide => true; 
    }




    /// <summary>
    /// A Single Injector to inject to
    /// </summary>
    public class InjectorSingle
    {
        /// <summary>
        /// The Fields To Inject To
        /// </summary>
        public IEnumerable<FieldInfo> Fields;
        /// <summary>
        /// The Methods TO Inject To
        /// </summary>
        public IEnumerable<MethodInfo> Methods;
        /// <summary>
        /// The Properties To Inject To
        /// </summary>
        public IEnumerable<PropertyInfo> Properties;
        /// <summary>
        /// Is THe Injection Satisfied
        /// </summary>
        public bool InjectionSatisfied;
        /// <summary>
        /// The Object To Inject TO
        /// </summary>
        public IDependencyInjector InjectorObject;
    }




    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton_C<Injector>
    {
        protected override bool AddToDontDestroyOnLoad => false;

        /// <summary>
        /// The Things To Allow For while finding
        /// </summary>
        const BindingFlags m_flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// The Providers That Are Providing
        /// </summary>
        readonly Dictionary<Type,object> registoryProviders = new Dictionary<Type,object>();

        /// <summary>
        /// The Injectors TO Inject
        /// </summary>
        readonly List<InjectorSingle> registoryInjectors = new List<InjectorSingle>();

        protected override void Awake()
        {
            base.Awake();



            var providers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None).OfType<IDependencyProvider>();
            foreach (var provider in providers)
            {
                
                if (provider.PreProvide)
                    RegistorProvider(provider);
            }
            var Injectables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None).OfType<IDependencyInjector>();
            foreach(var injectable in Injectables)
            {
                if (injectable.PreProvide)
                    Inject(injectable);
            }
        }


        /// <summary>
        /// Inject The Value TO The Instance
        /// </summary>
        /// <param name="instance">The Instance</param>
        /// <exception cref="Exception">Could Not Inject</exception>
        private bool Inject(InjectorSingle instance)
        {
            bool Successfull = true;
            
            foreach(var injectableField in instance.Fields)
            {
                var fieldtype = injectableField.FieldType;
                var resolved = Resolve(fieldtype);
                if(resolved==null)
                {
                    Debug.Log($" Failed To Inject {fieldtype.Name} into {instance.InjectorObject.GetType()} ");
                    Successfull = false;
                    continue;
                }
                injectableField.SetValue(instance.InjectorObject, resolved);
            }
            
            foreach (var injectableMethod in instance.Methods)
            {
                var requiredParameters=injectableMethod.GetParameters().Select(parameter=>parameter.ParameterType).ToArray();
                var resolvedInstance=requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstance.Any(resolvedInstance => resolvedInstance == null))
                {
                    Debug.Log($"Failed To Inject  {instance.InjectorObject.GetType()}  {injectableMethod.Name}");
                    Successfull=false;
                    continue;
                }
                injectableMethod.Invoke(instance.InjectorObject, resolvedInstance);
            }
            foreach (var injectableProperties in instance.Properties)
            {
                if(!injectableProperties.CanWrite)
                {
                    Debug.LogError($"Can Not Inject To {injectableProperties.Name} No Setter Found");
                    Successfull = false;
                    continue;
                }
                var propertytype=injectableProperties.PropertyType;
                var resolved = Resolve(propertytype);
                if( resolved==null)
                {
                    Debug.Log($" Failed To Inject {propertytype.Name} into {instance.InjectorObject.GetType()} ");
                    Successfull = false;
                    continue;
                }
                injectableProperties.SetValue(instance.InjectorObject, resolved);
            }
            return Successfull;
        }
        /// <summary>
        /// Resolve The Directory to get value
        /// </summary>
        /// <param name="resolveType">The Type For Which TO Get</param>
        /// <returns>The Value To Inject</returns>
        private object Resolve(Type resolveType)
        {
            registoryProviders.TryGetValue(resolveType, out var ans);
            return ans;
        }
        /// <summary>
        /// If it is Injectable
        /// </summary>
        /// <param name="obj">THe Class To Change</param>
        /// <returns>Is Injectable</returns>
        public static bool IsInjectable(MonoBehaviour obj)
        {
            var members=obj.GetType().GetMembers(m_flags);
            return members.Any(members => Attribute.IsDefined(members, typeof(ProvideAttribute)));
        }
        /// <summary>
        /// Registor The Provider (only one should provide)
        /// </summary>
        /// <param name="provider">The Provider</param>
        /// <exception cref="Exception">Provider Returned Null</exception>
        public void RegistorProvider(IDependencyProvider provider)
        {
            var methods=provider.GetType().GetMethods(m_flags);
            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                var provideAttribute= (ProvideAttribute)Attribute.GetCustomAttribute(method, typeof(ProvideAttribute));
                int depth = provideAttribute.Depth;
                var returntype = method.ReturnType;
                while (depth-->0)// registor the elements till the depth given
                {
                    var provideInstance = method.Invoke(provider, null);
                    if (returntype != null)
                    {
                        if (registoryProviders.ContainsKey(returntype))
                        {
                            registoryProviders[returntype] = provideInstance;
                            Debug.Log($"The Provider {provider.GetType().Name}  Changed The Value For {returntype}");
                        }
                        else
                        {
                            registoryProviders.Add(returntype, provideInstance);
                        }
                    }
                    else
                        throw new Exception($"Provider {provider.GetType().Name} returned null for {returntype.Name} ");
                    returntype=returntype.BaseType;
                }
            }
            var Properties=provider.GetType().GetProperties(m_flags);
            foreach(var property in Properties)
            {
                if (!Attribute.IsDefined(property, typeof(ProvideAttribute))) continue;
                var provideAttribute = (ProvideAttribute)Attribute.GetCustomAttribute(property, typeof(ProvideAttribute));
                int depth = provideAttribute.Depth;
                var returntype=property.PropertyType;
                var provideInstance= property.GetValue(provider, null);
                if(!property.CanRead)
                {
                    Debug.LogError($"The Property {property.Name} Does Not Have Getter");
                    break;
                }
                while (depth-- > 0)// registor the elements till the depth given
                {
                    if (returntype != null)
                    {
                        if (registoryProviders.ContainsKey(returntype))
                        {
                            registoryProviders[returntype] = provideInstance;
                            Debug.Log($"The Provider {provider.GetType().Name}  Changed The Value For {returntype}");
                        }
                        else
                        {
                            registoryProviders.Add(returntype, provideInstance);
                        }
                    }
                    else
                        throw new Exception($"Provider {provider.GetType().Name} returned null for {returntype.Name} ");
                    returntype = returntype.BaseType;
                }
            }
        }
        /// <summary>
        /// REinject all , usualy done when a provider takes time to provide
        /// </summary>
        public void ReInject()
        {
            for(int i=0;i<registoryInjectors.Count;i++)
            {
                if (!registoryInjectors[i].InjectionSatisfied)
                {
                    bool Satisfied= Inject(registoryInjectors[i]);
                    if(Satisfied&& !registoryInjectors[i].InjectorObject.ReInjectAllowed)
                    {
                        registoryInjectors[i].InjectionSatisfied = true;
                        registoryInjectors.RemoveAt(i--);
                    }
                }
            }
        }
        /// <summary>
        /// Inject The Value In It
        /// </summary>
        /// <param name="injector">The Injector</param>
        public void Inject(IDependencyInjector injector)
        {
            var type = injector.GetType();
            var injectableFields = type.GetFields(m_flags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            var injectableMethods = type.GetMethods(m_flags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            var injectableProperties = type.GetProperties(m_flags).Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            var InjectorSingle = new InjectorSingle 
            {
                Fields = injectableFields,
                Methods = injectableMethods ,
                Properties= injectableProperties,
                InjectionSatisfied =false,
                InjectorObject=injector
            };
            if (Inject(InjectorSingle) && !injector.ReInjectAllowed)
                InjectorSingle.InjectionSatisfied = true;// now i can registor  it not if performance decreases
            else
                registoryInjectors.Add(InjectorSingle);
        }
    }
}