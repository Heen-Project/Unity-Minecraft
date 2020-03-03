using UnityEngine;
using System.Collections;

public class grassInfo : MonoBehaviour {

	private int hitCount = 5;
	
	public int grassHitFn(){
		hitCount--;
		return hitCount;
	}
}
