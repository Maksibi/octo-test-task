using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public abstract class PopUpBase : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private PopUpType _popUpType;

        public PopUpType PopUpType => _popUpType;

        protected virtual void OnEnable() => 
            _closeButton.onClick.AddListener(Close);

        protected virtual void OnDisable() => 
            _closeButton.onClick.RemoveListener(Close);

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }

    public enum PopUpType
    {
        None = 0,
        Setting = 1,
        Menu = 2,
        SaveLoad = 3,
        Dialog = 4,
    }
}