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
            // �̹� 6���� ĭ�� ������ ���� ���ִ� ��Ȳ, �ٸ� �� �����۵��� �ʱ�ȭ�� �Ǿ����� �ʴ�.
            // TODO : ������ ������ ���� ��������
            ui_UserItemList[0].InitItem(this);
        }

        public void AddItem()
        {

        }
    }
}
