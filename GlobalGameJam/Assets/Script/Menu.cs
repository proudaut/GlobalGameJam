using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
	public AudioSource mSound;
	// Use this for initialization
	void Start () 
	{
		//PlayerPrefs.SetInt("Level1",0);
		MusicManager.PlayMusique(MusicID.Menu);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnButtonClick(Button _button)
	{
		mSound.Play();
		StartCoroutine(Next());
	}
	
	
	IEnumerator Next()
	{
		yield return new WaitForSeconds(1);
		Application.LoadLevel("Tutorial");
	}
}
