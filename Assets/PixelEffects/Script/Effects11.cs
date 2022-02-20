using System.Collections;
using UnityEngine;
public class Effects11 : MonoBehaviour
{
	Transform _t;
	Effects _effects;
	void Awake()
	{
		_t = transform;
		_effects = GetComponentInParent<Effects>();
	}
	void Start()
	{
		StartCoroutine(Test());
	}
	IEnumerator Test()
	{
		yield return new WaitForSeconds(.333f);
		while (true)
		{
			if (Random.value > .5f)
				_effects.TriggerTeleport(_t.localPosition);
			else
				_effects.TriggerNuclear(_t.localPosition);
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
