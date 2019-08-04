using UnityEngine;
using System.Collections;

public class MainMenuSequenceController : MonoBehaviour
{
	public float scrollAmount = 1F;
	public float scrollSpeed = 0.3F;

	public Transform mainCamera;
	public ObjectSpawner demoSpawner; 

	[HideInInspector]
	public bool shouldExecute = true;

	private Vector3 camInitPos;

	void Start() {
		camInitPos = mainCamera.position;
	}

	void Update() {
		if (!shouldExecute) {
			return;
		}

		var fromPos = mainCamera.position;
		var toPos = fromPos + (Vector3.up * scrollAmount);

		mainCamera.position = Vector3.Lerp(fromPos, toPos, scrollSpeed * Time.deltaTime);
	}

	public void Reset() {
		mainCamera.position = camInitPos;
		demoSpawner.PerformReset();
		demoSpawner.gameObject.SetActive(false);
	}

	public void Play() {
		shouldExecute = true;
		var camManager = mainCamera.GetComponent<CameraManager>();
		camManager.enabled = false;
		demoSpawner.gameObject.SetActive(true);
	}

	public void Stop() {
		shouldExecute = false;
		var camManager = mainCamera.GetComponent<CameraManager>();
		camManager.enabled = true;
		demoSpawner.gameObject.SetActive(false);
	}
}
