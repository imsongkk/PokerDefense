using System.Collections;
using UnityEngine;
public class Effects1 : MonoBehaviour
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
			var random = Random.Range(0, 6);
			if (random == 0)
				_effects.TriggerStar(_t.localPosition);
			else if (random == 1)
				_effects.TriggerCircle(_t.localPosition);
			else if (random == 2)
				_effects.TriggerGlint(_t.localPosition);
			else if (random == 3)
				_effects.TriggerPuff(_t.localPosition);
			else if (random == 4)
				_effects.TriggerWeb(_t.localPosition);
			else if (random == 5)
				_effects.TriggerBlock(_t.localPosition);
			yield return new WaitForSeconds(Random.Range(.5f, 1f));
		}
	}
}
