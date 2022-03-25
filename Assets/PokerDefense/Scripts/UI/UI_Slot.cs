using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PokerDefense.Data;
using TMPro;
using PokerDefense.Managers;

namespace PokerDefense.UI
{
    public class UI_Slot : UI_Base
    {
		public override void Init() { }

        public void InitUI(int index)
        {
            SlotData slotData = GameManager.Data.SlotDataList[index];

            if(slotData != null) // ∫Û ΩΩ∑‘¿Ã æ∆¥— ±‚¡∏¿« ΩΩ∑‘
			{
                var text = GetComponentInChildren<TextMeshProUGUI>();
                text.text = slotData.hardNess + " " + slotData.round;
			}
        }
    }
}
