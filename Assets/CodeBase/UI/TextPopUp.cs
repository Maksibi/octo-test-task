using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class TextPopUp : PopUpBase
    {
        [Serializable]
        private struct ButtonSlot
        {
            public Button Button;
            public TMP_Text Label;
        }

        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _content;
        [SerializeField] private ButtonSlot[] _buttonSlots = new ButtonSlot[5];

        public void Setup(PopUpDialogData data)
        {
            if (data.Buttons == null || data.Buttons.Length < 1 || data.Buttons.Length > 5)
                return;

            _header.text = data.Title ?? string.Empty; // пока не уверен в локализации при необходимости
            _content.text = data.Body ?? string.Empty;

            ClearButtonListeners();

            for (int i = 0; i < _buttonSlots.Length; i++)
            {
                bool isActive = i < data.Buttons.Length;
                ButtonSlot slot = _buttonSlots[i];

                if (slot.Button == null)
                    continue;

                slot.Button.gameObject.SetActive(isActive);

                if (!isActive)
                    continue;

                PopUpButtonOption option = data.Buttons[i];

                if (slot.Label != null)
                    slot.Label.text = option.Label ?? string.Empty;

                slot.Button.onClick.AddListener(() =>
                {
                    option.Callback?.Invoke();
                    if (option.CloseOnClick)
                        Close();
                });
            }
        }

        public override void Close()
        {
            ClearButtonListeners();
            base.Close();
        }

        private void ClearButtonListeners()
        {
            foreach (ButtonSlot slot in _buttonSlots)
            {
                if (slot.Button != null)
                    slot.Button.onClick.RemoveAllListeners();
            }
        }
    }
}