using UnityEngine;

using DoozyUI;

namespace Codari.TTT.UI
{
    [RequireComponent(typeof(UIButton))]
    internal abstract class UIButtonAction : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<UIButton>().OnClick.AddListener(Action);
        }

        protected abstract void Action();
    }
}
