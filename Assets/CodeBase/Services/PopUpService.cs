using System.Collections.Generic;
using System.Linq;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Services
{
    public class PopUpService
    {
        private readonly PopUpFactory _factory;
        private readonly Dictionary<PopUpType, PopUpBase> _activePopUps = new();
        // Использую статику, чтобы не добавлять в проект DI
        public static PopUpService Instance { get; private set; }

        public PopUpService(PopUpFactory factory)
        {
            _factory = factory;
            Instance = this;
        }

        public void OpenPopUp(PopUpType popUpType)
        {
            PopUpBase popUp = GetOrCreate(popUpType);
            if (popUp == null)
                return;

            popUp.Open();
        }
        
        public TextPopUp OpenDialog(PopUpDialogData data)
        {
            if (data == null)
            {
                Debug.LogError("PopUpDialogData is null.");
                return null;
            }

            if (data.Buttons == null || data.Buttons.Length < 1 || data.Buttons.Length > 5)
            {
                Debug.LogError("Dialog must have from 1 to 5 buttons.");
                return null;
            }

            PopUpBase popUp = GetOrCreate(PopUpType.Dialog);
            if (popUp is not TextPopUp textPopUp)
            {
                Debug.LogError("PopUpType.Dialog must use TextPopUp prefab.");
                return null;
            }

            textPopUp.Setup(data);
            textPopUp.Open();
            return textPopUp;
        }

        private PopUpBase GetOrCreate(PopUpType popUpType)
        {
            PurgeDestroyedPopUps();

            if (_activePopUps.TryGetValue(popUpType, out PopUpBase existingPopUp))
                return existingPopUp;

            PopUpBase newPopUp = _factory.Create(popUpType);
            if (newPopUp == null)
                return null;

            _activePopUps[popUpType] = newPopUp;
            return newPopUp;
        }

        public void ClosePopUp(PopUpType popUpType)
        {
            PurgeDestroyedPopUps();

            if (_activePopUps.TryGetValue(popUpType, out PopUpBase popUp))
                popUp.Close();
        }

        public void CleanUpAllPopUp() => _activePopUps.Clear();

        public void CloseAllPopUps()
        {
            PurgeDestroyedPopUps();
            foreach (PopUpBase popUp in _activePopUps.Values.ToArray())
                popUp.Close();
        }

        public bool IsPopUpOpen(PopUpType popUpType)
        {
            PurgeDestroyedPopUps();
            return _activePopUps.TryGetValue(popUpType, out PopUpBase popUp) && popUp.gameObject.activeSelf;
        }

        private void PurgeDestroyedPopUps()
        {
            var staleTypes = new List<PopUpType>();

            foreach (var pair in _activePopUps)
            {
                if (pair.Value == null)
                    staleTypes.Add(pair.Key);
            }

            foreach (PopUpType popUpType in staleTypes)
                _activePopUps.Remove(popUpType);
        }
    }
}