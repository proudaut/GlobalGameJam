using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ActionType
{
	Nothing,
	Attack,
	Shield,
}
public enum MoveType
{
	Nothing,
	Slide,
	Jump,
}

public class CharacterManager : MonoBehaviour 
{
	public GameObject mJumpSound;
	public GameObject mAttackSound;
	public GameObject mIdleMovementSound;
	public GameObject mIdleActionSound;
	public GameObject mSlideSound;
	public GameObject mShieldSound;
	public ParticleSystem mExplosion;
	public Level mLevel;
	public Animation mAnimation;
	public bool mIsDead;
	public int mYPosition = 1;
	
	
	bool mAsShield = false;
	bool mIsGlissing = false;
	bool mIsJumping = false;
	bool mIsAttacking = false;
	

	ActionType mActionType = ActionType.Nothing;
	MoveType mMoveType = MoveType.Nothing;
	
	public List<GameObject> mInstanciateSound = new List<GameObject>();
	

	public void Explode()
	{
		mExplosion.Play();
	}
	
	public void ExplodeStop()
	{
		mExplosion.Stop();
	}
	
	public void HandleJump(int _Position)
    {
		Debug.Log("HandleJump");
		CleanSound();
		mIsGlissing = false;
		mIsJumping = true;
		mAnimation["Jump"].layer = 1;
		mAnimation.Play("Jump");
		mInstanciateSound.Add(Instantiate(mJumpSound) as GameObject);
		
		StartCoroutine(ManageMove(_Position));
	}
	
