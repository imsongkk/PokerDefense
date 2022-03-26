using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using PokerDefense.Managers;
using PokerDefense.Utils;
using PokerDefense.UI.Popup;

namespace PokerDefense.UI
{
    public class UI_Slot : UI_Base
    {
        int index;
        GameObject deleteButton;
        TextMeshProUGUI text;

        enum GameObjects
        {
            DeleteButton,
        }

        private void Awake()
            => Init();
        public override void Init() 
        {
            BindObjects();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            text = GetComponentInChildren<TextMeshProUGUI>();

            AddUIEvent(gameObject, OnClickSlot, Define.UIEvent.Click);

            deleteButton = GetObject((int)GameObjects.DeleteButton);
            AddUIEvent(deleteButton, OnClickDeleteButton, Define.UIEvent.Click);
            AddButtonAnim(deleteButton);
        }

        public void InitUI(int index)
        {
            this.index = index;

            Refresh();
        }

        private void OnClickSlot(PointerEventData evt)
        {
            if(GameManager.InGameSceneMode == GameManager.inGameSceneMode.NewGame)
            {
                // TODO : 난이도, 게임 모드도 선택 가능하게 하기
                if(GameManager.Data.SlotDataList[index] == null) // 정상 새 게임
                    InGameManager.NewGame(index);
                else // 덮어쓰기
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_SlotOverwritePopup>();
                    popup.SlotIndex = index;
                }
            }
            else if(GameManager.InGameSceneMode == GameManager.inGameSceneMode.LoadGame)
            {
                if (GameManager.Data.SlotDataList[index] == null) // Load할 수 없음
                    GameManager.UI.ShowPopupUI<UI_SlotErrorPopup>();
                else // 정상 로드
                    InGameManager.LoadGame(index);
            }
        }

        private void OnClickDeleteButton(PointerEventData evt)
        {
            if (GameManager.Data.SlotDataList[index] == null) // 빈 슬롯이면
                return;

            GameManager.Data.DeleteSlot(index, Refresh);
        }

        private void Refresh()
        {
            if (GameManager.Data.SlotDataList[index] != null) // 빈 슬롯이 아닌 기존의 슬롯
                text.text = GameManager.Data.SlotDataList[index].hardNess + " " + GameManager.Data.SlotDataList[index].round;
            else
                text.text = "빈 슬롯";
        }
    }
}
