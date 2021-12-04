using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class InGameScene : BaseScene
    {
        [SerializeField] private GameObject RoundManagerObject;

        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            GameManager.UI.ShowSceneUI<UI_InGameScene>();
            InitManagers();
        }

        private void InitManagers()
        {
            GameManager.AddRoundManager(RoundManagerObject);
            GameManager.AddInputManager();
            GameManager.Tower.InitTowerManager();

            AddOnDestroyAction(() => GameManager.DeleteInputManager());
        }
    }
}