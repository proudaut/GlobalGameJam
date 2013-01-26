using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Level : MonoBehaviour
{
	public int mDuration;
	public GameObject mCharater;
	public List<Track> mTrack;
	public List<LevelElement> mLevelElements =  new List<LevelElement>();
	public AudioSource mSound;
	public TextAsset mLevelDescriptor;
	
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
	
	
	void OnButtonClick(Button _button)
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
	
	public void Play()
	{
		mIsPlaying = true;
		mSound.Play();
		foreach(Track lTrack in mTrack)
		{
			lTrack.Play();
		}	
	}
	
	public void Stop()
	{
		mIsPlaying = false;
		mCharater.transform.localPosition = new Vector3(0.5f,3,0);
		mSound.Stop();
		foreach(Track lTrack in mTrack)
		{
			lTrack.Stop();
		}	
	}
	
	
	// Use this for initialization
	void Start () 
	{
		LoadLevel();
		GenerateLevel();
	}
	
	void LoadLevel()
	{
		ArrayList lElementList = (ArrayList)Prime31.Json.jsonDecode(mLevelDescriptor.text);
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
			switch(lLevelElement.mLevelElementType)
			{
				case LevelElementType.G :  
				{
					GameObject lInstanceElement = Instantiate(G) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.GL : 
				{
					GameObject lInstanceElement =  null;
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundL) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GL) as GameObject;
					}
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				
				case LevelElementType.GR :  
				{
					GameObject lInstanceElement =  null;
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundR) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GR) as GameObject;
					}
				
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				
				
				case LevelElementType.GD :  
				{
					GameObject lInstanceElement =  null;
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(GroundHole) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GD) as GameObject;
					}
					
					
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.GF :
				{
					GameObject lInstanceElement =  null;
					if(lLevelElement.mY == 0)
					{
						lInstanceElement = Instantiate(Ground) as GameObject;
					}
					else
					{
						lInstanceElement = Instantiate(GF) as GameObject;
					}
				
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				
				case LevelElementType.spi :  
				{
					GameObject lInstanceElement = Instantiate(spi) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.mid : 
				{
					GameObject lInstanceElement = Instantiate(mid) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.low :  
				{
					GameObject lInstanceElement = Instantiate(low) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.vc :  
				{
					GameObject lInstanceElement = Instantiate(vc) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
				case LevelElementType.hc :
				{
					GameObject lInstanceElement = Instantiate(hc) as GameObject;
					lInstanceElement.transform.parent = this.transform;
					lInstanceElement.transform.localPosition = new Vector3(lLevelElement.mX*TileOffset,lLevelElement.mY*TileOffset,0);
					break;
				}
				
			}
		}
	}
	
	
	
	
	
	
	
	
	// Update is called once per frame
	void Update () 
	{
		if (mIsPlaying)
		{
			mCharater.transform.Translate(Vector3.right * Time.deltaTime * -2);
		}
	}
}
