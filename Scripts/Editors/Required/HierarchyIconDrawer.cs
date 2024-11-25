
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyIconDrawer
{
    static readonly Texture2D requiredIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MyThings/Resources/Sprits/Red Warning.png");
    static readonly Texture2D requiredIcon2 = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MyThings/Resources/Sprits/Red Warning 2.png");

    static Dictionary<Type, FieldInfo[]> cachedFieldinfo = new();

    static HierarchyIconDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGui;
    }

    static void OnHierarchyWindowItemOnGui(int instanceID, Rect selectionRect)
    {
        if (EditorUtility.InstanceIDToObject(instanceID) is not GameObject gameobject)
            return;
        Check(gameobject, selectionRect,0);
    }
    static bool Check(GameObject gameobject, Rect selectionRect,int depth)
    {
        foreach (var component in gameobject.GetComponents<Component>())
        {
            if (component == null) continue;
            var fields = GetCachedFieldWithRequiredAttributes(component.GetType());

            if (fields == null) continue;
            if (fields.Any(field => IsFieldUnassigned(field.GetValue(component))))
            {

                Rect iconrect = new Rect(selectionRect.xMax - 20, selectionRect.y, 16, 16);
                if (depth == 0)
                    GUI.Label(iconrect, new GUIContent(requiredIcon, $"One Or More Required Fields Are Missings or empty In {component} component"));
                else
                    GUI.Label(iconrect, new GUIContent(requiredIcon2, $"One Or More Required Fields Are Missings or empty In {component} component At Depth {depth}"));
                return true;
            }
        }
        for (int i = 0; i < gameobject.transform.childCount; i++)
        {
            if(Check(gameobject.transform.GetChild(i).gameObject, selectionRect, depth+1))
                return true;
        }
        return false;
    }
    static FieldInfo[] GetCachedFieldWithRequiredAttributes(Type type)
    {
        if (!cachedFieldinfo.TryGetValue(type,out FieldInfo[] fields))
        {
            fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            List<FieldInfo> list = new List<FieldInfo>();

            foreach (FieldInfo field in fields)
            {
                bool isSerialized=field.IsPublic|field.IsDefined(typeof(SerializeField),false);
                bool isRequired=field.IsDefined(typeof(RequiredFieldAttribute),false);

                if (isRequired && isSerialized)
                {
                    list.Add(field);
                }
            }
            fields = list.ToArray();
            cachedFieldinfo[type] = fields;
        }
        return fields;
    }
    static bool IsFieldUnassigned(object fieldValue)
    {
        if (fieldValue == null)
            return true ;

        if(fieldValue is string stringvalue && string.IsNullOrEmpty(stringvalue))
            return true ;

        if(fieldValue is System.Collections.IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item == null)
                    return true;
            }
        }

        return false;
    }
}