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
		if ( _button == mButtonTop )
		{
			SetFrameButton(mButtonDown,1);
			SetFrameButton(mButtonMiddle,1);
			SetFrameButton(mButtonTop,1);
		}
		if ( _button == mButtonMiddle )
        {
			SetFrameButton(mButtonDown,1);
			SetFrameButton(mButtonMiddle,1);
			SetFrameButton(mButtonTop,1);
		}
		if ( _button == mButtonDown )
        {
			SetFrameButton(mButtonDown,1);
			SetFrameButton(mButtonMiddle,1);
			SetFrameButton(mButtonTop,1);
		}
	}
	
	
	void SetFrameButton(Button _Button, int _Frame)
	{
			_Button.mCurrentFrame = _Frame;
			_Button.mDefaultFrame = _Frame;
			_Button.mOverFrame = _Frame;
			mButtonMiddle.UpdateFrame();
	}
	
}
