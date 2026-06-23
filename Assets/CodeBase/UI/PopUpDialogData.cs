using System;

namespace CodeBase.UI
{
    public class PopUpDialogData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public PopUpButtonOption[] Buttons { get; set; }
    }
}