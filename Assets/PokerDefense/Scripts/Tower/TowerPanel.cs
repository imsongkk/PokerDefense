using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using UnityEngine;
using static PokerDefense.Managers.RoundManager;

namespace PokerDefense.Towers
{
    public class TowerPanel : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        GameObject towerBase;

        Tower tower;

        Color originColor;

        public int Index { get; private set; }

        private void Start()
            => Init();

        private void Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originColor = spriteRenderer.color;
            towerBase = transform.GetChild(0).gameObject;
            towerBase.SetActive(false);

            Index = transform.GetSiblingIndex();
        }

        public void GetTouched(RoundState currentState) // InputManager에 의해 호출
        {
            switch(currentState)
            {
                case RoundState.NONE:
                case RoundState.STOP:
                case RoundState.READY:
                    return;
                case RoundState.TOWER:
                    if (HasTower()) // TowerPanel에 이미 Tower가 있을 경우
                    {
                        UI_TowerTouchPopup towerTouchPopup = GameManager.UI.ShowPopupUI<UI_TowerTouchPopup>();
                        towerTouchPopup.SetTouchedTowerPanel(this);
                    }
                    else
                    {
                        UI_TowerSelectPopup towerSelectPopup = GameManager.UI.ShowPopupUI<UI_TowerSelectPopup>();
                        towerSelectPopup.SetTowerPanel(this);
                    }
                    break;
                case RoundState.PLAY:
                    if (HasTower()) // TowerPanel에 이미 Tower가 있을 경우
                    {
                        UI_TowerTouchPopup towerTouchPopup = GameManager.UI.ShowPopupUI<UI_TowerTouchPopup>();
                        towerTouchPopup.SetTouchedTowerPanel(this);
                    }
                    break;
            }
        }

        public void SetTower(Tower target)
        {
            tower = target;
            SetTowerBaseStatus(false);
        }

        public Tower GetTower()
            => tower;

        public void OnEndPoker()
            => ResetPanel();

        public void SetTowerBaseStatus(bool setBase)
            => towerBase.SetActive(setBase);

        public void HighligtPanel()
            => spriteRenderer.color = Color.red;

        public void ResetPanel()
            => spriteRenderer.color = originColor;

        public bool HasTower()
            => tower != null;
    }
}