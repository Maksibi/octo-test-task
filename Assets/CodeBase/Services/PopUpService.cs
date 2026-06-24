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

        public void CleanUpAllPopUp() => 
            _activePopUps.Clear();

        public PopUpService(PopUpFactory factory)
        {
            _factory = factory;
        }

        public void OpenPopUp(PopUpType popUpType)
        {
            PopUpBase popUp = GetOrCreate(popUpType);

            popUp.Open();
        }
        
        public TextPopUp OpenDialog(PopUpDialogData data)
        {
            PopUpBase popUp = GetOrCreate(PopUpType.Dialog);
            
            if (popUp is not TextPopUp textPopUp)
                return null;

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

            _activePopUps[popUpType] = newPopUp;
            return newPopUp;
        }

        public void ClosePopUp(PopUpType popUpType)
        {
            PurgeDestroyedPopUps();

            if (_activePopUps.TryGetValue(popUpType, out PopUpBase popUp))
                popUp.Close();
        }

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