using PokerDefense.Managers;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerDefense.UI
{
    public class UI_UserItem : UI_Base
    {
        [SerializeField] Image itemImage;
        [SerializeField] Image backgroundImage;

        UI_UserItemSlots userItemSlots;

        private void Start()
            => Init();

        public override void Init()
            => BindObjects();

        private void BindObjects()
        {
            AddUIEvent(gameObject, OnClickItem, Define.UIEvent.Click);
        }

        public void InitItem(UI_UserItemSlots _userItemSlots, int itemId)
        {
            IsInit = true;

            userItemSlots = _userItemSlots;
        }

        public void ItemAdded(int count)
        {

        }

        public void ItemUsed()
        {

        }

        public void ItemDeleted()
        {

        }

        private void OnClickItem(PointerEventData evt)
        {
            if (!IsInit)
                return;



            Debug.Log("A");
        }
    }
}
