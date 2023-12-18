using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class RegisterScreen : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private TMP_InputField _confirmPasswordInput;
        [SerializeField] private Button _registerButton;
        [SerializeField] private RectTransform _loginScreen;

        private TMP_InputField UsernameInput { get => _usernameInput; }
        private TMP_InputField PasswordInput { get => _passwordInput; }
        private TMP_InputField ConfirmPasswordInput { get => _confirmPasswordInput; }
        private Button RegisterButton { get => _registerButton; }
        private RectTransform LoginScreen { get => _loginScreen; }

        private void Start()
        {
            UsernameInput.onValueChanged.AddListener((value) => UpdateRegisterButton());
            PasswordInput.onValueChanged.AddListener((value) => UpdateRegisterButton());
            ConfirmPasswordInput.onValueChanged.AddListener((value) => UpdateRegisterButton());
        }

        public void Register()
        {
            Network.Register(UsernameInput.text, PasswordInput.text, (response) =>
            {
                if (response.result)
                {
                    gameObject.SetActive(false);
                    LoginScreen.gameObject.SetActive(true);
                }
            });
        }

        private void UpdateRegisterButton()
        {
            RegisterButton.interactable = !string.IsNullOrEmpty(UsernameInput.text) && !string.IsNullOrEmpty(PasswordInput.text) && PasswordInput.text == ConfirmPasswordInput.text;
        }
    }
}
