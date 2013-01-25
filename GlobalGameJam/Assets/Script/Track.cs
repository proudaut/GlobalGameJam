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
	public TrackType mTrackType;
	public List<string> mAnimation;
	public Level mLevel;
	
	public GameObject mInputPrefab;

	private ArrayList mTrackAnimation;
	public List<GameObject> mInputList = new List<GameObject>();
	
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
				lInput.transform.localPosition = new Vector3(lInPutManager.mPosition * 10 ,0,0);
				
				mInputList.Add(lInput);
				for(int m=1 ;m<count; m++)
				{
					GameObject lInputCopy = Instantiate(mInputPrefab)as GameObject;
					lInputCopy.transform.parent = this.transform;
					InPutManager lInPutManagerCopy = lInputCopy.GetComponent<InPutManager>();
					lInPutManagerCopy.mPosition = (mDuration*m)+i;
					lInPutManagerCopy.mIsCopy = true;
					lInPutManager.AddInputManagerCopy(lInPutManagerCopy);
					
					lInputCopy.transform.localPosition = new Vector3(lInPutManagerCopy.mPosition * 10,0,0);
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
