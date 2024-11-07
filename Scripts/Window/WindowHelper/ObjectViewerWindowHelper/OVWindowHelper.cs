using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace MyThings.Window.WindowHelper
{
    public class OVWindowHelper : MonoBehaviour, IWindowHelper
    {
        [SerializeField] private GameObject Content;
        [SerializeField] private Button FieldButton;
        [SerializeField] private Button PropertiesButton;
        [SerializeField] private RectTransform FieldsHolder;
        [SerializeField] private RectTransform PropertiesHolder;
        [SerializeField] private GameObject PrefabSingle;
        private List<OVWindowHHelper> Fields = new List<OVWindowHHelper>();
        private List<OVWindowHHelper> Properties = new List<OVWindowHHelper>();
        private bool FieldsShow = false;
        private bool PropertiesShow = false;
        private void SetUpFields(FieldInfo[] Fields, object obj)
        {
            FieldButton.interactable = Fields.Length != 0;
            FieldButton.onClick.AddListener(OnFieldClick);
            foreach (FieldInfo field in Fields)
            {
                var FieldGame = Instantiate(PrefabSingle, FieldsHolder).GetComponent<OVWindowHHelper>();
                this.Fields.Add(FieldGame);
                FieldGame.SetUp(field.FieldType.IsArray ? "" : field.Name, field.FieldType, field.GetValue(obj), FieldsHolder.gameObject, PrefabSingle);
                FieldGame.gameObject.SetActive(false);
            }
        }
        private void SetUpProperties(PropertyInfo[] properties, object obj)
        {
            PropertiesButton.interactable = properties.Length != 0;
            PropertiesButton.onClick.AddListener(OnProperitesClick);
            foreach (PropertyInfo property in properties)
            {
                var PropertyGame = Instantiate(PrefabSingle, PropertiesHolder).GetComponent<OVWindowHHelper>();
                try
                {
                    object value = property.GetValue(obj);
                    this.Properties.Add(PropertyGame);
                    PropertyGame.SetUp(property.PropertyType.IsArray ? "" : property.Name, property.PropertyType, value, PropertiesHolder.gameObject, PrefabSingle);
                    PropertyGame.gameObject.SetActive(false);
                }
                catch
                {
                    Destroy(PropertyGame.gameObject);
                }
            }
        }
        public void SetUp(object obj)
        {
            var Fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var Properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            SetUpFields(Fields, obj);
            SetUpProperties(Properties, obj);
        }
        private void OnFieldClick()
        {
            FieldsShow = !FieldsShow;
            if (FieldsShow)
            {
                Content.SetActive(false);
                foreach (var field in Fields)
                {
                    field.ChildShow();
                    field.gameObject.SetActive(true);
                }
                Content.SetActive(true);
            }
            else
            {
                Content.SetActive(false);
                foreach (var field in Fields)
                {
                    field.ChildForceHide();
                    field.gameObject.SetActive(false);
                }
                Content.SetActive(true);
            }
        }
        private void OnProperitesClick()
        {
            PropertiesShow = !PropertiesShow;
            if (PropertiesShow)
            {
                Content.SetActive(false);
                foreach (var property in Properties)
                {
                    property.ChildShow();
                    property.gameObject.SetActive(true);
                }
                Content.SetActive(true);
            }
            else
            {
                Content.SetActive(false);
                foreach (var property in Properties)
                {
                    property.ChildForceHide();
                    property.gameObject.SetActive(false);
                }
                Content.SetActive(true);
            }
        }
    }
}