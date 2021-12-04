using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using PokerDefense.UI.Popup;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class InGameScene : BaseScene
    {
        [SerializeField] private GameObject RoundManagerObject, TowerManagerObject;

        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            GameManager.UI.ShowSceneUI<UI_InGameScene>();
            GameManager.UI.ShowPopupUI<UI_Poker>();

            GameManager.AddTowerManager(TowerManagerObject);
            GameManager.AddRoundManager(RoundManagerObject);
            GameManager.GetOrAddPokerManager(RoundManagerObject);
            GameManager.AddInputManager();

            AddOnDestroyAction(() => GameManager.DeleteInputManager());
        }
    }
}