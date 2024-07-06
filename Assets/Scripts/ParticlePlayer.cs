using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlayer : MonoBehaviour, IPoolable
{
	ParticleSystem _ps;

	public void Active()
	{
		if(_ps == null)
			_ps = GetComponent<ParticleSystem>();
		gameObject.SetActive(true);
	}

	public void SetColor(Color c)
	{
		if(_ps == null)
			_ps = GetComponent<ParticleSystem>();

		ParticleSystem.MainModule mm = _ps.main;
		mm.startColor = c;
	}

	public void Deactive()
	{
		gameObject.SetActive(false);
	}
}