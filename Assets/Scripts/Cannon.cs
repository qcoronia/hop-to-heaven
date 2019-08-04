using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	public float speed;

	private Vector3 v3Forward;

	void Start() {
		v3Forward = transform.forward;
	}

	void Update() {
		transform.position = transform.position + (v3Forward * (speed * Time.deltaTime));
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
