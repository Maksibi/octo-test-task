using System.Collections.Generic;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services
{
    //Регистрация где-то в DI
    public class PopUpFactory
    {
        private readonly Transform _root;
        private readonly IReadOnlyDictionary<PopUpType, PopUpBase> _prefabs;

        public PopUpFactory(Transform root, IReadOnlyDictionary<PopUpType, PopUpBase> prefabs)
        {
            _root = root;
            _prefabs = prefabs;
        }

        public PopUpBase Create(PopUpType popUpType)
        {
            if (!_prefabs.TryGetValue(popUpType, out PopUpBase prefab) || prefab == null)
                return null;

            PopUpBase instance = Object.Instantiate(prefab, _root);
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}