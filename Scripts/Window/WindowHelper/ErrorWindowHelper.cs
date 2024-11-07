using TMPro;
using UnityEngine;

namespace MyThings.Window.WindowHelper
{
    public class ErrorWindowHelper : MonoBehaviour, IWindowHelper
    {
        [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
    }
}