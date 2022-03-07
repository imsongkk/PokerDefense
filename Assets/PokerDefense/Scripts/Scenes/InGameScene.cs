using PokerDefense.Managers;
using PokerDefense.UI.Scene;
using UnityEngine;

namespace PokerDefense.Scene
{
    public class InGameScene : BaseScene
    {
        [SerializeField] private GameObject RoundManagerObject, PokerManagerObject, HorseManagerObject, SkillManagerObject, SystemMessageManagerObject;
        UI_InGameScene uI_InGameScene;

        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            uI_InGameScene = GameManager.UI.ShowSceneUI<UI_InGameScene>();

            InitManagers();
        }

        private void InitManagers()
        {
            GameManager.AddRoundManager(RoundManagerObject);
            GameManager.GetOrAddPokerManager(PokerManagerObject);
            GameManager.AddHorseManager(HorseManagerObject);
            GameManager.AddSkillManager(SkillManagerObject);
            GameManager.AddSystemMessageManager(SystemMessageManagerObject);
            GameManager.AddInputManager();

            GameManager.Tower.InitTowerManager();
            GameManager.Data.InitDataManager();
            GameManager.Skill.InitSkillManager();
            GameManager.Round.InitRoundManager(uI_InGameScene);
            GameManager.Inventory.InitInventoryManager(uI_InGameScene);
            GameManager.Horse.InitHorseManager();
            GameManager.SystemText.InitSystemTextManager(uI_InGameScene);

            AddOnDestroyAction(() => GameManager.DeleteInputManager());
        }
    }
}