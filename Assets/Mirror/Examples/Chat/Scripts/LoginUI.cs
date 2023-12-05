using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mirror.Examples.Chat
{
    public class LoginUI : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] internal TMP_InputField usernameInput;
        [SerializeField] internal Button hostButton;
        [SerializeField] internal Button clientButton;
        [SerializeField] internal Text errorText;

        public static LoginUI instance;

        void Awake()
        {
            instance = this;
        }

        // Called by UI element UsernameInput.OnValueChanged
        public void ToggleButtons(string username)
        {
            hostButton.interactable = !string.IsNullOrWhiteSpace(username);
            clientButton.interactable = !string.IsNullOrWhiteSpace(username);
        }
    }
}
