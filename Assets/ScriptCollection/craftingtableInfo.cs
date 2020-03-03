using UnityEngine;
using System.Collections;

public class craftingtableInfo : MonoBehaviour {
	private int hitCount = 5;
	
	public int craftingtableHitFn(){
		hitCount--;
		return hitCount;
	}
}
