using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public int mDuration;
	public Animation mCharacterAnimation;
	public GameObject mCharater;
	public List<Track> mTrack;
	public AudioSource mSound;
	private bool mIsPlaying = false;
	
	void OnButtonClick(Button _button)
	{
		if(mIsPlaying == false)
		{
			Play();
		}
		else
		{
			Stop();
		}
	}
	
	void Play()
	{
		mIsPlaying = true;
		mSound.Play();
		foreach(Track lTrack in mTrack)
		{
			lTrack.Play();
		}	
	}
	
	void Stop()
	{
		mIsPlaying = false;
		mCharater.transform.localPosition = new Vector3(-18,0,0);
		mSound.Stop();
		foreach(Track lTrack in mTrack)
		{
			lTrack.Stop();
		}	
	}
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mIsPlaying)
		{
			mCharater.transform.Translate(Vector3.right * Time.deltaTime * 3);
		}
	}
}
