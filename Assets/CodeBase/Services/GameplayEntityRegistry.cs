using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Entities;

namespace CodeBase.Services
{
    public class GameplayEntityRegistry
    {
        //Статика вместо DI
        public static GameplayEntityRegistry Instance { get; } = new();

        private readonly HashSet<IGameplayEntity> _all = new();
        private readonly List<IGameplayEntity> _activeBuffer = new();
        private bool _activeCacheDirty = true;

        public event Action RegistryChanged;

        public void Register(IGameplayEntity entity)
        {
            if (entity == null || !_all.Add(entity))
                return;

            entity.ActiveStateChanged += OnEntityActiveStateChanged;
            MarkDirty();
        }

        public void Unregister(IGameplayEntity entity)
        {
            if (entity == null || !_all.Remove(entity))
                return;

            entity.ActiveStateChanged -= OnEntityActiveStateChanged;
            MarkDirty();
        }

        public IReadOnlyList<IGameplayEntity> GetActiveEntities()
        {
            if (_activeCacheDirty)
                RebuildActiveCache();

            return _activeBuffer;
        }

        public void ForEachActive(Action<IGameplayEntity> action)
        {
            IReadOnlyList<IGameplayEntity> active = GetActiveEntities();

            for (int i = 0; i < active.Count; i++)
                action(active[i]);
        }

        private void RebuildActiveCache()
        {
            _activeBuffer.Clear();

            foreach (IGameplayEntity entity in _all)
            {
                if (IsEntityValidAndActive(entity))
                    _activeBuffer.Add(entity);
            }

            _activeCacheDirty = false;
        }

        private static bool IsEntityValidAndActive(IGameplayEntity entity)
        {
            if (entity is UnityEngine.Object unityObject && unityObject == null)
                return false;

            return entity.IsActive;
        }

        private void OnEntityActiveStateChanged(IGameplayEntity _)
        {
            MarkDirty();
        }

        private void MarkDirty()
        {
            _activeCacheDirty = true;
            RegistryChanged?.Invoke();
        }
    }
}