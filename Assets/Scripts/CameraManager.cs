using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public Transform target;
	public float upperBound;
	public float lowerBound;
	public float followSpeed;

	private float targetElevation;

	void Update() {
        if(!target) {
            return;
        }

		if(target.position.y > transform.position.y + upperBound) {
			targetElevation = target.position.y - upperBound;
		}

		if(target.position.y < transform.position.y + lowerBound) {
			targetElevation = target.position.y - lowerBound;
		}

		var finalElevation = Mathf.Lerp(transform.position.y, targetElevation, followSpeed * Time.deltaTime);

		transform.position = new Vector3(transform.position.x, finalElevation, transform.position.z);
	}
}
