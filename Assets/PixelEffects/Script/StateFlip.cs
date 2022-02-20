using UnityEngine;
namespace ca.HenrySoftware
{
	public class StateFlip : StateMachineBehaviour
	{
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.gameObject.transform.localScale = new Vector3((Random.value > .5f) ? 1f : -1f, 1f, 1f);
		}
	}
}
