using System.Collections;
using UnityEngine;
public class Effects5 : MonoBehaviour
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
				_effects.TriggerSparks(_t.localPosition);
			else
			{
				var type = Random.value > .5f ? (Random.value > .5f ? 0 : 1) : 2;
				_effects.TriggerConsume(type, _t.localPosition);
			}
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
