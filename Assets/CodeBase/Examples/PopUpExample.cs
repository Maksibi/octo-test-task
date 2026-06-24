using CodeBase.SaveService;
using CodeBase.Services;
using CodeBase.UI;
using Zenject;

namespace CodeBase.Examples
{
    public class PopUpExample
    {
        private readonly PopUpService _popUpService;
        private readonly SaveSystem _saveSystem;

        [Inject]
        public PopUpExample(PopUpService popUpService, SaveSystem saveSystem)
        {
            _popUpService = popUpService;
            _saveSystem = saveSystem;
        }

        public void ExampleDialogMethod()
        {
            TextPopUp dialogPopUp = new TextPopUp();
            dialogPopUp = _popUpService.OpenDialog(new PopUpDialogData
            {
                Title = "Выход",
                Body = "Сохранить перед выходом?",
                Buttons = new[]
                {
                    new PopUpButtonOption("Да", () => OnYes()),
                    new PopUpButtonOption("Нет", () => OnNo()),
                    new PopUpButtonOption("Отмена", () => dialogPopUp.Close()),
                }
            });
        }

        public void ExampleSaveMethod()
        {
            
        }

        private void OnNo()
        {
            // Не сохраняемся и выходим
        }

        private void OnYes()
        {
            // Сохраняемся через saveSystem и выходим
        }
    }
}
