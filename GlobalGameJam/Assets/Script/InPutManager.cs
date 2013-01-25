using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum InPutState
{
    Down,
    Middle,
    Up,
}


public class InPutManager : MonoBehaviour 
{
	[SerializeField]
	private InPutState mInPutStateField = InPutState.Middle;
	public InPutState mInPutState
	{
		set
		{ 
			mInPutStateField = value;
			switch(mInPutStateField)
			{
				case InPutState.Down : 
				{
					SetFrameButton(mButtonDown,2);
					SetFrameButton(mButtonMiddle,1);
					SetFrameButton(mButtonTop,1);
					break;
				}
				case InPutState.Middle : 
				{
					SetFrameButton(mButtonDown,1);
					SetFrameButton(mButtonMiddle,2);
					SetFrameButton(mButtonTop,1);
					break;
				}
				case InPutState.Up : 
				{
					SetFrameButton(mButtonDown,1);
					SetFrameButton(mButtonMiddle,1);
					SetFrameButton(mButtonTop,2);
					break;
				}
				
			}
			
			
			
			foreach(InPutManager lInPutManager in mInPutManagerCopy)
			{
				lInPutManager.mInPutState = value;
			}
		}
		get
		{
			return mInPutStateField;
		}
	}
	
	
	
	public int mPosition=0;
	public Animation mAnimation
	{
		get{ return mAnimation;}
	}
	public Button mButtonTop;
	public Button mButtonMiddle;
	public Button mButtonDown;
	public bool mIsCopy = false;
	public List<InPutManager> mInPutManagerCopy = new List<InPutManager>();
	
	// Use this for initialization
	public void AddInputManagerCopy (InPutManager _InPutManager)
	{
		mInPutManagerCopy.Add(_InPutManager);
	}
	
	// Use this for initialization
	void Start ()
	{
	
	}

	void OnButtonClick(Button _button)
	{
		if( !mIsCopy )
		{
			if ( _button == mButtonTop )
			{
				mInPutState = InPutState.Up;
			}
			else if ( _button == mButtonMiddle )
	        {
				mInPutState = InPutState.Middle;
			}
			else if ( _button == mButtonDown )
	        {
				mInPutState = InPutState.Down;
			}
		}
	}
	
	
	
	
	
	void SetFrameButton(Button _Button, int _Frame)
	{
			_Button.mCurrentFrame = _Frame;
			_Button.mDefaultFrame = _Frame;
			_Button.mOverFrame = _Frame;
			_Button.UpdateFrame();
	}
	
}
