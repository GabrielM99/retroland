using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class LoginScreen : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _usernameInput;
        [SerializeField] private TMP_InputField _passwordInput;
        [SerializeField] private Button _loginButton;
        [SerializeField] private RectTransform _selectCharacterScreen;

        private TMP_InputField UsernameInput { get => _usernameInput; }
        private TMP_InputField PasswordInput { get => _passwordInput; }
        private Button LoginButton { get => _loginButton; }
        private RectTransform SelectCharacterScreen { get => _selectCharacterScreen; }

        private void Start()
        {
            UsernameInput.onValueChanged.AddListener((value) => UpdateLoginButton());
            PasswordInput.onValueChanged.AddListener((value) => UpdateLoginButton());
        }

        public void Login()
        {
            Network.Login(UsernameInput.text, PasswordInput.text, (response) =>
            {                
                if (response.result)
                {
                    gameObject.SetActive(false);
                    SelectCharacterScreen.gameObject.SetActive(true);
                }
            });
        }

        private void UpdateLoginButton()
        {
            LoginButton.interactable = !string.IsNullOrEmpty(UsernameInput.text) && !string.IsNullOrEmpty(PasswordInput.text);
        }
    }
}
