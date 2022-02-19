using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using PokerDefense.UI.Popup;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class InGameScene : BaseScene
    {
        [SerializeField] private GameObject RoundManagerObject, PokerManagerObject;

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
            GameManager.GetOrAddPokerManager(PokerManagerObject);
            GameManager.AddInputManager();

            GameManager.Tower.InitTowerManager();
            GameManager.Data.InitDataManager();
            GameManager.Round.InitRoundManager();

            AddOnDestroyAction(() => GameManager.DeleteInputManager());
        }
    }
}