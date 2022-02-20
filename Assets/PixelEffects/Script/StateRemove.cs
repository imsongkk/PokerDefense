using ca.HenrySoftware.Rage;
using UnityEngine;
namespace ca.HenrySoftware
{
	public class StateRemove : StateMachineBehaviour
	{
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var pool = animator.gameObject.GetComponentInParent<Pool>();
			if (pool != null)
			{
				animator.gameObject.SetActive(false);
				animator.runtimeAnimatorController = null;
				pool.Exit(animator.gameObject);
			}
		}
	}
}
