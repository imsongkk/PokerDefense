using UnityEngine;
using static PokerDefense.Managers.RoundManager;
using PokerDefense.Managers;

public class InputManager : MonoBehaviour
{
    RoundState roundState = RoundState.NONE;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            roundState = GameManager.Round.CurrentState;
            if (roundState.HasFlag(
                RoundState.POKER | 
                RoundState.PLAY |
                RoundState.TOWER))
                return;
            // Tower¼±ÅÃ UI ¶ç¿ì±â
            // GameManager.UI.ShowPopupUI<> 
        }
    }
}
