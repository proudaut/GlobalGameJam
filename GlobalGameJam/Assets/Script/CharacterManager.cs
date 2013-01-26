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
	
	
	public List<GameObject> mInstanciateSound = new List<GameObject>();
	
	public void HandleJump()
    {
		CleanSound();

		this.GetComponentInChildren<Animation>()["Jump"].layer = 1;
		this.GetComponentInChildren<Animation>().Play("Jump");
		mInstanciateSound.Add(Instantiate(mJumpSound) as GameObject);
	}
	
	
	public void HandleSlide()
    {
		CleanSound();

		
		this.GetComponentInChildren<Animation>()["Slide"].layer = 1;
		this.GetComponentInChildren<Animation>().Play("Slide");
		mInstanciateSound.Add(Instantiate(mSlideSound) as GameObject);
	}
	
	public void HandleIdleMovement()
    {
		CleanSound();

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
		List<GameObject> lRemoveList = new List<GameObject>();
		foreach(GameObject lGameObject in mInstanciateSound)
		{
			AudioSource lAudioSource = lGameObject.GetComponent<AudioSource>();
			if(!lAudioSource.isPlaying)
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
		/*
		Debug.Log("Enter collision " + other.gameObject.name); 
		if( (other.gameObject.name == "spi"|| other.gameObject.name == "Sphere") && mAsShield == false)
		{
			mLevel.Stop();
		}
		else if( (other.gameObject.name == "spi"|| other.gameObject.name == "Sphere") && mAsShield == false)
		{
			other.collider.enabled = false;
		}
		if( other.gameObject.name == "mid")
		{
			mLevel.Stop();
		}*/
		
		//this.animation.Stop("jump");
		
		//this.animation.Play("idle");
		//this.transform.localPosition = new Vector3(this.transform.localPosition.x,  other.gameObject.transform.localPosition.y+0.2f, 0);
    }
	/*
	void OnCollisionExit(Collision other) 
	{
       Debug.Log("Exit collision " + other.gameObject.name); 
    }

	
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log("Enter collision " + other.gameObject.name); 
    }
	void OnTriggerExit(Collider other) 
	{
       Debug.Log("Exit collision " + other.gameObject.name); 
    }
	*/
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
