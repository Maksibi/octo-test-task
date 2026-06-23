using System;

namespace CodeBase.UI
{
    public readonly struct PopUpButtonOption
    {
        public string Label { get; }
        public Action Callback { get; }
        public bool CloseOnClick { get; }

        public PopUpButtonOption(string label, Action callback, bool closeOnClick = true)
        {
            Label = label;
            Callback = callback;
            CloseOnClick = closeOnClick;
        }
    }
}