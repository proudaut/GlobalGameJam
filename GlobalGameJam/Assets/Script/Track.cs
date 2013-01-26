using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum TrackType
{
    Move,
    Action,
}

public class Track : MonoBehaviour 
{
	public int mDuration;
	public Level mLevel;
	public CharacterManager mCharactereManager;
	public GameObject mInputPrefab;
	public TrackType mTrackType;

	
	private Dictionary<int,InPutManager> mInputList = new Dictionary<int, InPutManager>();
	private bool mIsPlaying = false;
	private float mStartTime =0;
	private int mIndexAnimation = 0;
	void Start () 
	{
		if(mLevel != null)
		{
			int count = mLevel.mDuration/mDuration;
			for(int i=0; i<mDuration; i++)
			{
				GameObject lInput = Instantiate(mInputPrefab) as GameObject;
				lInput.transform.parent = this.transform;
				InPutManager lInPutManager = lInput.GetComponent<InPutManager>();
				lInPutManager.mPosition = i;
				lInPutManager.mCharactereManager = mCharactereManager;
				lInPutManager.mTrackType = mTrackType;
				lInput.transform.localPosition = new Vector3(lInPutManager.mPosition * 5 ,0,0);
				
				mInputList.Add(lInPutManager.mPosition,lInPutManager);
				
				//mInputList.Add(lInPutManager);
				for(int m=1 ;m<count; m++)
				{
					GameObject lInputCopy = Instantiate(mInputPrefab)as GameObject;
					lInputCopy.transform.parent = this.transform;
					InPutManager lInPutManagerCopy = lInputCopy.GetComponent<InPutManager>();
					lInPutManagerCopy.mPosition = (mDuration*m)+i;
					lInPutManagerCopy.mIsCopy = true;
					lInPutManagerCopy.mTrackType = mTrackType;
					lInPutManagerCopy.mCharactereManager = lInPutManager.mCharactereManager;
					lInPutManager.AddInputManagerCopy(lInPutManagerCopy);
					
					
					mInputList.Add(lInPutManagerCopy.mPosition,lInPutManagerCopy);
					
					
					
					lInputCopy.transform.localPosition = new Vector3(lInPutManagerCopy.mPosition * 5,0,0);
				}
			}
		}
	}
	
	
	public void Play()
	{
		mIsPlaying = true;
		mStartTime = Time.time -2;
		mIndexAnimation = 0;
	}
	
	
	
	public void Stop()
	{
		mIsPlaying = false;

	}
	
	void Update () 
	{
		if (mIsPlaying)
		{
			if(Time.time - mStartTime>=1.3)
			{
				mStartTime = Time.time;
				if(mInputList.Count>mIndexAnimation)
				{
					mInputList[mIndexAnimation].Play();
					mIndexAnimation++;
				}
			}
		}
	}
}
