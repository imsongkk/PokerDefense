using System.Collections;
using UnityEngine;
public class Effects4 : MonoBehaviour
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
			var random = Random.Range(0, 3);
			if (random == 0)
				_effects.TriggerSlash(Random.Range(0, 3), _t.localPosition);
			else if (random == 1)
				_effects.TriggerClaw(Random.value > .5f, _t.localPosition);
			else if (random == 2)
				_effects.TriggerSplatterBlood(_t.localPosition);
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
