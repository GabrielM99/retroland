using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class CreateCharacterScreen : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private RectTransform _selectCharacterScreen;

        private TMP_InputField NameInput { get => _nameInput; }
        private RectTransform SelectCharacterScreen { get => _selectCharacterScreen; }

        public void Create()
        {
            Network.CreateCharacterProtocol.Execute(new CreateCharacterRequestMessage(NameInput.text), (response) =>
            {
                if (response.result)
                {
                    gameObject.SetActive(false);
                    SelectCharacterScreen.gameObject.SetActive(true);
                }
            });
        }
    }
}
