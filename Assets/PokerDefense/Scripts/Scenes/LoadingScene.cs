using PokerDefense.Managers;
using PokerDefense.UI.Scene;

namespace PokerDefense.Scene
{
    public class LoadingScene : BaseScene
    {
        private void Start()
            => Init();

        protected override void Init()
        {
            base.Init();
            // ENTRY POINT
            SceneType = Utils.Define.Scene.Loading;
            GameManager.UI.ShowSceneUI<UI_LoadingScene>("UI_LoadingScene");
            InitManagers();
        }

        private void InitManagers()
		{
            GameManager.Data.InitDataManager();
            // TODO : 게임 보드 연동, Firebase 연동
		}
    }
}
