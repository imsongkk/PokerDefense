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
                // TODO : ���̵�, ���� ��嵵 ���� �����ϰ� �ϱ�
                if(GameManager.Data.SlotDataList[index] == null) // ���� �� ����
                    InGameManager.NewGame(index);
                else // �����
                {
                    var popup = GameManager.UI.ShowPopupUI<UI_SlotOverwritePopup>();
                    popup.SlotIndex = index;
                }
            }
            else if(GameManager.InGameSceneMode == GameManager.inGameSceneMode.LoadGame)
            {
                if (GameManager.Data.SlotDataList[index] == null) // Load�� �� ����
                    GameManager.UI.ShowPopupUI<UI_SlotErrorPopup>();
                else // ���� �ε�
                    InGameManager.LoadGame(index);
            }
        }

        private void OnClickDeleteButton(PointerEventData evt)
        {
            if (GameManager.Data.SlotDataList[index] == null) // �� �����̸�
                return;

            GameManager.Data.DeleteSlot(index, Refresh);
        }

        private void Refresh()
        {
            if (GameManager.Data.SlotDataList[index] != null) // �� ������ �ƴ� ������ ����
                text.text = GameManager.Data.SlotDataList[index].hardNess + " " + GameManager.Data.SlotDataList[index].round;
            else
                text.text = "�� ����";
        }
    }
}
