using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Level : MonoBehaviour
{
	const int TileOffset = 2;
	const int LevelSizeMax = 18;
	
	public int mCurrentScene = -1;
	public int mDuration;
	public string mName = "";
	public string mSound = "";
	public GameObject mCharater;
	public CharacterManager mCharactereManager;
	
	public Track mActionTrack;
	public Track mMoveTrack;
	
	public Button mButtonMenu;
	public Button mButtonPlay;
	
	public List<LevelElement> mLevelElements =  new List<LevelElement>();
	public Dictionary<LevelElement, GameObject> mLevelElementCreated = new Dictionary<LevelElement, GameObject>();
	
	

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
		if(_button == mButtonPlay)
		{
			if(mIsPlaying == false)
			{
				Play();
			}
			else
			{
				Stop();
			}
		}
		else if(_button == mButtonMenu)
		{
			BackHome();
		}
	}
	
	
	void BackHome()
	{
		mCurrentScene = 0; 
		PlayerPrefs.SetInt("Level1",0);
		Application.LoadLevel("TitleScreen");
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
		mCharater.transform.localPosition = new Vector3(0,3,mCharater.transform.localPosition.z);
		mCharactereManager.mNewRealY = mCharater.transform.localPosition.y;
		
		yield return new WaitForSeconds(0.1f);
		
		mCharactereManager.mIsDead = false;
		mIsPlaying = true;
		mStartTime = Time.time -2;
		mIndexAnimation = 0;
	}
	
	
	public void Stop()
	{
		mIsPlaying = false;
		MusicManager.PlayMusique(MusicID.Menu);
		mCharactereManager.mAnimation.Stop();
		mCharactereManager.mAnimation.gameObject.SampleAnimation(mCharactereManager.mAnimation["Run"].clip,0);
		mCharactereManager.mYPosition = 1;
		mCharater.transform.localPosition = new Vector3(-4,3,mCharater.transform.localPosition.z);
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
		
		Physics.gravity = new Vector3(0, -40.0f, 0);
		
		mDuration = int.Parse(lElementList["Level_size"].ToString());
		mActionTrack.mDuration = int.Parse(lElementList["Action_size"].ToString());
		mMoveTrack.mDuration = int.Parse(lElementList["Move_size"].ToString());
		mMoveTrack.mTopMax = int.Parse(lElementList["Jump_max"].ToString());
		mMoveTrack.mMinMax = int.Parse(lElementList["Slide_max"].ToString());
		mActionTrack.mTopMax =int.Parse(lElementList["Prot_max"].ToString()); 
		mActionTrack.mMinMax = int.Parse(lElementList["Strike_max"].ToString());
			
		if(lElementList.Contains("Level_name"))
		{
			mName = lElementList["Level_name"].ToString();
		}
		if(lElementList.Contains("Level_sound"))
		{
			mSound = lElementList["Level_sound"].ToString();
		}
		if(lElementList.Contains("Jump_sound"))
		{
			mMoveTrack.mTopSound = lElementList["Jump_sound"].ToString();
		}
		if(lElementList.Contains("Slide_sound"))
		{
			mMoveTrack.mMinSound = lElementList["Slide_sound"].ToString();
		}
		if(lElementList.Contains("Prot_sound"))
		{
			mActionTrack.mTopSound =lElementList["Prot_sound"].ToString(); 
		}
		if(lElementList.Contains("Strike_sound"))
		{
			mActionTrack.mMinSound = lElementList["Strike_sound"].ToString(); 
		}

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
		
		for(int i=mDuration; i< mDuration + (LevelSizeMax -mDuration) ; i++)
		{
			GameObject lInstanceElement = Instantiate(Ground) as GameObject;
			lInstanceElement.transform.parent = this.transform;
			lInstanceElement.transform.localPosition = new Vector3(i*TileOffset,0*TileOffset,0);
		}
	}

	
	
	// Update is called once per frame
	void Update () 
	{
		if(!mIsPlaying)
		{
			if(mCharater.transform.localPosition.x < 0)
			{
				mCharater.animation.Play("Run");
				mCharater.transform.Translate(Vector3.right * Time.deltaTime * -2);
			}
			else 
			{
				mCharater.animation.Stop("Run");
				mCharater.animation.gameObject.SampleAnimation(mCharater.animation["Run"].clip,0);
			}
		}
		else if (mIsPlaying && !mCharactereManager.mIsDead)
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
				}
				else if(mIndexAnimation>(mDuration+ (LevelSizeMax -mDuration -2) ))
				{
					LoadNextScene();
				}
				else
				{
					mCharater.animation.Play("Run");
				}
				mIndexAnimation++;
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
