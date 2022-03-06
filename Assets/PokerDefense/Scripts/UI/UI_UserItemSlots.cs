using PokerDefense.Managers;
using PokerDefense.Utils;
using static PokerDefense.Utils.Define;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PokerDefense.UI
{
    public class UI_UserItemSlots : UI_Base
    {
        [SerializeField] Transform content;
        [SerializeField] List<UI_UserItem> ui_UserItemList = new List<UI_UserItem>();

        public override void Init() { }

        public void InitItemSlots()
        {
            // 이미 6개는 칸수 구분을 위해 들어가있는 상황, 다만 각 아이템들은 초기화가 되어있지 않다.
            // TODO : 유저의 아이템 정보 가져오기
            ui_UserItemList[0].InitItem(this);
        }

        public void AddItem()
        {

        }
    }
}
