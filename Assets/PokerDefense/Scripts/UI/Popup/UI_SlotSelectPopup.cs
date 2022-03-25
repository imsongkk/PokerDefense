using UnityEngine;
using PokerDefense.Utils;

namespace PokerDefense.UI.Popup
{
    public class UI_SlotSelectPopup : UI_Popup
    {
        enum GameObjects
        {
            SlotList,
        }

        Transform slotList;

        private void Start()
            => Init();

        public override void Init()
        {
            base.Init();

            BindObjects();
            InitSlotUIs();
        }

        private void BindObjects()
        {
            Bind<GameObject>(typeof(GameObjects));

            slotList = GetObject((int)GameObjects.SlotList).transform;
        }

        private void InitSlotUIs()
		{
            var slotUIList = slotList.GetComponentsInChildren<UI_Slot>();
            for(int i=0; i<slotUIList.Length; i++)
			{
                slotUIList[i].InitUI(i);
			}
		}
    }
}
