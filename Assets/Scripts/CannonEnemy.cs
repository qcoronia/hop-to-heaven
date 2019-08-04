using UnityEngine;
using System.Collections;

public class CannonEnemy : MonoBehaviour {
	public Transform nozzlePosition;
	public Transform projectile;
	public float interval;
	
	private float currentTime = 0F;

	void Start() {
		transform.LookAt(transform.position + (Vector3.forward * Mathf.Sign(transform.position.x)));
	}

	void Update() {
		currentTime += Time.deltaTime;
		if(currentTime > interval) {
			currentTime = 0F;
			var cannonProjectile = Instantiate(projectile, nozzlePosition.position, nozzlePosition.rotation) as Transform;
			cannonProjectile.SetParent(transform);
		}
	}
}
