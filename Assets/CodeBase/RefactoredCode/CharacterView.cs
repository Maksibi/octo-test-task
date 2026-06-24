using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CodeBase.RefactoredCode
{
    public class CharactersView : MonoBehaviour
    {
        //Я убрал обновления по кадрам совсем. Это лишнее при работе с UI в данном случае.
        
        [SerializeField] private List<Transform> _characterTransforms;
        [SerializeField] private TMP_Text _label;

        private readonly List<Character> _characters = new();

        private void OnEnable()
        {
            ResubscribeAll();
            RefreshUI();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
            _characters.Clear();
        }

        private void ResubscribeAll()
        {
            UnsubscribeAll();
            RebuildCharacterCache();

            for (int i = 0; i < _characters.Count; i++)
                Subscribe(_characters[i]);
        }

        private void Subscribe(Character character)
        {
            character.CharacterChanged += OnCharacterChanged;
            character.SomeValueChanged += RefreshUI;
        }

        private void Unsubscribe(Character character)
        {
            character.CharacterChanged -= OnCharacterChanged;
            character.SomeValueChanged -= RefreshUI;
        }

        private void UnsubscribeAll()
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i] != null)
                    Unsubscribe(_characters[i]);
            }
        }

        private void OnCharacterChanged(Character _)
        {
            ResubscribeAll();
        }

        private void RebuildCharacterCache()
        {
            _characters.Clear();

            foreach (Transform t in _characterTransforms)
            {
                if (t == null)
                    continue;

                Character character = t.GetComponent<Character>();
                if (character != null)
                    _characters.Add(character);
            }
        }

        private void RefreshUI()
        {
            float totalValue = 0f;
            int count = 0;

            for (int i = _characters.Count - 1; i >= 0; i--)
            {
                Character character = _characters[i];

                totalValue += character.SomeValue;
                count++;
            }

            float average = count > 0 ? totalValue / count : 0f;
            _label.text = $"Characters: {count}  Avg value: {average:F2}";
        }
    }
}