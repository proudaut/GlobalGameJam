using UnityEngine;
using System.Collections;

using pumpkin.display;

[RequireComponent(typeof(InteractiveMovieClipBehaviour))]
[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Tekken/Button")]
public class Button : MonoBehaviour
{
	public GameObject mTarget;
	public GameObject mTargetDrag;
	
	public AudioSource mSoundClic;
	public AudioSource mSoundOver;
	public string mMovieClipButtonName = "button";
	
	public bool mDelayPress = true;
	
	public int mDefaultFrame = 1;
	public int mOverFrame = 2;
	public int mCurrentFrame = 1;
	public int mDisabledFrame = -1;
	
	public Color mHoverColor = Color.white;
	public Color mPressedColor = Color.grey;
	public Color mDisabledColor = Color.grey;
	public Color mDefaultColor = Color.white;
	
	public Vector2 mDelta = Vector2.zero;
	
	public Vector2 mDragDelta = Vector2.zero;
	
	
	public int mTag;
	
	private bool isEnabled = true;
	
	public bool mJustClick = false;
	
	public bool mIsEnabled
	{
		get
		{
			return isEnabled;
		}
		set
		{
			isEnabled = value;
			if ( mButtonScript != null )
			{
				mButtonScript.isEnabled = isEnabled;
			}
			if ( this.collider != null )
			{
				this.collider.enabled = isEnabled;
			}
			if ( mDisabledFrame > 0 )
			{
				UpdateFrame();
			}
		}
	}
	
	public string mButtonTitle
    {
		get
		{
            if (mLabelValue != null)
            {
                return mLabelValue[0];
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (mLabelValue != null && mLabelValue.Length > 0)
            {
                mLabelValue[0] = value;
            }
            else
            {
                Debug.Log("error " + gameObject.name);
            }
        }
    }

    public string[] mLabelValue;
    public UILabel[] mLabelList;

    public bool mManageOverButtonState = true;

    public MovieClip mMovieClipButton;

    private InteractiveMovieClipBehaviour mMovieClipBehaviour;
    private UIButton mButtonScript;

    private bool mOver;

    void Awake()
    {
        if (mLabelList == null || mLabelList.Length == 0)
        {
            mLabelList = gameObject.GetComponentsInChildren<UILabel>();
        }
        if (mLabelValue == null || mLabelValue.Length == 0)
        {
            mLabelValue = new string[mLabelList.Length];
        }
    }

    public void GetMovieClip()
	{
		mMovieClipBehaviour = GetComponent<InteractiveMovieClipBehaviour>();

        if (mMovieClipBehaviour != null)
        {
            if (mMovieClipButtonName.Length > 0)
            {
                mMovieClipButton = mMovieClipBehaviour.movieClip.getChildByName<MovieClip>(mMovieClipButtonName);
            }
            else
            {
                mMovieClipButton = mMovieClipBehaviour.movieClip;
            }

            if (mMovieClipButton != null)
            {
                mMovieClipButton.gotoAndStop(mDefaultFrame);
            }
            else
            {
                //Debug.LogWarning("Movie clip " + mMovieClipButtonName + " not found !");
            }
        }
	}
	
    void Start()
    {
        GetMovieClip ();
		
        mOver = false;

        if (gameObject.GetComponent<UIButtonMessage>() == null)
        {
            UIButtonMessage messageScript = gameObject.AddComponent<UIButtonMessage>();
            messageScript.trigger = UIButtonMessage.Trigger.OnPress;
            messageScript.functionName = "OnButton_Clicked";
            messageScript.target = this.gameObject;

            messageScript = gameObject.AddComponent<UIButtonMessage>();
            messageScript.trigger = UIButtonMessage.Trigger.OnRelease;
            messageScript.functionName = "OnButton_Up";
            messageScript.target = this.gameObject;

            messageScript = gameObject.AddComponent<UIButtonMessage>();
            messageScript.trigger = UIButtonMessage.Trigger.OnMouseOver;
            messageScript.functionName = "OnButton_Enter";
            messageScript.target = this.gameObject;

            messageScript = gameObject.AddComponent<UIButtonMessage>();
            messageScript.trigger = UIButtonMessage.Trigger.OnMouseOut;
            messageScript.functionName = "OnButton_Leave";
            messageScript.target = this.gameObject;

        }
        
		mButtonScript = gameObject.GetComponent<UIButton>();
		if ( mButtonScript == null )
		{
        	mButtonScript = gameObject.AddComponent<UIButton>();
			// Find in labels if a tween color is to be found : modify color of that label only (in case of subtitles)
			foreach ( UILabel label in mLabelList )
			{
				if(label != null)
				{
					TweenColor tween = label.GetComponent<TweenColor>();
					if ( tween != null )
					{
						mButtonScript.tweenTarget = label.gameObject;
						break;
					}	
				}
			}
		}

        mButtonScript.hover = mHoverColor;
        mButtonScript.disabledColor = mDisabledColor;
        mButtonScript.pressed = mPressedColor;
		mButtonScript.defaultColor = mDefaultColor;
		
        mButtonScript.UpdateColor(isEnabled, true);

        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null)
        {
            if (mainCamera.GetComponent<UICamera>() == null)
            {
                mainCamera.AddComponent<UICamera>();
            }
        }

        this.OnButton_Leave();
    }

    // Update is called once per frame
    void Update()
    {
    }
	
	public void SetHidden(bool _hidden)
	{
		if ( collider != null )
		{
			collider.enabled = !_hidden;
		}
		renderer.enabled = !_hidden;
	}


	
	public void SwitchFrame()
	{
		if ( mIsEnabled == false )
		{
			return;
		}
		
        if (mMovieClipButton != null)
        {
			if(mCurrentFrame == mDefaultFrame)
			{
				mCurrentFrame = mOverFrame;
			}
			else
			{
				mCurrentFrame = mDefaultFrame;
			}
			mMovieClipButton.gotoAndStop(mCurrentFrame);
		}
	}
	
    public void UpdateFrame()
    {
		if(mButtonScript!=null)
		{
		  	mButtonScript.hover = mHoverColor;
	        mButtonScript.disabledColor = mDisabledColor;
	        mButtonScript.pressed = mPressedColor;
			mButtonScript.defaultColor = mDefaultColor;
			mButtonScript.UpdateColor(isEnabled, true);
		}
        if (mMovieClipButton != null)
        {
			if ( mIsEnabled == false && mDisabledFrame > 0 )
			{
				mMovieClipButton.gotoAndStop(mDisabledFrame);
			}
			else
			{
            	mMovieClipButton.gotoAndStop(mOver ? mOverFrame : mDefaultFrame);
			}
        }
    }

    void OnButton_Clicked()
    {
		if ( mIsEnabled == false )
		{
			return;
		}
		
		mDragDelta = Vector2.zero;
		
		if ( mJustClick == false )
		{
			if (mManageOverButtonState && mMovieClipButton != null)
	        {
	            mMovieClipButton.gotoAndStop(mOverFrame);
	        }
			/*if ( mSoundClic != null && GameConf.IsSoundActive )
	        {
	            DeviceManager.PlaySound(mSoundClic);
			}*/
		}
		
		if ( mTarget != null )
		{
			mTarget.SendMessage("OnButtonPressed", this, SendMessageOptions.DontRequireReceiver);
		}
    }
	
	void OnButton_DragEnd()
	{
		if ( mIsEnabled == false )
		{
			return;
		}
		
		if ( mTargetDrag != null )
		{
			mTargetDrag.SendMessage("OnButtonDragEnd", mDelta, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnButton_Drag(Vector2 delta)
	{
		if ( mIsEnabled == false )
		{
			return;
		}
		
		if ( mTargetDrag != null )
        {
			mDelta = delta;
			mDragDelta += delta;
            mTargetDrag.SendMessage("OnButtonDrag", mDelta, SendMessageOptions.DontRequireReceiver);
        }
	}

    void OnButton_Up()
    {
		if ( mIsEnabled == false )
		{
			return;
		}
		
		if ( mJustClick == false )
		{
	        if (mManageOverButtonState && mMovieClipButton != null)
	        {
	            mMovieClipButton.gotoAndStop(mDefaultFrame);
	        }
		}
		
		if ( mTargetDrag != null )
		{
			mTargetDrag.SendMessage("OnButtonDragEnd", mDelta, SendMessageOptions.DontRequireReceiver);
		}
		

		if ( mDragDelta.sqrMagnitude * transform.localScale.x > 10 )
		{
			return;
		}
		
        if (mTarget != null)
        {
            mTarget.SendMessage("OnButtonClick", this);
        }
    }

    void OnButton_Enter()
    {
		mOver = true;
		if ( mJustClick || mIsEnabled == false )
		{
			return;
		}
        
		/*if ( mSoundOver != null && GameConf.IsSoundActive )
        {
            DeviceManager.PlaySound(mSoundOver);
		}*/
		
		if (mManageOverButtonState && mMovieClipButton != null)
        {
            mMovieClipButton.gotoAndStop(mOverFrame);
        }
		
        if (mTarget != null)
        {
        	mTarget.SendMessage("OnButtonEnter", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnButton_Leave()
    {
		mOver = false;
		
		if ( mJustClick || mIsEnabled == false )
		{
			return;
		}
		
		if (mManageOverButtonState && mMovieClipButton != null)
        {
            mMovieClipButton.gotoAndStop(mDefaultFrame);
        }
		
        if (mTarget != null)
        {
            mTarget.SendMessage("OnButtonLeave", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
