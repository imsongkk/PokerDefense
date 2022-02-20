using System.Collections;
using UnityEngine;
public class Effects7 : MonoBehaviour
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
			var random = Random.Range(0, 4);
			if (random == 0)
				_effects.TriggerFire(_t.localPosition);
			else if (random == 1)
				_effects.TriggerEarth(_t.localPosition);
			else if (random == 2)
				_effects.TriggerIce(_t.localPosition);
			else if (random == 3)
				_effects.TriggerWater(_t.localPosition);
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
