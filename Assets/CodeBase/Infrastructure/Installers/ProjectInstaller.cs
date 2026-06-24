using System;
using System.Collections.Generic;
using CodeBase.SaveService;
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    [Serializable]
    public class PopUpPrefabEntry
    {
        public PopUpType Type;
        public PopUpBase Prefab;
    }

    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private Transform _popUpRoot;
        [SerializeField] private PopUpPrefabEntry[] _popUpPrefabs;

        public override void InstallBindings()
        {
            Container.Bind<SaveSystem>().AsSingle();
            Container.Bind<SaveScreenshotMaker>().AsSingle();
            Container.Bind<GameplayEntityRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();

            if (_popUpRoot == null)
                _popUpRoot = CreatePopUpRoot();

            IReadOnlyDictionary<PopUpType, PopUpBase> prefabs = BuildPrefabDictionary();

            Container.Bind<PopUpFactory>().AsSingle().WithArguments(_popUpRoot, prefabs);
            Container.Bind<PopUpService>().AsSingle();
        }

        private Transform CreatePopUpRoot()
        {
            var rootObject = new GameObject("PopUpRoot");
            rootObject.transform.SetParent(transform, false);
            return rootObject.transform;
        }

        private IReadOnlyDictionary<PopUpType, PopUpBase> BuildPrefabDictionary()
        {
            var prefabs = new Dictionary<PopUpType, PopUpBase>();

            if (_popUpPrefabs == null)
                return prefabs;

            foreach (PopUpPrefabEntry entry in _popUpPrefabs)
            {
                if (entry == null || entry.Prefab == null || entry.Type == PopUpType.None)
                    continue;

                prefabs[entry.Type] = entry.Prefab;
            }

            return prefabs;
        }
    }
}