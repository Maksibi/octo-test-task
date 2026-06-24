using System;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Entities
{
    public abstract class GameplayEntityBase : MonoBehaviour, IGameplayEntity
    {
        [SerializeField] private bool _startsActive = true;

        private GameplayEntityRegistry _registry;
        private bool _isGameplayActive;
        private bool _isRegistered;
        private bool _isInjected;

        public bool IsActive =>
            isActiveAndEnabled && _isGameplayActive;

        public event Action<IGameplayEntity> ActiveStateChanged;

        [Inject]
        private void Construct(GameplayEntityRegistry registry)
        {
            _registry = registry;
            _isInjected = true;

            if (isActiveAndEnabled)
            {
                _isGameplayActive = _startsActive;
                Register();
            }
        }

        protected virtual void OnEnable()
        {
            if (!_isInjected || _registry == null)
                return;

            _isGameplayActive = _startsActive;
            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        protected virtual void OnDestroy()
        {
            Unregister();
        }

        private void Register()
        {
            if (_isRegistered || _registry == null)
                return;

            _registry.Register(this);
            _isRegistered = true;
            NotifyActiveStateChanged();
        }

        private void Unregister()
        {
            if (!_isRegistered || _registry == null)
                return;

            _registry.Unregister(this);
            _isRegistered = false;
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