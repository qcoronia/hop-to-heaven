using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	public float smoothingFactor = 3f;
	private float previousH;

	void Start() {
		previousH = 0f;
	}

	void Update() {
		var h = Input.GetAxis("Horizontal");

#if UNITY_EDITOR
		gameObject.BroadcastMessage("Move", h);
		return;
#endif
		var hasInput = false;
		h = 0f;
	
		if(Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			var touchPosition = Input.GetTouch(0).position.x;
			if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
			{
				h = touchPosition / Screen.width;
				h = Mathf.Sign((h * 2f) - 1f);
				h = Mathf.Lerp(previousH, h, smoothingFactor * Time.deltaTime);
				hasInput = true;
			}
		}
		
		if (!hasInput) {
			h = Mathf.Lerp(previousH, h, smoothingFactor * Time.deltaTime);
		}
		
		previousH = h;

		gameObject.BroadcastMessage("Move", h);
	}
}