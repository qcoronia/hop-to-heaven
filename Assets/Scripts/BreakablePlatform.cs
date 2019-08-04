using UnityEngine;
using System.Collections;

public class BreakablePlatform : Platform {
	public ParticleSystem poofEffect;

	public void Break() {
		if (poofEffect != null) {
			Instantiate(poofEffect, transform.position, transform.rotation);
		}

		Destroy(gameObject);
	}
}
