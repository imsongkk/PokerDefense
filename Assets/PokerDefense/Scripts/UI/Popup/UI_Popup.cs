using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerDefense.UI;
using PokerDefense.Managers;
using UnityEngine.EventSystems;
using PokerDefense.Utils;

namespace PokerDefense.UI.Popup
{
    public class UI_Popup : UI_Base
    {
        public override void Init()

        {
            IStoppable obj = this as IStoppable;
            if (obj != null)
                obj.Stop();
            GameManager.UI.SetCanvas(gameObject, true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IStoppable obj = this as IStoppable;
            if (obj != null)
                obj.Resume();
        }

        public virtual void ClosePopupUI()
        {
            GameManager.UI.ClosePopupUI(this);
        }
    }
}
