using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.MainMenu
{
    public class OpenPopUpButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private PopUpType _popUpType;

        private PopUpService _popUpService;

        [Inject]
        private void Construct(PopUpService popUpService)
        {
            _popUpService = popUpService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        private void OnButtonClick()
        {
            _popUpService?.OpenPopUp(_popUpType);
        }
        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}