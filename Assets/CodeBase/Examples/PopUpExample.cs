using CodeBase.Services;
using CodeBase.UI;

namespace CodeBase.Examples
{
    public class PopUpExample
    {
        public void ExampleDialogMethod()
        {
            PopUpService.Instance.OpenDialog(new PopUpDialogData
            {
                Title = "Выход",
                Body = "Сохранить перед выходом?",
                Buttons = new[]
                {
                    new PopUpButtonOption("Да", () => OnYes()),
                    new PopUpButtonOption("Нет", () => OnNo()),
                    new PopUpButtonOption("Отмена", null)
                }
            });
        }

        public void ExampleSaveMethod()
        {
            SaveSystem.SaveSystem.Instance.LoadLatestOrNew();
            SaveSystem.SaveSystem.Instance.SaveData.Money = 100;
            SaveSystem.SaveSystem.Instance.QuickSaveToStorage();
        }

        private void OnNo()
        {
            // Какой-то смешной комментарий 1
        }

        private void OnYes()
        {
            // Какой-то смешной комментарий 2
            SaveSystem.SaveSystem.Instance.QuickSaveToStorage();
        }
    }
}