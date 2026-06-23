using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.MainMenu
{
    public class OpenPopUpButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private PopUpType _popUpType;
        private void OnEnable()
        {
            if (_button == null)
                return;
            _button.onClick.AddListener(OnButtonClick);
        }
        private void OnButtonClick()
        {
            PopUpService.Instance?.OpenPopUp(_popUpType);
        }
        private void OnDisable()
        {
            if (_button == null)
                return;
            _button.onClick.RemoveListener(OnButtonClick);
        }
        private void OnValidate()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }
    }
}