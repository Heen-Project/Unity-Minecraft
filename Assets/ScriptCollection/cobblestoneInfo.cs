using UnityEngine;
using System.Collections;

public class cobblestoneInfo : MonoBehaviour {

	private int hitCount = 5;
	
	public int cobblestoneHitFn(){
		hitCount--;
		return hitCount;
	}
}
