using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using PokerDefense.UI.Popup;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class InGameScene : BaseScene
    {
        [SerializeField] private GameObject RoundManagerObject, PokerManagerObject, HorseManagerObject, SkillManagerObject;

        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            GameManager.UI.ShowSceneUI<UI_InGameScene>();

            InitManagers();
        }

        private void InitManagers()
        {
            GameManager.AddRoundManager(RoundManagerObject);
            GameManager.GetOrAddPokerManager(PokerManagerObject);
            GameManager.AddHorseManager(HorseManagerObject);
            GameManager.AddSkillManager(SkillManagerObject);
            GameManager.AddInputManager();

            GameManager.Tower.InitTowerManager();
            GameManager.Data.InitDataManager();
            GameManager.Round.InitRoundManager();
            GameManager.Skill.InitSkillManager();

            AddOnDestroyAction(() => GameManager.DeleteInputManager());
        }
    }
}