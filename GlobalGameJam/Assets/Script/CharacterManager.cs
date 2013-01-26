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
	public Level mLevel;
	
	bool mAsShield = false;
	bool mIsAttacking = true;
	bool mAscollade = false;
	
	public List<GameObject> mInstanciateSound = new List<GameObject>();
	
	public void HandleJump()
    {
		Debug.Log("HandleJump");
		CleanSound();
		this.GetComponentInChildren<Animation>()["Jump"].speed = 1;
		this.GetComponentInChildren<Animation>()["Jump"].layer = 1;
		this.GetComponentInChildren<Animation>().Play("Jump");
		mInstanciateSound.Add(Instantiate(mJumpSound) as GameObject);
	}
	
	
	public void HandleSlide()
    {
		CleanSound();
		Debug.Log("HandleSlide");
		this.GetComponentInChildren<Animation>()["Slide"].layer = 1;
		this.GetComponentInChildren<Animation>().Play("Slide");
		mInstanciateSound.Add(Instantiate(mSlideSound) as GameObject);
	}
	
	public void HandleIdleMovement()
    {
		CleanSound();
		Debug.Log("HandleIdleMovement");
		this.GetComponentInChildren<Animation>()["Run"].layer = 1;
		this.GetComponentInChildren<Animation>().Play("Run");
		
		
		mInstanciateSound.Add(Instantiate(mIdleMovementSound) as GameObject);
	}
	
	public void HandleIdleAction()
    {
		CleanSound();
		mAsShield = false;
		mInstanciateSound.Add(Instantiate(mIdleActionSound) as GameObject);
	}
	
	
	public void HandleAttack()
    {
		CleanSound();
		mAsShield = false;
		this.GetComponentInChildren<Animation>()["Attack"].layer = 2;
		this.GetComponentInChildren<Animation>().Play("Attack");
		mInstanciateSound.Add(Instantiate(mAttackSound) as GameObject);
	}
	

	
	public void HandleShield()
    {
		CleanSound();
		mAsShield = true;
		this.GetComponentInChildren<Animation>()["Shield"].layer = 2;
		this.GetComponentInChildren<Animation>().Play("Shield");
		mInstanciateSound.Add(Instantiate(mShieldSound) as GameObject);
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
	
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log(other.gameObject.name);
		//if( mAscollade == false) 
		{
			if( other.gameObject.name == "spi(Clone)" && mAsShield == false)
			{
				mLevel.Stop();
			}
			if( other.gameObject.name == "mid(Clone)")
			{
				mLevel.Stop();
			}
			
			if( other.gameObject.name == "GR(Clone)")
			{
				//mAscollade = true;
				float lTime = this.GetComponentInChildren<Animation>()["Jump"].time;
				this.GetComponentInChildren<Animation>().Stop("Jump");
				
				
				this.GetComponentInChildren<Animation>().gameObject.SampleAnimation(this.GetComponentInChildren<Animation>()["Run"].clip,lTime);
				
		
				//this.GetComponentInChildren<Animation>()["Run"].time = lTime;
				this.GetComponentInChildren<Animation>().Play("Run");
				//this.GetComponentInChildren<Animation>()["Run"].time = lTime;

		
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y +3.2f, 0);
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
		if(this.GetComponentInChildren<Animation>()["Jump"].time>0.8)
		{
			this.GetComponentInChildren<Animation>()["Jump"].speed = 1;
		}
	}
}
