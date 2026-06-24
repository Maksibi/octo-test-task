using CodeBase.SaveService;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrap : IInitializable
    {
        private readonly SaveSystem _saveSystem;

        public GameBootstrap(SaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        public void Initialize()
        {
        }
    }
}
