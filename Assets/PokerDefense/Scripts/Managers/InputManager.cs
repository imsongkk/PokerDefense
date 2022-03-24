using UnityEngine;
using static PokerDefense.Managers.RoundManager;
using PokerDefense.Towers;
using UnityEngine.EventSystems;

namespace PokerDefense.Managers
{
    /* UI가 아닌 GameObject 상호작용 처리 매니저
     * InGameScene에서만 enable되며 DontDestroy에 담김
     */
    public class InputManager : MonoBehaviour
    {
        RoundState roundState = RoundState.NONE;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                roundState = InGameManager.Round.CurrentState;

                // UI 입력은 EventSystem이 따로 해주기 떄문에 무시
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
#else
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
#endif
                {
                    return;
                }

                TowerPanel selectedTowerPanel = FindTowerPanel(Input.mousePosition);

                // TowerPanel이외의 영역을 터치했을 때
                if (selectedTowerPanel == null) return;

                selectedTowerPanel.GetTouched(roundState);  
            }
        }

        private TowerPanel FindTowerPanel(Vector3 screenPos)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(screenPos);
            Ray ray = new Ray(touchPos, Vector3.forward);
            RaycastHit2D[] result = Physics2D.GetRayIntersectionAll(ray);

            for (int i = 0; i < result.Length; i++)
            {
                TowerPanel towerPanel = result[i].collider?.GetComponent<TowerPanel>();
                if (towerPanel != null)
                    return towerPanel;
            }
            return null;
        }
    }
}
