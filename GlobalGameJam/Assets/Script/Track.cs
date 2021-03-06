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
	public Level mLevel;
	public CharacterManager mCharactereManager;

	public GameObject mInputPrefab;
	public TrackType mTrackType;
	public Dictionary<int,InPutManager> mInputList = new Dictionary<int, InPutManager>();
	public List<GameObject> mInputInstanciate = new List<GameObject>();
	
	public MovieClipBehaviour mTrackBackGround;
	public UILabel mTopLabelMax;
	public UILabel mMinLabelMax;
	public AudioSource mSourceNot;
	
	public int mDuration;
	public int mTopMax;
	public int mMinMax;
	public string mTopSound;
	public string mMinSound;	
	

	public void Configure()
	{
		foreach(GameObject lObj in mInputInstanciate)
		{
			Destroy(lObj);
		}
		mInputInstanciate.Clear();
		mInputList.Clear();
		
		
		mTopLabelMax.text = mTopMax.ToString();
		mMinLabelMax.text = mMinMax.ToString();
		if(mLevel != null && mDuration==0)
		{
			if(mTrackBackGround!= null)
			{
				mTrackBackGround.gameObject.SetActiveRecursively(false);
			}
		}
		if(mLevel != null && mDuration>0)
		{
			if(mTrackBackGround != null)
			{
				mTrackBackGround.movieClip.gotoAndStop(	mDuration - 1 );
			}
			float lD = mLevel.mDuration ;
			float lD1 = mDuration;
			
			int count = (int)Mathf.Ceil(lD/lD1);
			for(int i=0; i<mDuration; i++)
			{
				GameObject lInput = Instantiate(mInputPrefab) as GameObject;
				mInputInstanciate.Add(lInput);
				lInput.transform.parent = this.transform;
				InPutManager lInPutManager = lInput.GetComponent<InPutManager>();
				lInPutManager.mPosition = i;
				lInPutManager.mCharactereManager = mCharactereManager;
				lInPutManager.mTrackType = mTrackType;
				lInPutManager.mTrack = this;
				lInput.transform.localPosition = new Vector3(lInPutManager.mPosition * 1.88f ,0,0);
				lInput.transform.localScale = new Vector3(1,1,1);
				
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
					lInPutManagerCopy.mTrack = this;
					lInPutManager.AddInputManagerCopy(lInPutManagerCopy);

					mInputList.Add(lInPutManagerCopy.mPosition,lInPutManagerCopy);					
					lInputCopy.transform.localPosition = new Vector3(lInPutManagerCopy.mPosition * 1.88f ,0,0);
					lInputCopy.transform.localScale = new Vector3(1,1,1);
				}
			}
		}
	}
	
	public bool isValidCheck(InPutState _InPutState)
	{
		if(_InPutState == InPutState.Down)
		{
			int lCountDown = 0;
			foreach(InPutManager lInPutManager in mInputList.Values)
			{
				if(lInPutManager.mInPutState == InPutState.Down && lInPutManager.mIsCopy == false)
				{
					lCountDown++;
				}
			}
			if( lCountDown < mMinMax )
			{
				return true;
			}
			else
			{
				mSourceNot.Play();
				return false;
			}
		}
		else if(_InPutState == InPutState.Up)
		{
			int lCountUp = 0;
			foreach(InPutManager lInPutManager in mInputList.Values)
			{
				if(lInPutManager.mInPutState == InPutState.Up && lInPutManager.mIsCopy == false)
				{
					lCountUp++;
				}
			}
			if( lCountUp < mTopMax )
			{
				return true;
			}
			else
			{
				mSourceNot.Play();
				return false;
			}
		}
		else
		{
			return true;
		}
	}
}
