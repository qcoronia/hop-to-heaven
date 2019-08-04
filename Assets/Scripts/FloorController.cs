using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {
	public Camera mainCamera;
	public float distanceToDispose;

	void Update() {
		if (!mainCamera) {
			return;
		}

		if (Mathf.Abs(mainCamera.transform.position.y - transform.position.y) > distanceToDispose) {
		}
	}
}
