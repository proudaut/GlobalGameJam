using UnityEngine;
using System.Collections;

public class Breakbale : MonoBehaviour
{
	public ParticleSystem speed;
	public ParticleSystem broken;
	
	public void Break()
	{
		speed.Play();
		broken.Play();
	}
	public void BreakStop()
	{
		speed.Stop();
		broken.Stop();
	}

}
