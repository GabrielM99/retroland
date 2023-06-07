using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CharacterGraphics : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;

        public CharacterData data { get; private set; }
        public Action onToggle { get; set; }

        private TextMeshProUGUI nameText { get => _nameText; }
        private Toggle toggle { get; set; }

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        public void Set(CharacterData data, ToggleGroup group)
        {
            nameText.text = data.name;
            toggle.group = group;
            toggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    onToggle?.Invoke();
                }
            });
        }
    }
}
