using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MyThings.Window.WindowHelper
{
    public class PromptWindowHelper : MonoBehaviour,IWindowHelper
    {
        [field: SerializeField] public TextMeshProUGUI TextArea { get; private set; }
        [field: SerializeField] public Button Close { get; private set; }
        [field: SerializeField] public Button Accept { get; private set; }
    }
}