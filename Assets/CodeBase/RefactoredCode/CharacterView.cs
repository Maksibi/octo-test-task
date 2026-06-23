using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CodeBase.RefactoredCode
{
    public class CharactersView : MonoBehaviour
    {
        //Я бы вообще убрал обновления по кадрам. Для UI в большинстве случаев достаточно событий.
        // Для разработки это тоже проще
        
        [SerializeField] private List<Transform> _characterTransforms;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private int _updateEveryNFrames = 1;

        private readonly List<Character> _characters = new();
        private bool _refreshScheduled;
        private int _framesUntilRefresh;

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

        private void Update()
        {
            if (!_refreshScheduled)
                return;

            _framesUntilRefresh--;
            if (_framesUntilRefresh > 0)
                return;

            _refreshScheduled = false;
            RefreshUI();
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
            character.SomeValueChanged += ScheduleRefresh;
        }

        private void Unsubscribe(Character character)
        {
            character.CharacterChanged -= OnCharacterChanged;
            character.SomeValueChanged -= ScheduleRefresh;
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
            ScheduleRefresh();
        }

        private void ScheduleRefresh()
        {
            _refreshScheduled = true;
            _framesUntilRefresh = _updateEveryNFrames;
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
            if (_label == null)
                return;

            float totalValue = 0f;
            int count = 0;

            for (int i = _characters.Count - 1; i >= 0; i--)
            {
                Character character = _characters[i];
                if (character == null)
                {
                    _characters.RemoveAt(i);
                    continue;
                }

                totalValue += character.SomeValue;
                count++;
            }

            float average = count > 0 ? totalValue / count : 0f;
            _label.text = $"Characters: {count}  Avg value: {average:F2}";
        }
    }
}