	public void HandleSlide(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();
			mIsGlissing = true;
			mIsJumping = false;
			mAnimation["Slide"].layer = 1;
			mAnimation.Play("Slide");
			mInstanciateSound.Add(Instantiate(mSlideSound) as GameObject);
			
			StartCoroutine(ManageMove(_Position));
		}
	}
	
	public void HandleIdleMovement(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();
			mIsJumping = false;
			mIsGlissing = false;
			mAnimation["Run"].layer = 1;
			mAnimation.Play("Run");
			
			if(mIdleMovementSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleMovementSound) as GameObject);
			
			StartCoroutine(ManageMove(_Position));
		}
	}

	
	public void HandleIdleAction(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = false;
			if(mIdleActionSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleActionSound) as GameObject);
		}
	}
	
	public void HandleAttack(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = false;
			mIsAttacking = true;
			mAnimation["Attack"].layer = 2;
			mAnimation.Play("Attack");
			mInstanciateSound.Add(Instantiate(mAttackSound) as GameObject);
			
			StartCoroutine(PerformAttack(_Position));
		}
	}
	
	
	public void HandleShield(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = true;
			mIsAttacking = false;
			mAnimation["Shield"].layer = 2;
			mAnimation.Play("Shield");
			mInstanciateSound.Add(Instantiate(mShieldSound) as GameObject);
		}
	}
	
	
	
	IEnumerator PerformAttack(int _Position)
	{
		yield return new WaitForSeconds(0.5f);
		foreach(LevelElement lLevelElement in mLevel.mLevelElementCreated.Keys)
		{
			if( (lLevelElement.mX == _Position -1) ||  (lLevelElement.mX == _Position) || (lLevelElement.mX == _Position +1) )
			{
				if( (lLevelElement.mY == mYPosition -1) ||  (lLevelElement.mY == mYPosition) || (lLevelElement.mY == mYPosition +1) )
				{
					if(mLevel.mLevelElementCreated[lLevelElement].GetComponent<Animation>() != null)
					{
						mLevel.mLevelElementCreated[lLevelElement].GetComponent<Animation>().Play();
					}
				}
			}
		}
	}
	
	IEnumerator ManageMove(int _Position)
	{
		Debug.Log("Position : " + _Position + " " + mYPosition);
		List<LevelElement> lLevelElemenCaseCurrent = new List<LevelElement>();
		foreach(LevelElement lLevelElement in mLevel.mLevelElements)
		{
			if( (lLevelElement.mX == _Position) &&  (lLevelElement.mY == mYPosition))
			{
				Debug.Log("Add " + lLevelElement.mLevelElementType);
				lLevelElemenCaseCurrent.Add(lLevelElement);
			}
		}
		
		List<LevelElement> lLevelElemenCaseUp = new List<LevelElement>();
		foreach(LevelElement lLevelElement in mLevel.mLevelElements)
		{
			if( (lLevelElement.mX == _Position) && (lLevelElement.mY == mYPosition +1))
			{
				lLevelElemenCaseUp.Add(lLevelElement);
			}
		}
		
		
		ComplexElementLevel lComplexElementLevel = new ComplexElementLevel(lLevelElemenCaseCurrent);
		ComplexElementLevel lComplexElementLevelUp = new ComplexElementLevel(lLevelElemenCaseUp);
		
		
		
		if(mActionType == ActionType.Nothing && mMoveType == MoveType.Nothing)  // walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.vc ) || (lComplexElementLevel.mid ) || (lComplexElementLevel.GF) )
			{
				Debug.Log(lComplexElementLevel.spi+ " "  +lComplexElementLevel.vc+ " " + lComplexElementLevel.mid+ " "  + lComplexElementLevel.GF);
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Nothing)  // Attack walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.mid ) || (lComplexElementLevel.GF) )
			{
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Nothing)  // Shield walk
		{
			if( (lComplexElementLevel.vc ) || (lComplexElementLevel.mid ) || (lComplexElementLevel.GF) )
			{
				Debug.Log("3");
				StartCoroutine(Die());
				yield break;
			}
		}
	
		
		
		
	
		else if(mActionType == ActionType.Nothing && mMoveType == MoveType.Jump)  // Jump
		{
			if ((lComplexElementLevel.G || lComplexElementLevel.hc) && (lComplexElementLevel.spi) || (lComplexElementLevel.mid) )
			{
				StartCoroutine(Die());
				yield break;
			}
			if(lComplexElementLevelUp.spi)
			{
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Jump)  // Attack Jump
		{
			if ((lComplexElementLevel.G) && (lComplexElementLevel.spi) || (lComplexElementLevel.mid) )
			{
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Jump)  // Shield Jump
		{
			if ((lComplexElementLevel.G || lComplexElementLevel.hc) && (lComplexElementLevel.mid) )
			{
				StartCoroutine(Die());
				yield break;
			}
		}
		
		
		
		
	
		else if(mActionType == ActionType.Nothing && mMoveType == MoveType.Slide)  // Slide
		{
			if(lComplexElementLevel.spi || lComplexElementLevel.low || lComplexElementLevel.GF ||   lComplexElementLevel.vc)
			{
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Slide)  // Attack Slide
		{
			if(lComplexElementLevel.spi || lComplexElementLevel.low || lComplexElementLevel.GF)
			{
				StartCoroutine(Die());
				yield break;
			}
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Slide)  // Shield Slide
		{
			if(lComplexElementLevel.low || lComplexElementLevel.GF ||   lComplexElementLevel.vc)
			{
				StartCoroutine(Die());
				yield break;
			}
		}
	}
	
	

		
	
	
	private void CleanSound()
	{
		List<GameObject> lRemoveList = new List<GameObject>();
		foreach(GameObject lGameObject in mInstanciateSound)
		{
			AudioSource lAudioSource = lGameObject.GetComponent<AudioSource>();
			if(lAudioSource!= null && !lAudioSource.isPlaying)
			{
				lRemoveList.Add(lGameObject);
			}
		}
		foreach(GameObject lGameObject in lRemoveList)
		{
			mInstanciateSound.Remove(lGameObject);
			Destroy(lGameObject);
		}
	}
	
	IEnumerator Die()
	{
		mIsDead = true;
		yield return true;
		mAnimation.Stop();
		mAnimation.Play("Die");
		yield return new WaitForSeconds(0.5f);
		mLevel.Stop();
	}
	
	/*
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log(other.gameObject.name);
		{
			if( other.gameObject.name == "spi(Clone)" && mAsShield == false)
			{
				StartCoroutine(Die());
			}
			if( other.gameObject.name == "mid(Clone)" || other.gameObject.name == "Ground(Clone)" || other.gameObject.name == "ground(Clone)"|| other.gameObject.name == "GF(Clone)")
			{
				StartCoroutine(Die());
			}
			
			if( other.gameObject.name == "low(Clone)" &&  mIsGlissing  == true)
			{
				StartCoroutine(Die());
			}
			
			if( other.gameObject.name == "G(Clone)")
			{
				float lTime = mAnimation["Jump"].time;
				mAnimation.Stop("Jump");
				mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,lTime);
				mAnimation.Play("Run");
			}
			
			if( other.gameObject.name == "GR(Clone)")
			{
				float lTime = mAnimation["Jump"].time;
				mAnimation.Stop("Jump");
				mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,lTime);
				mAnimation.Play("Run");
				mYPosition++;
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y +3.2f, 0);
			}
			
			if(( other.gameObject.name == "GL(Clone)" || other.gameObject.name == "Ground_L(Clone)" ||  other.gameObject.name == "Ground_Hole(Clone)" || other.gameObject.name == "GD(Clone)") && !mIsJumping)
			{
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y+1.2f, 0);
				mYPosition--;
			}
			else if(( other.gameObject.name == "GL(Clone)" || other.gameObject.name == "Ground_L(Clone)" ||  other.gameObject.name == "Ground_Hole(Clone)" || other.gameObject.name == "GD(Clone)") && mIsJumping)
			{
				float lTime = mAnimation["Jump"].time;
				mAnimation.Stop("Jump");
				mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,lTime);
				mAnimation.Play("Run");
				
				mYPosition++;
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y +3.2f, 0);
			}
		}
    }
	 */
	
	
	void MoveUp()
	{
	}
	
	void MoveDown()
	{
	}
	
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
