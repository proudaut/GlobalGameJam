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
		//Debug.Log("PLAY JUMP");
		
		StartCoroutine(Play());
	}
	
	IEnumerator Play()
	{
		yield return true;
		
		
		foreach(Track lTrack in mTrack)
		{
			lTrack.GenerateAnimation();
		}
		
		
		mIsPlaying = true;
		mSound.Play();
		
		
		for(int i= 0 ; i < mDuration ; i++)
		{
			foreach(Track lTrack in mTrack)
			{
				string lAnimationName = lTrack.mTrackAnimation[i];
				
				if( lAnimationName == "jump")
				{
					lTrack.mSound1.Play();
				}
				else if( lAnimationName == "slide")
				{
					lTrack.mSound3.Play();
				}
				if( lAnimationName == "shield")
				{
					lTrack.mSound1.Play();
				}
				else if( lAnimationName == "attack")
				{
					lTrack.mSound3.Play();
				}
				
				
				
				//lTrack.mSound1.Play();
				
				//AudioSource lAudioSource = lTrack.mTrackSound[i];
				//lAudioSource.Play();
				
				if(lAnimationName.Length>0)
				{
					mCharacterAnimation[lAnimationName].layer = mTrack.IndexOf(lTrack);
					mCharacterAnimation.Play(lAnimationName);
					
				}
			}
			yield return new WaitForSeconds(0.653f);
		}
		
		mIsPlaying = false;
		mCharater.transform.localPosition = new Vector3(-18,0,0);
		mSound.Stop();
		
		foreach(Track lTrack in mTrack)
		{
			lTrack.CleanAnimation();
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
