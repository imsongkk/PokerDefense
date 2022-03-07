using PokerDefense.Managers;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PokerDefense.UI
{
    public class UI_UserItem : UI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] Image itemImage;
        [SerializeField] Image backgroundImage;

        UI_UserItemSlots userItemSlots;
        ScrollRect scrollRect;
        string itemId;

        private void Start()
            => Init();

        public override void Init()
            => BindObjects();

        private void BindObjects()
        {
            AddUIEvent(gameObject, OnClickItem, Define.UIEvent.Click);
        }

        public void InitItem(UI_UserItemSlots _userItemSlots, string _itemId)
        {
            userItemSlots = _userItemSlots;
            scrollRect = userItemSlots.GetComponentInChildren<ScrollRect>();
            itemId = _itemId;
        }

        public void ItemPurchased(int count)
        {

        }

        public void ItemUsed()
        {

        }

        public void ItemDeleted()
        {
            GameManager.Resource.Destroy(gameObject);
        }

        private void OnClickItem(PointerEventData evt)
        {
            GameManager.Inventory.OnClickUse(itemId);
            scrollRect.OnBeginDrag(evt);
        }

        public void OnDrag(PointerEventData eventData)
        {
            ((IDragHandler)scrollRect).OnDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ((IBeginDragHandler)scrollRect).OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ((IEndDragHandler)scrollRect).OnEndDrag(eventData);
        }
    }
}
