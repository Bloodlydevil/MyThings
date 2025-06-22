using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
public class RequiredFieldDrawer : PropertyDrawer
{
    Texture2D requiredIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/MyThings/Resources/Sprits/Red Warning.png");

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.BeginChangeCheck();

        Rect fieldrect = new Rect(position.x, position.y, position.width - 20, position.height);

        EditorGUI.PropertyField(fieldrect, property, label);

        if(IsFieldUnAssigned(property))
        {
            Rect iconrect = new Rect(position.xMax - 18, position.y, 16, 16);
            GUI.Label(iconrect, new GUIContent(requiredIcon,"This Field Is Required And Is Either Missing Or Empty"));
        }

        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(property.serializedObject.targetObject);

            EditorApplication.RepaintHierarchyWindow();
        }

        EditorGUI.EndProperty();
    }

    bool IsFieldUnAssigned(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.ObjectReference when property.objectReferenceValue:
            case SerializedPropertyType.ExposedReference when property.exposedReferenceValue:
            case SerializedPropertyType.AnimationCurve when property.animationCurveValue is { length: > 0 }:
            case SerializedPropertyType.String when !string.IsNullOrEmpty(property.stringValue):
                return false;
            default:
                return true;
        }
    }
}
