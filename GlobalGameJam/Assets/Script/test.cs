using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	/*void OnTriggerEnter(Collider other) 
	{
		Debug.Log("Enter collision " + other.gameObject.name); 
    }
	void OnTriggerExit(Collider other) 
	{
       Debug.Log("Exit collision " + other.gameObject.name); 
    }*/
	
	
	void OnCollisionEnter(Collision other) 
	{
		Debug.Log("Enter collision " + other.gameObject.name); 
    }
	void OnCollisionExit(Collision other) 
	{
       Debug.Log("Exit collision " + other.gameObject.name); 
    }

}
