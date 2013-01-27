using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
	public AudioSource mSound;

	// Use this for initialization
	void Start () {
	
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
		Application.LoadLevel("LevelScene");
	}

}
