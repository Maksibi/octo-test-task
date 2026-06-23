using System;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay.Entities
{
    public abstract class GameplayEntityBase : MonoBehaviour, IGameplayEntity
    {
        [SerializeField] private bool _startsActive = true;

        private bool _isGameplayActive;

        public bool IsActive => 
            isActiveAndEnabled && _isGameplayActive;

        public event Action<IGameplayEntity> ActiveStateChanged;

        protected virtual void OnEnable()
        {
            _isGameplayActive = _startsActive;
            GameplayEntityRegistry.Instance.Register(this);
            NotifyActiveStateChanged();
        }

        protected virtual void OnDisable()
        {
            GameplayEntityRegistry.Instance.Unregister(this);
        }

        protected virtual void OnDestroy()
        {
            GameplayEntityRegistry.Instance.Unregister(this);
        }

        public void SetGameplayActive(bool isActive)
        {
            if (_isGameplayActive == isActive)
                return;

            _isGameplayActive = isActive;
            NotifyActiveStateChanged();
        }

        public void Complete()
        {
            SetGameplayActive(false);
        }

        private void NotifyActiveStateChanged()
        {
            ActiveStateChanged?.Invoke(this);
        }
    }
}