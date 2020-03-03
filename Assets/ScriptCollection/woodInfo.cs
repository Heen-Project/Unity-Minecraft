using UnityEngine;
using System.Collections;

public class woodInfo : MonoBehaviour {

	private int hitCount = 5;
	
	public int woodHitFn(){
		hitCount--;
		return hitCount;
	}
}
