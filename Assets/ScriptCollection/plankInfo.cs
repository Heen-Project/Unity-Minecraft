using UnityEngine;
using System.Collections;

public class plankInfo : MonoBehaviour {
	
	private int hitCount = 5;
	
	public int plankHitFn(){
		hitCount--;
		return hitCount;
	}
}
