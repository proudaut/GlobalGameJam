using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	
	bool mAsShield = false;
	bool mIsGlissing = true;
	bool mIsAttacking = true;
	bool mAscollade = false;


	
	
	public List<GameObject> mInstanciateSound = new List<GameObject>();
	
	public void HandleJump()
    {
		Debug.Log("HandleJump");
		CleanSound();
		mIsGlissing = false;
		
		mAnimation["Jump"].layer = 1;
		mAnimation.Play("Jump");
		mInstanciateSound.Add(Instantiate(mJumpSound) as GameObject);
	}
	
	public void Explode()
	{
		mExplosion.Play();
	}
	
	public void ExplodeStop()
	{
		mExplosion.Stop();
	}
	
	
	public void HandleSlide()
    {
		if(!mIsDead)
		{
			CleanSound();
			mIsGlissing = true;
	
			mAnimation["Slide"].layer = 1;
			mAnimation.Play("Slide");
			mInstanciateSound.Add(Instantiate(mSlideSound) as GameObject);
		}
	}
	
	public void HandleIdleMovement()
    {
		if(!mIsDead)
		{
			CleanSound();
			
			mIsGlissing = false;
			mAnimation["Run"].layer = 1;
			mAnimation.Play("Run");
			
			if(mIdleMovementSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleMovementSound) as GameObject);
		}
	}
	
	public void HandleIdleAction()
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = false;
			if(mIdleActionSound!= null)
			mInstanciateSound.Add(Instantiate(mIdleActionSound) as GameObject);
		}
	}
	
	
	public void HandleAttack()
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = false;
			mAnimation["Attack"].layer = 2;
			mAnimation.Play("Attack");
			mInstanciateSound.Add(Instantiate(mAttackSound) as GameObject);
		}
	}
	

	
	public void HandleShield()
    {
		if(!mIsDead)
		{
			CleanSound();
			mAsShield = true;
			mAnimation["Shield"].layer = 2;
			mAnimation.Play("Shield");
			mInstanciateSound.Add(Instantiate(mShieldSound) as GameObject);
		}
	}
	
	

		
	
	
	private void CleanSound()
	{
		mAscollade = false;
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
		mAnimation.Stop();
		yield return new WaitForSeconds(0.1f);
		mAnimation.Play("Die");
		yield return new WaitForSeconds(1);
		mLevel.Stop();
	}
	
	
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log(other.gameObject.name);
		//if( mAscollade == false) 
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
				//mAscollade = true;
				float lTime = mAnimation["Jump"].time;
				mAnimation.Stop("Jump");
				mAnimation.gameObject.SampleAnimation(mAnimation["Run"].clip,lTime);
				mAnimation.Play("Run");
				
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y +3.2f, 0);
			}
			
			if( other.gameObject.name == "GL(Clone)" || other.gameObject.name == "Ground_L(Clone)" ||  other.gameObject.name == "Ground_Hole(Clone)")
			{
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y+1.2f, 0);
			}
			
		}

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
