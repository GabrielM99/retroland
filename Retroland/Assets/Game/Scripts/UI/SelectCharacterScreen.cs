using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI
{
    public class SelectCharacterScreen : MonoBehaviour
    {
        [SerializeField] private CharacterGraphics _characterGraphicsPrefab;
        [SerializeField] private ToggleGroup _charactersParent;
        [SerializeField] private Button _selectButton;

        private CharacterGraphics CharacterGraphicsPrefab { get => _characterGraphicsPrefab; }
        private ToggleGroup CharactersParent { get => _charactersParent; }
        private Button SelectButton { get => _selectButton; }
        private CharacterData SelectedCharacterData { get; set; }

        private void OnEnable()
        {
            LoadCharacters();
        }

        public void Select()
        {
            Network.PlayProtocol.Execute(new PlayRequestMessage(SelectedCharacterData.id), (response) =>
            {
                if (response.result)
                {
                    Network.LoadPlayScene();
                }
            });
        }

        private void LoadCharacters()
        {
            ClearCharacterGraphics();

            Network.GetCharactersProtocol.Execute(new GetCharactersRequestMessage(), (response) =>
            {
                foreach (CharacterData character in response.characterData)
                {
                    CreateCharacterGraphics(character);
                }

                CharactersParent.EnsureValidState();
            });
        }

        private void ClearCharacterGraphics()
        {
            SelectButton.interactable = false;

            foreach (Transform characterGraphics in CharactersParent.transform)
            {
                Destroy(characterGraphics.gameObject);
            }
        }

        private void CreateCharacterGraphics(CharacterData character)
        {
            CharacterGraphics characterGraphics = Instantiate(CharacterGraphicsPrefab, CharactersParent.transform);
            characterGraphics.Set(character, CharactersParent);
            characterGraphics.onToggle += () =>
            {
                SelectButton.interactable = true;
                SelectedCharacterData = character;
            };
        }
    }
}
