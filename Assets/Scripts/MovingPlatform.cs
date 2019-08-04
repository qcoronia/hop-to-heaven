using UnityEngine;
using System.Collections;

public class MovingPlatform : Platform {
	public float movementSpeed;
	public float horizontalLimit;

	protected Vector3 v3Right;
	protected float direction;

	void Start() {
		v3Right = Vector3.right;
		direction = Mathf.Sign(movementSpeed);
	}

	void Update() {
		transform.position = transform.position + (v3Right * (movementSpeed * Time.deltaTime));
		if(direction < 0) {
			if(transform.position.x < -horizontalLimit) {
				transform.position = new Vector3(horizontalLimit, transform.position.y, transform.position.z);
			}
		}
		else {
			if(transform.position.x > horizontalLimit) {
				transform.position = new Vector3(-horizontalLimit, transform.position.y, transform.position.z);
			}
		}
	}
}
