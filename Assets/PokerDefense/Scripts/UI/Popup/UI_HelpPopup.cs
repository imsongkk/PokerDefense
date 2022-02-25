using UnityEngine;
using PokerDefense.Utils;
using UnityEngine.EventSystems;

namespace PokerDefense.UI.Popup
{
    public class UI_HelpPopup : UI_Popup
    {
        enum GameObjects
        {
            PokerHelpButton,
            EnemyHelpButton,
            TowerHelpButton,
            BackButton,
        }

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            GameObject pokerHelpButton = GetObject((int)GameObjects.PokerHelpButton);
            AddUIEvent(pokerHelpButton, OnClickPokerHelpButton, Define.UIEvent.Click);
            AddButtonAnim(pokerHelpButton);

            GameObject enemyHelpButton = GetObject((int)GameObjects.EnemyHelpButton);
            AddUIEvent(enemyHelpButton, OnClickEnemyHelpButton, Define.UIEvent.Click);
            AddButtonAnim(enemyHelpButton);

            GameObject towerHelpButton = GetObject((int)GameObjects.TowerHelpButton);
            AddUIEvent(towerHelpButton, OnClickTowerHelpButton, Define.UIEvent.Click);
            AddButtonAnim(towerHelpButton);

            GameObject backButton = GetObject((int)GameObjects.BackButton);
            AddUIEvent(backButton, (e) => ClosePopupUI(), Define.UIEvent.Click);
            AddButtonAnim(backButton);
        }

        private void OnClickPokerHelpButton(PointerEventData evt)
        {

        }

        private void OnClickTowerHelpButton(PointerEventData evt)
        {

        }

        private void OnClickEnemyHelpButton(PointerEventData evt)
        {
            
        }
    }
}
