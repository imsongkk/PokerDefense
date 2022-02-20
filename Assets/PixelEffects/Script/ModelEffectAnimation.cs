using UnityEngine;
namespace ca.HenrySoftware
{
	[CreateAssetMenu]
	public class ModelEffectAnimation : ScriptableObject
	{
		public RuntimeAnimatorController Controller;
		public bool Blend;
		public Vector2 Offset = Vector2.zero;
		public Vector2 BackOffset = Vector2.zero;
		public Vector2 ForeOffset = Vector2.zero;
	}
}
