using MyThings.Window.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyThings.Window.WindowHelper
{

    public class OVWindowHHelper : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ValueName;
        [SerializeField] private TextMeshProUGUI Value;
        [SerializeField] private TextMeshProUGUI Type;
        [SerializeField] private Button ValButton;
        private List<GameObject> Childs = new List<GameObject>();
        private GameObject Parent;
        private bool ShowHide = false;
        public void SetUp(string Name, Type type, object val, GameObject parent, GameObject Prefab)
        {
            ValueName.text = Name;
            Type.text = type.ToString();
            Parent = parent;
            if (val == null)
            {
                Value.text = "NULL";
                ValButton.interactable = false;
                return;
            }
            Value.text = val.ToString();
            if (type.IsArray)
            {
                Array ar = val as Array;
                if (ar.Length == 0)
                {
                    ValButton.interactable = false;
                    return;
                }
                foreach (object SingleObject in (Array)val)
                {
                    if (SingleObject == null) continue;
                    var obj = Instantiate(Prefab, Parent.transform).GetComponent<OVWindowHHelper>();
                    Childs.Add(obj.gameObject);
                    obj.SetUp(Name + "-", SingleObject.GetType(), SingleObject, gameObject, Prefab);
                    obj.gameObject.SetActive(false);
                }
                ValButton.onClick.AddListener(OnClick);
            }
            else if (val is IEnumerable)
            {
                var temp = val as ICollection;
                if (temp == null || temp.Count == 0)
                {
                    ValButton.interactable = false;
                    return;
                }

                ValButton.interactable = true;
                var Collec = val as IEnumerable;
                foreach (object SingleObject in Collec)
                {
                    if (SingleObject == null) continue;
                    var obj = Instantiate(Prefab, Parent.transform).GetComponent<OVWindowHHelper>();
                    Childs.Add(obj.gameObject);
                    obj.SetUp(Name + "-", SingleObject.GetType(), SingleObject, gameObject, Prefab);
                    obj.gameObject.SetActive(false);
                }
                ValButton.onClick.AddListener(OnClick);
            }
            else
            {
                if (type.IsPrimitive)
                {
                    ValButton.interactable = false;
                }
                else
                {
                    ValButton.interactable = true;
                    ValButton.onClick.AddListener(() => { WindowObjectViewer.Create(val); });
                }
            }
        }
        private void OnClick()
        {
            ShowHide = !ShowHide;
            ChildShow();
            if (!ShowHide)
            {
                ChildForceHide();
            }
        }
        public void ChildShow()
        {
            if (ShowHide)
            {
                Parent.SetActive(false);
                foreach (var child in Childs)
                {
                    child.SetActive(true);
                }
                Parent.SetActive(true);
            }
        }
        public void ChildForceHide()
        {
            Parent.SetActive(false);
            foreach (var child in Childs)
            {
                child.SetActive(false);
            }
            Parent.SetActive(true);
        }
    }
}