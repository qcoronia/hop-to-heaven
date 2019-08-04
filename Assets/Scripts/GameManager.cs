using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool shouldExecute = false;

	[Header("UI")]
	public Text scoreText;

	[Header("Camera")]
	public Transform cam;
	private Vector3 initCamPos;

	[Header("Player")]
	public CharacterManager player;
	[HideInInspector]
	public int score;
	private Vector3 initPlayerPos;

	[Header("Platforms")]
	public ObjectSpawner platformManager;
	public Transform initialPlatform;

	[Header("Mechanics")]
	public float gameOverLimit;
	public UnityEvent OnGameOver;
	private float gameOverAltitude;

	void Start() {
		initPlayerPos = player.transform.position;
		initCamPos = cam.position;

		player.gameObject.SetActive(false);
		platformManager.gameObject.SetActive(false);
		initialPlatform.gameObject.SetActive(false);

		gameOverAltitude = initPlayerPos.y - gameOverLimit;

		player.OnHit.AddListener(OnPlayerHit);

		OnGameOver = OnGameOver ?? new UnityEvent();
	}

	void Update() {
		if(!shouldExecute) {
			return;
		}

		score = Mathf.Max(Mathf.FloorToInt(player.transform.position.y), score);
		scoreText.text = score.ToString();

		gameOverAltitude = score - gameOverLimit;

		if (initialPlatform.position.y < gameOverAltitude) {
			initialPlatform.gameObject.SetActive(false);
		}

		if(player.transform.position.y <= gameOverAltitude) {
			OnGameOver.Invoke();
			player.gameObject.SetActive(false);
		}
	}

	public void PerformReset() {
		score = 0;

		player.gameObject.SetActive(true);
		platformManager.gameObject.SetActive(true);
		initialPlatform.gameObject.SetActive(true);
		cam.GetComponent<CameraManager>().enabled = true;

		player.PerformReset();
		player.transform.position = initPlayerPos;
		cam.position = initCamPos;
		player.WakeUp();
		platformManager.PerformReset();
	}

	void OnPlayerHit() {
		OnGameOver.Invoke();
	}

#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(-3F, gameOverAltitude, 0F),
					     new Vector3(3F, gameOverAltitude, 0F));
	}
#endif
}
