using System;

namespace CodeBase.Gameplay.Entities
{
    public interface IGameplayEntity
    {
        event Action<IGameplayEntity> ActiveStateChanged;
        bool IsActive { get; }
    }
}