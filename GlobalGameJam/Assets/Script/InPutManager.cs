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
	public TrackType mTrackType;
	public CharacterManager mCharactereManager;
	
	[SerializeField]
	private InPutState mInPutStateField = InPutState.Middle;
	public InPutState mInPutState
	{
		set
		{ 
			mInPutStateField = value;
			UpdateButton();
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
		UpdateButton();
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
			Play();
		}
	}
	
	
	public void Play()
	{
		if(mTrackType == TrackType.Action)
		{
			switch (mInPutState)
			{
				case InPutState.Down : mCharactereManager.HandleShield(); break;
				case InPutState.Up : mCharactereManager.HandleAttack(); break;
				//case InPutState.Middle : mCharactereManager.HandleIdle();break;
			}
		}
		else
		{
			switch (mInPutState)
			{
				case InPutState.Down : mCharactereManager.HandleSlide(); break;
				case InPutState.Up : mCharactereManager.HandleJump();  break;
				case InPutState.Middle : mCharactereManager.HandleIdle();break;
			}
		}
	}
	
	
	void UpdateButton()
	{
		int lEnable = 2;
		int lDisable = 1;
		if(mIsCopy)
		{
			lEnable = 4;
			lDisable = 3;
		}
		
		switch(mInPutStateField)
		{
			case InPutState.Down : 
			{
				SetFrameButton(mButtonDown,lEnable);
				SetFrameButton(mButtonMiddle,lDisable);
				SetFrameButton(mButtonTop,lDisable);
				break;
			}
			case InPutState.Middle : 
			{
				SetFrameButton(mButtonDown,lDisable);
				SetFrameButton(mButtonMiddle,lEnable);
				SetFrameButton(mButtonTop,lDisable);
				break;
			}
			case InPutState.Up : 
			{
				SetFrameButton(mButtonDown,lDisable);
				SetFrameButton(mButtonMiddle,lDisable);
				SetFrameButton(mButtonTop,lEnable);
				break;
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
