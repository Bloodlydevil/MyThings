using System;
using UnityEngine;

namespace MyThings.Events
{
    public class ComponentEvent : MonoBehaviour
    {
        private Action OnDestroyEvent;
        private Action OnEnableEvent;
        private Action OnDisableEvent;
        private Action OnUpdateEvent;

        private void OnDestroy() => OnDestroyEvent?.Invoke();

        private void OnEnable()=> OnEnableEvent?.Invoke();

        private void OnDisable() => OnDisableEvent?.Invoke();

        private void Update() => OnUpdateEvent?.Invoke();


        public static void CreateComponent(Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null)
        {
            GameObject gameObject = new GameObject("ComponentEvent");
            AddComponent(gameObject, OnDestroyFunc, OnEnableFunc, OnDisableFunc, OnUpdate);
        }

        public static void AddComponent(GameObject gameObject, Action OnDestroyFunc = null, Action OnEnableFunc = null, Action OnDisableFunc = null, Action OnUpdate = null)
        {
            ComponentEvent componentFuncs = gameObject.AddComponent<ComponentEvent>();
            componentFuncs.OnDestroyEvent = OnDestroyFunc;
            componentFuncs.OnEnableEvent = OnEnableFunc;
            componentFuncs.OnDisableEvent = OnDisableFunc;
            componentFuncs.OnUpdateEvent = OnUpdate;
        }
    }
}