using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PokerDefense.UI
{
    public class UI_RemainItem : UI_Base
    {
        [SerializeField] Image remainItemImage, remainTimeImage;

        public override void Init() { }

        public void InitUI(float remainTime, Image remainItemImage)
        {
            this.remainItemImage.gameObject.SetActive(true);
            //this.remainItemImage = remainItemImage;
            StartCoroutine(ShowRemainTime(remainTimeImage, remainTime));
        }

        IEnumerator ShowRemainTime(Image targetImage, float coolTime)
        {
            float time = 0f;
            targetImage.fillAmount = 1f;

            while (time <= coolTime)
            {
                targetImage.fillAmount = 1 - time / coolTime;
                time += Time.deltaTime;
                yield return null;
            }

            targetImage.fillAmount = 0f;

            Destroy(gameObject);
        }
    }
}
