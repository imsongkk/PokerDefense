using PokerDefense.Managers;
using PokerDefense.Utils;

namespace PokerDefense.UI.Popup
{
    public class UI_Popup : UI_Base
    {
        protected bool isStoppable = false;

        public override void Init()
        {
            if (isStoppable) 
                Util.Stop();
            GameManager.UI.SetCanvas(gameObject, true);
        }

        protected override void OnDestroy()
        {
            if (isStoppable)
                Util.Resume();
            base.OnDestroy();
        }

        public virtual void ClosePopupUI()
        {
            GameManager.UI.ClosePopupUI(this);
        }
    }
}
