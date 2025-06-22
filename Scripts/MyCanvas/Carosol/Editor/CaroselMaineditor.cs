using UnityEditor;
using UnityEngine;

namespace MyThings.MyCanvas.Carousel
{
    [CustomEditor(typeof(CarouselMain))]
    public class CaroselMaineditor : Editor
    {
        private void OnEnable()
        {
            CarouselMain car = (CarouselMain)target;
            car.CreateElement ??= (prefab, parent) => PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
        }
        public override void OnInspectorGUI()
        {
            CarouselMain car = (CarouselMain)target;
            DrawDefaultInspector();
            if (GUILayout.Button("Add New Item"))
            {
                car.AddNewItem();
            }
            if (GUILayout.Button("Remove Last Item"))
            {
                car.RemoveLastItem();
            }
            if (GUILayout.Button("Clear All Items"))
            {
                car.ClearAllItems();
            }
        }
    }
}