using System;

namespace CodeBase.SaveService
{
    [Serializable]
    public class PlayerSaveData
    {
        // Здесь хранятся данные игрока: деньги, жизни, что угодно. Можно сделать что-то по типу "GirlRelationshipData" и тоже хранить здесь,
        //  либо сохранять и загружать данные отдельно.
        //
        // События нужны для того, чтобы при изменении чего-либо в данных игрока эти изменения отобразил UI. (Опционально)

        private int cachedMoney;
        public event Action<int> OnMoneyChanged;
        
        public void Initialize()
        {
            Money = 0;
        }

        public int Money
        {
            get => cachedMoney;
            set
            {
                if (cachedMoney == value)
                    return;
                
                cachedMoney = value;
                OnMoneyChanged?.Invoke(cachedMoney);
            }
        }
    }
}