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
	public bool fall  = false;

	public float mNewRealY;

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

		
		mMoveType = MoveType.Jump;
		
		mAnimation["Jump"].layer = 1;
		mAnimation.Play("Jump");
		mInstanciateSound.Add(Instantiate(mJumpSound) as GameObject);
		
		fall = false;
		StartCoroutine(ManageMove(_Position));
	}
	
	public void HandleSlide(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();

			
			mMoveType = MoveType.Slide;
			
			mAnimation["Slide"].layer = 1;
			mAnimation.Play("Slide");
			mInstanciateSound.Add(Instantiate(mSlideSound) as GameObject);
			
			fall = false;
			StartCoroutine(ManageMove(_Position));
		}
	}
	
	public void HandleIdleMovement(int _Position)
    {
		if(!mIsDead)
		{
			CleanSound();

			
			mMoveType = MoveType.Nothing;
			
			mAnimation["Run"].layer = 1;
			mAnimation.Play("Run");
			
			if(mIdleMovementSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleMovementSound) as GameObject);
			
			fall = false;
			StartCoroutine(ManageMove(_Position));
		}
	}

	
	public void HandleIdleAction(int _Position)
    {
		if(!mIsDead)
		{
			mActionType = ActionType.Nothing;
			
			CleanSound();

			if(mIdleActionSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleActionSound) as GameObject);
		}
	}
	
	public void HandleAttack(int _Position)
    {
		if(!mIsDead)
		{
			mActionType = ActionType.Attack;
			
			CleanSound();

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
			mActionType = ActionType.Shield;
			
			CleanSound();

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
			if(  (lLevelElement.mX == _Position))
			{
				if( (lLevelElement.mY == mYPosition -1) ||  (lLevelElement.mY == mYPosition) )
				{
					if(mLevel.mLevelElementCreated[lLevelElement] != null)
					{
						if(mLevel.mLevelElementCreated[lLevelElement].GetComponent<Animation>() != null)
						{
							mLevel.mLevelElementCreated[lLevelElement].GetComponent<Animation>().Play();
						}
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
		
		List<LevelElement> lLevelElemenCaseDown = new List<LevelElement>();
		foreach(LevelElement lLevelElement in mLevel.mLevelElements)
		{
			if( (lLevelElement.mX == _Position) && (lLevelElement.mY == mYPosition -1))
			{
				lLevelElemenCaseDown.Add(lLevelElement);
			}
		}
		
		
		ComplexElementLevel lComplexElementLevel = new ComplexElementLevel(lLevelElemenCaseCurrent);
		ComplexElementLevel lComplexElementLevelUp = new ComplexElementLevel(lLevelElemenCaseUp);
		ComplexElementLevel lComplexElementLevelDown = new ComplexElementLevel(lLevelElemenCaseDown);
		
		
		if(mActionType == ActionType.Nothing && mMoveType == MoveType.Nothing)  // walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.vc && !fall) || (lComplexElementLevel.mid && !fall ) || (lComplexElementLevel.GF) )
			{
				Debug.Log(lComplexElementLevel.spi+ " "  +lComplexElementLevel.vc+ " " + lComplexElementLevel.mid+ " "  + lComplexElementLevel.GF);
				StartCoroutine(Die());
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				yield break;
			}
				
				
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Nothing)  // Attack walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.mid && !fall) || (lComplexElementLevel.GF) )
			{
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
			
			if( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) )
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				yield break;
			}
			
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Nothing)  // Shield walk
		{
			if( (lComplexElementLevel.vc && !fall) || (lComplexElementLevel.mid && !fall  ) || (lComplexElementLevel.GF) )
			{
				Debug.Log("3");
				StartCoroutine(Die());
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				yield break;
			}
		}

	
		else if(mActionType == ActionType.Nothing && mMoveType == MoveType.Jump)  // Jump
		{
			/*if ((lComplexElementLevel.G || lComplexElementLevel.hc) && (lComplexElementLevel.spi) || (lComplexElementLevel.mid && !fall ) )
			{
				Debug.Log("1");
				StartCoroutine(Die());
				yield break;
			}
			*/
			
			if(lComplexElementLevel.GF &&  !fall)
			{	
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
			
			
			if(lComplexElementLevel.G || lComplexElementLevel.hc)  
			{
				mMoveType = MoveType.Nothing;
				CheckWalk(lComplexElementLevel, lComplexElementLevelDown,_Position);
				yield break;
			}
			
			
			if( (!(lComplexElementLevel.G || lComplexElementLevel.hc)) && lComplexElementLevelUp.spi)
			{
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
		
			
			
			if( (lComplexElementLevel.GR ||  (lComplexElementLevel.GD && !fall)) && !lComplexElementLevel.hc)
			{
				Debug.Log ("JUMP " + lComplexElementLevel.GR + " "+ lComplexElementLevelDown.GD);
				MoveUp();
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("JUMP1 FALL");
				MoveDown(_Position);
				yield break;
			}
			
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Jump)  // Attack Jump
		{
			if(lComplexElementLevel.GF &&  !fall)
			{	
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
			
			if(lComplexElementLevel.G || lComplexElementLevel.hc)  
			{
				mMoveType = MoveType.Nothing;
				CheckWalk(lComplexElementLevel,lComplexElementLevelDown,_Position);
				yield break;
			}
			
			/*if ((lComplexElementLevel.G) && (lComplexElementLevel.spi) || (lComplexElementLevel.mid && !fall ) )
			{
				StartCoroutine(Die());
				yield break;
			}*/
			
			if( (lComplexElementLevel.GR ||(lComplexElementLevel.GD && !fall)) )
			{
				MoveUp();
				yield break;
			}
			
			if( lComplexElementLevelDown.GL )
			{
				Debug.Log ("JUMP2 FALL");
				MoveDown(_Position);
				yield break;
			}

		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Jump)  // Shield Jump
		{
			/*if ((lComplexElementLevel.G || lComplexElementLevel.hc) && (lComplexElementLevel.mid && !fall ) )
			{
				StartCoroutine(Die());
				yield break;
			}*/
			
			if(lComplexElementLevel.GF &&  !fall)
			{	
				Debug.Log("2");
				StartCoroutine(Die());
				yield break;
			}
			
			if(lComplexElementLevel.G || lComplexElementLevel.hc)  
			{
				mMoveType = MoveType.Nothing;
				CheckWalk(lComplexElementLevel,lComplexElementLevelDown,_Position);
				yield break;
			}
			
			
			if( (lComplexElementLevel.GR ||(lComplexElementLevel.GD && !fall)) && !lComplexElementLevel.hc)
			{
				MoveUp();
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL ) && !lComplexElementLevelDown.hc)
			{
				Debug.Log ("JUMP3 FALL");
				MoveDown(_Position);
				yield break;
			}
		}
		
		
		
		
	
		else if(mActionType == ActionType.Nothing && mMoveType == MoveType.Slide)  // Slide
		{
			if(lComplexElementLevel.spi ||(lComplexElementLevel.low && !fall) || lComplexElementLevel.GF ||   (lComplexElementLevel.vc && !fall))
			{
				StartCoroutine(Die());
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL || (lComplexElementLevelDown.GD && !fall) ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("Slide FALL");
				MoveDown(_Position);
				yield break;
			}
			
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Slide)  // Attack Slide
		{
			if(lComplexElementLevel.spi || (lComplexElementLevel.low && !fall)|| lComplexElementLevel.GF)
			{
				StartCoroutine(Die());
				yield break;
			}
			
			if( lComplexElementLevelDown.GL ||(lComplexElementLevelDown.GD && !fall) )
			{
				Debug.Log ("Slide FALL");
				MoveDown(_Position);
				yield break;
			}
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Slide)  // Shield Slide
		{
			if( (lComplexElementLevel.low && !fall) || lComplexElementLevel.GF ||   (lComplexElementLevel.vc && !fall))
			{
				StartCoroutine(Die());
				yield break;
			}
			
			if(( lComplexElementLevelDown.GL || (lComplexElementLevelDown.GD && !fall)) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("Slide FALL");
				MoveDown(_Position);
				yield break;
			}
		}
	}
	
	

	void CheckWalk(ComplexElementLevel lComplexElementLevel,ComplexElementLevel lComplexElementLevelDown, int _Position)
	{
		mAnimation.Stop("Jump");
		mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,0);
		if(mActionType == ActionType.Nothing && mMoveType == MoveType.Nothing)  // walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.vc && !fall) || (lComplexElementLevel.mid && !fall ) || (lComplexElementLevel.GF) )
			{
				Debug.Log(lComplexElementLevel.spi+ " "  +lComplexElementLevel.vc+ " " + lComplexElementLevel.mid+ " "  + lComplexElementLevel.GF);
				StartCoroutine(Die());
				return;
			}
			
			if(( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				return;
			}	
		}
		else if(mActionType == ActionType.Attack && mMoveType == MoveType.Nothing)  // Attack walk
		{
			if( (lComplexElementLevel.spi) || (lComplexElementLevel.mid && !fall) || (lComplexElementLevel.GF) )
			{
				Debug.Log("2");
				StartCoroutine(Die());
				return;
			}
			
			if( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) )
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				return;
			}
			
		}
		else if(mActionType == ActionType.Shield && mMoveType == MoveType.Nothing)  // Shield walk
		{
			if( (lComplexElementLevel.vc && !fall) || (lComplexElementLevel.mid && !fall  ) || (lComplexElementLevel.GF) )
			{
				Debug.Log("3");
				StartCoroutine(Die());
				return;
			}
			
			if(( lComplexElementLevelDown.GL ||  (lComplexElementLevelDown.GD && !fall) ) && ! lComplexElementLevelDown.hc)
			{
				Debug.Log ("walk FALL");
				MoveDown(_Position);
				return;
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
		yield return new WaitForSeconds(0.2f);
		mAnimation.Stop();
		mAnimation.Play("Die");
		yield return new WaitForSeconds(0.5f);
		mLevel.Stop();
	}
	

	
	
	void MoveUp()
	{
		mYPosition++;
		mNewRealY = this.transform.localPosition.y + 2.0f;
		//this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 2.0f, 0);
	}
	
	void MoveDown(int _Position)
	{
		fall = true;
		
		Debug.Log("MoveDown()" + mYPosition );
		mYPosition--;
		

		this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 2.0f, 0);
		mNewRealY = this.transform.localPosition.y;
			
			
		if(mActionType == ActionType.Attack)
			mActionType = ActionType.Nothing;

		
		
		StartCoroutine(ManageMove(_Position));
	}
	
	
	// Use this for initialization
	void Start ()
	{
		mNewRealY = this.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mAnimation["Jump"].time > 0.5 &&  mAnimation.IsPlaying("Jump"))
		{
			mAnimation.Stop("Jump");
			mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,0);
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, mNewRealY, 0);
		}
	}
}
