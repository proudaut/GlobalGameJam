using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Level : MonoBehaviour
{
	public int mDuration;
	public GameObject mCharater;
	public CharacterManager mCharactereManager;
	
	public Track mActionTrack;
	public Track mMoveTrack;
	
	
	
	public List<LevelElement> mLevelElements =  new List<LevelElement>();
	public Dictionary<LevelElement, GameObject> mLevelElementCreated = new Dictionary<LevelElement, GameObject>();
	
	
	public static int mCurrentScene = -1;
	public List<TextAsset> mLevelDescriptors = new List<TextAsset>();
	public List<TextAsset> mLevelConfigs = new List<TextAsset>();
	
	
	private bool mIsPlaying = false;
	
	public GameObject GroundHole; //GD
	public GameObject GroundR;//GR
	public GameObject GroundL;//GL
	public GameObject Ground;//GF
	
	public GameObject G;
	public GameObject GL;
	public GameObject GR;
	public GameObject GD;
	public GameObject GF;
	public GameObject spi;
	public GameObject mid;
	public GameObject low;
	public GameObject vc;
	public GameObject hc;
	
	
	private float mStartTime =0;
	private int mIndexAnimation = 0;
	
	
	
	void OnButtonClick(Button _button)
	{
		Debug.Log("CLICK");
		if(mIsPlaying == false)
		{
			Play();
		}
		else
		{
			Stop();
		}
	}
	
	
	public void Play()
	{
		StartCoroutine( ReadyStart());
	}
	
	IEnumerator ReadyStart()
	{
		MusicManager.PlayMusique(MusicID.Level2);
		
		mCharactereManager.mAnimation.Stop();
		mCharactereManager.mAnimation.gameObject.SampleAnimation(mCharactereManager.mAnimation["Run"].clip,0);
		mCharactereManager.mYPosition = 1;
		mCharater.transform.localPosition = new Vector3(0,3,0);
		mCharactereManager.mNewRealY = mCharater.transform.localPosition.y;
		
		
		yield return new WaitForSeconds(0.1f);
		
		mCharactereManager.mIsDead = false;
		mIsPlaying = true;


		mStartTime = Time.time -2;
		mIndexAnimation = 0;
		
		
	}
	
	
	public void Stop()
	{
		//Application.LoadLevel("LevelScene");
		
		mIsPlaying = false;
		MusicManager.PlayMusique(MusicID.Menu);
		mCharactereManager.mAnimation.Stop();
		mCharactereManager.mAnimation.gameObject.SampleAnimation(mCharactereManager.mAnimation["Run"].clip,0);
		mCharactereManager.mYPosition = 1;
		mCharater.transform.localPosition = new Vector3(0,3,0);
		mCharactereManager.mNewRealY = mCharater.transform.localPosition.y;
		
	}
	
	
	// Use this for initialization
	void Start () 
	{
		if(mCurrentScene == -1)
		{
			if(PlayerPrefs.HasKey("Level1"))
			{
				mCurrentScene = PlayerPrefs.GetInt("Level1");
			}
			else
			{
				mCurrentScene = 0;
			}
		}

		MusicManager.PlayMusique(MusicID.Menu);
		LoadLevel();
		LoadConfig();
		GenerateLevel();
	}
	
	void LoadConfig()
	{
		IDictionary lElementList = (IDictionary)Prime31.Json.jsonDecode(mLevelConfigs[mCurrentScene].text);
		
		mDuration = int.Parse(lElementList["Level_size"].ToString());	
		mActionTrack.mDuration = int.Parse(lElementList["Action_size"].ToString());
		mMoveTrack.mDuration = int.Parse(lElementList["Move_size"].ToString());
		
		mMoveTrack.mTopMax = int.Parse(lElementList["Jump_max"].ToString());
		mMoveTrack.mMinMax = int.Parse(lElementList["Slide_max"].ToString());
		
		mActionTrack.mTopMax =int.Parse(lElementList["Strike_max"].ToString()); 
		mActionTrack.mMinMax = int.Parse(lElementList["Prot_max"].ToString()); 
		
		
		mMoveTrack.Configure();
		mActionTrack.Configure();
		
		


	}
	
	void LoadLevel()
	{
		ArrayList lElementList = (ArrayList)Prime31.Json.jsonDecode(mLevelDescriptors[mCurrentScene].text);
		foreach(IDictionary lDic in lElementList)
		{
			LevelElement lLevelElement = new LevelElement(lDic);
			mLevelElements.Add(lLevelElement);
		}
	}
	
	const int TileOffset = 2;

	void GenerateLevel()
	{
		foreach(LevelElement lLevelElement in mLevelElements)
		{
			GameObject lInstanceElement = null;
			switch(lLevelElement.mLevelElementType)
			{
				case LevelElementType.G :  
				{
					lInstanceElement = Instantiate(G) as GameObject;
					break;
				}
				
				case LevelElementType.GL : 
				{
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundL) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GL) as GameObject;
					}
					break;
				}
				
				
				case LevelElementType.GR :  
				{
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundR) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GR) as GameObject;
					}
					break;
				}
				
				
				
				case LevelElementType.GD :  
				{
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundHole) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GD) as GameObject;
					}
					break;
				}
				
				case LevelElementType.GF :
				{
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(Ground) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GF) as GameObject;
					}
					break;
				}
				
				
				case LevelElementType.spi :  
				{
					lInstanceElement = Instantiate(spi) as GameObject;
					break;
				}
				
				case LevelElementType.mid : 
				{
					lInstanceElement = Instantiate(mid) as GameObject;
					break;
				}
				
				case LevelElementType.low :  
				{
					lInstanceElement = Instantiate(low) as GameObject;
					break;
				}
				
				case LevelElementType.vc :  
				{
					lInstanceElement = Instantiate(vc) as GameObject;
					break;
				}
				
				case LevelElementType.hc :
				{
					lInstanceElement = Instantiate(hc) as GameObject;
					break;
				}
			}
			lInstanceElement.transform.parent = this.transform;
			lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
			mLevelElementCreated.Add(lLevelElement, lInstanceElement);
		}
	}
	
	
	
	

	
	
	
	// Update is called once per frame
	void Update () 
	{
		if (mIsPlaying && !mCharactereManager.mIsDead)
		{
			mCharater.transform.Translate(Vector3.right * Time.deltaTime * -2);
			if(Time.time - mStartTime>0.99999)
			{
				mStartTime = Time.time;
				if(mIndexAnimation<mDuration)
				{
					if(mActionTrack.mInputList.ContainsKey(mIndexAnimation))
					{
						mActionTrack.mInputList[mIndexAnimation].Play(mIndexAnimation);
					}
					
					if(mMoveTrack.mInputList.ContainsKey(mIndexAnimation))
					{
						mMoveTrack.mInputList[mIndexAnimation].Play(mIndexAnimation);
					}
					else
					{
						mActionTrack.mCharactereManager.HandleIdleMovement(mIndexAnimation);
					}

					mIndexAnimation++;
				}
				else
				{
					Stop ();
					LoadNextScene();
				}
			}
		}
	}
	
	void LoadNextScene()
	{
		mCurrentScene++;
		PlayerPrefs.SetInt("Level1",mCurrentScene);
		Application.LoadLevel("LevelScene");
	}

}
