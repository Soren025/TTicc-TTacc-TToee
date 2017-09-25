using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using DoozyUI;
using Sirenix.OdinInspector;

namespace Codari.TTT
{
    public sealed class ProfileMenuUI : MonoBehaviour
    {
        [SerializeField]
        private Text idDisplay;

        [SerializeField, BoxGroup("Name")]
        private Text nameDisplay;
        [SerializeField, BoxGroup("Name")]
        private UIButton changeNameButton;
        [SerializeField, BoxGroup("Name")]
        private InputField nameInput;

        private void RefreshProfileInfo()
        {
            nameDisplay.text = TTTProfile.Local.Name;
        }

        #region Unity Callbacks

        void Awake()
        {
            TTTProfile.LoadProfile();
            idDisplay.text = "id: " + TTTProfile.Local.Id;

            changeNameButton.OnClick.AddListener(UIListener_OnChangeNameClick);
            nameInput.onEndEdit.AddListener(UIListener_OnNameInputSubmit);

            // Hide this if it is not already hidden
            nameInput.gameObject.SetActive(false);
        }

        void Start()
        {
            RefreshProfileInfo();
        }

        #endregion

        #region UI Listeners

        void UIListener_OnChangeNameClick()
        {
            nameInput.text = TTTProfile.Local.Name;

            changeNameButton.Interactable = false;
            nameDisplay.gameObject.SetActive(false);
            nameInput.gameObject.SetActive(true);

            EventSystem.current.SetSelectedGameObject(nameInput.gameObject);
        }

        void UIListener_OnNameInputSubmit(string newName)
        {
            if (newName != TTTProfile.Local.Name)
            {
                TTTProfile.Local.Name = newName;
                TTTProfile.SaveProfile();

                RefreshProfileInfo();
            }

            changeNameButton.Interactable = true;
            nameDisplay.gameObject.SetActive(true);
            nameInput.gameObject.SetActive(false);
        }

        #endregion
    }
}
