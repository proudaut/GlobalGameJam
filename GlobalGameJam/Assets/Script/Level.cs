using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Level : MonoBehaviour
{
	public int mDuration;
	public Animation mCharacterAnimation;
	public GameObject mCharater;
	public List<Track> mTrack;
	public List<LevelElement> mLevelElements;
	public AudioSource mSound;

	
	private bool mIsPlaying = false;
	
	
	public GameObject G;
	public GameObject GL;
	public GameObject GR;
	public GameObject GD;
	public GameObject GF;
	public GameObject spi;
	public GameObject mid;
	public GameObject low;
	public GameObject vc;
	public GameObject hc;
	
	
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
	void Start () 
	{
	
	}
	

	
	void GenerateLevel()
	{
		foreach(LevelElement lLevelElement in mLevelElements)
		{
			switch(lLevelElement.mLevelElementType)
			{
				case LevelElementType.G :  Instantiate(G); break;
				case LevelElementType.GL :  Instantiate(GL); break;
				case LevelElementType.GR :  Instantiate(GR); break;
				case LevelElementType.GD :  Instantiate(GD); break;
				case LevelElementType.GF :  Instantiate(GF); break;
				case LevelElementType.spi :  Instantiate(spi); break;
				case LevelElementType.mid :  Instantiate(mid); break;
				case LevelElementType.low :  Instantiate(low); break;
				case LevelElementType.vc :  Instantiate(vc); break;
				case LevelElementType.hc :  Instantiate(hc); break;
			}
		}
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
