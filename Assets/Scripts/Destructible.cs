using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {
	public void StartDestroy() {
		Destroy(gameObject);
	}
}
