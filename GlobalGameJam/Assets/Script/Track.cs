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
	public List<string> mAnimation;
	public Level mLevel;
	
	public GameObject mInputPrefab;

	public List<string> mTrackAnimation= new List<string>();
	public ArrayList mTrackSound= new ArrayList();
	
	private List<InPutManager> mInputList = new List<InPutManager>();
	
	public AudioSource mSound1;
	public AudioSource mSound2;
	public AudioSource mSound3;
	
	
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
				lInput.transform.localPosition = new Vector3(lInPutManager.mPosition * 5 ,0,0);
				
				mInputList.Add(lInPutManager);
				for(int m=1 ;m<count; m++)
				{
					GameObject lInputCopy = Instantiate(mInputPrefab)as GameObject;
					lInputCopy.transform.parent = this.transform;
					InPutManager lInPutManagerCopy = lInputCopy.GetComponent<InPutManager>();
					lInPutManagerCopy.mPosition = (mDuration*m)+i;
					lInPutManagerCopy.mIsCopy = true;
					lInPutManager.AddInputManagerCopy(lInPutManagerCopy);
					lInputCopy.transform.localPosition = new Vector3(lInPutManagerCopy.mPosition * 5,0,0);
				}
			}
		}

	}
	
	public void GenerateAnimation()
	{
		int count = mLevel.mDuration/mDuration;
		for(int i=0; i<count; i++)
		{
			foreach(InPutManager lInPutManager in mInputList)
			{
				switch(lInPutManager.mInPutState)
				{
					case InPutState.Middle :
					{
						mTrackAnimation.Add(mAnimation[1]); break;
						mTrackSound.Add(mSound2);
					}
					case InPutState.Down :
					{
						mTrackAnimation.Add(mAnimation[0]); break;
						mTrackSound.Add(mSound1);
					}
					case InPutState.Up :
					{
						mTrackAnimation.Add(mAnimation[2]); break;
						mTrackSound.Add(mSound3);
					}
				}
			}
		}
		Debug.Log("sound count : " + mTrackSound.Count);
		Debug.Log("sound count : " + mTrackAnimation.Count);
	}
	
	
	public void CleanAnimation()
	{
		mTrackAnimation.Clear();
		mTrackSound.Clear();
	}
	
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
