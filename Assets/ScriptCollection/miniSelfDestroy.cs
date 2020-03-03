using UnityEngine;
using System.Collections;

public class miniSelfDestroy : MonoBehaviour
{
	
	// Use this for initialization
	void Start () {
		StartCoroutine (timerFn());	
	}
	
	// Update is called once per frame
	void Update()
	{

	}
	
	IEnumerator timerFn()
	{
		yield return new WaitForSeconds(30.0f);
		Destroy(this.gameObject);
	}
	
}
