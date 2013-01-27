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
			Play(0);
		}
	}
	
	
	public void Play(int _Position)
	{
		if(mTrackType == TrackType.Action)
		{
			switch (mInPutState)
			{
				case InPutState.Down : mCharactereManager.HandleShield(); break;
				case InPutState.Up : mCharactereManager.HandleAttack(_Position); break;
				case InPutState.Middle : mCharactereManager.HandleIdleAction();break;
			}
		}
		else
		{
			switch (mInPutState)
			{
				case InPutState.Down : mCharactereManager.HandleSlide(); break;
				case InPutState.Up : mCharactereManager.HandleJump();  break;
				case InPutState.Middle : mCharactereManager.HandleIdleMovement();break;
			}
		}
	}
	
	
	void UpdateButton()
	{
		
		int lDisable = 1;
		int lEnable = 2;
		int lEnableTop = 3;
		int lEnableDown = 4;
		int lDisableEmpty = 9;
		
		if(mIsCopy)
		{
			lDisable = 9;
			lEnableTop = 3;
			lEnableDown = 4;
		}
		
		if(mTrackType == TrackType.Action)
		{
			lDisable = 5;
			lEnable = 6;
			lEnableTop = 7;
			lEnableDown = 8;
			lDisableEmpty = 9;

			if(mIsCopy)
			{
				lDisable = 9;
				lEnableTop = 7;
				lEnableDown = 8;
			}
		}
		
		
		
		switch(mInPutStateField)
		{
			case InPutState.Down : 
			{
				SetFrameButton(mButtonDown,lEnable,lEnableDown);
				SetFrameButton(mButtonMiddle,lDisable, lDisableEmpty);
				SetFrameButton(mButtonTop,lDisable,lDisableEmpty);
				break;
			}
			case InPutState.Middle : 
			{
				SetFrameButton(mButtonDown,lDisable,lDisableEmpty);
				SetFrameButton(mButtonMiddle,lEnable,lDisableEmpty);
				SetFrameButton(mButtonTop,lDisable,lDisableEmpty);
				break;
			}
			case InPutState.Up : 
			{
				SetFrameButton(mButtonDown,lDisable,lDisableEmpty);
				SetFrameButton(mButtonMiddle,lDisable,lDisableEmpty);
				SetFrameButton(mButtonTop,lEnable,lEnableTop);
				break;
			}
			
		}
	}
	
	
	
	void SetFrameButton(Button _Button, int _Frame,  int _Frame2)
	{
			_Button.mCurrentFrame = _Frame;
			_Button.mDefaultFrame = _Frame;
			_Button.mOverFrame = _Frame;
			_Button.UpdateFrame();
		
		if(mIsCopy)
		{
			_Button.mCurrentFrame = _Frame2;
			_Button.mDefaultFrame = _Frame2;
			_Button.mOverFrame = _Frame2;
			_Button.UpdateFrame();
		}
	}
	
}
