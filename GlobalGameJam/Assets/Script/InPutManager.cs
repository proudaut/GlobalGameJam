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
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
