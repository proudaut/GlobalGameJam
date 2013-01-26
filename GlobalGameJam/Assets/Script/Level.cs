using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Level : MonoBehaviour
{
	public int mDuration;
	public Animation mCharacterAnimation;
	public GameObject mCharater;
	public List<Track> mTrack;
	
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
		for(int i= 0 ; i < mDuration ; i++)
		{
			foreach(Track lTrack in mTrack)
			{
				string lAnimationName = lTrack.mTrackAnimation[i];
				if(lAnimationName.Length>0)
				{
					mCharacterAnimation[lAnimationName].layer = mTrack.IndexOf(lTrack);
					mCharacterAnimation.Play(lAnimationName);
				}
			}
			yield return new WaitForSeconds(0.66666f);
		}
		
		mIsPlaying = false;
		mCharater.transform.localPosition = new Vector3(-18,0,0);
		
		
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
