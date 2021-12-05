using UnityEngine;
using static PokerDefense.Managers.RoundManager;
using PokerDefense.Managers;
using PokerDefense.UI.Popup;
using PokerDefense.Towers;
using UnityEngine.EventSystems;

namespace PokerDefense.Managers
{
    public class InputManager : MonoBehaviour
    {
        RoundState roundState = RoundState.NONE;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject()) // UI ��ġ�� �ȹ���
#else
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
#endif
                {
                    return;
                }

                roundState = GameManager.Round.CurrentState;

                if (roundState == RoundState.TOWER)
                {
                    FindTowerPanel();
                    return;
                }
            }
        }

        private void FindTowerPanel()
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(touchPos, Vector3.forward);
            RaycastHit2D[] result = Physics2D.GetRayIntersectionAll(ray);
            for (int i = 0; i < result.Length; i++)
            {
                TowerPanel towerPanel = result[i].collider?.GetComponent<TowerPanel>();
                if (towerPanel != null)
                {
                    UI_TowerSelectPopup towerselect = GameManager.UI.ShowPopupUI<UI_TowerSelectPopup>();
                    towerselect.SetTowerPanel(towerPanel);
                }
            }
        }
    }
}
