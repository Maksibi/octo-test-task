using System;
using UnityEngine;

namespace CodeBase.RefactoredCode
{
    public class Character : MonoBehaviour
    {
        private float _someValue;

        public event Action SomeValueChanged;
        public event Action<Character> CharacterChanged;

        public float SomeValue
        {
            get => _someValue;
            set
            {
                if (Mathf.Approximately(_someValue, value))
                    return;

                _someValue = value;
                SomeValueChanged?.Invoke();
            }
        }

        private void OnDestroy()
        {
            CharacterChanged?.Invoke(this);
        }
    }
}