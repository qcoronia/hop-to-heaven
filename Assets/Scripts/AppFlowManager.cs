#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AppFlowManager : MonoBehaviour {
	public AppState appState;
	public GameManager gameManager;
	public MainMenuSequenceController mainMenuSequenceController;

	[Header("Panels")]
	public Panels panels;

	[Header("Labels")]
	public Text scoreLabel;
	public Text lastBestLabel;

	[Header("Sliders")]
	public Slider backgroundMusicSlider;
	public Slider soundEffectsSlider;

	[Header("Audio")]
	public AudioSource menuMusic;
	public AudioSource gameMusic;

	private bool allowQuit = false;

	void Start() {
		appState = AppState.Home;
		gameManager.OnGameOver.AddListener(OnGameOver);
		UpdateState();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			switch (appState)
			{
				case AppState.Home:
					TryQuitGame();
					break;
				case AppState.Settings:
					OnBackToHome("Settings");
					break;
				case AppState.Paused:
					// Do Nothing
					break;
				case AppState.InGame:
					appState = AppState.Paused;
					UpdateState();
					break;
				case AppState.Quitting:
					// Do Nothing
					break;
				case AppState.GameOver:
					// Do Nothing
					break;
				default:
					break;
			}
		}
	}

	void OnGameOver() {
		appState = AppState.GameOver;
		UpdateState();
	}

	public void OnStartPressed() {
		appState = AppState.InGame;
		UpdateState();
	}

	public void OnSettingsPressed() {
		appState = AppState.Settings;
		UpdateState();
	}

	public void OnPausedPressed() {
		appState = AppState.Paused;
		UpdateState();
	}

	public void OnResumePressed() {
		appState = AppState.Resume;
		UpdateState();
	}

	public void OnRetry() {
		appState = AppState.InGame;
		UpdateState();
	}
	
	public void OnBackToHome(string referrer) {
		switch(referrer) {
			case "Settings":
				appState = AppState.Home;
				UpdateState();
				break;
			case "Paused":
				Time.timeScale = 1;
				SceneManager.LoadScene(0);
				break;
			default:
				SceneManager.LoadScene(0);
				break;
		}
	}
	
	void UpdateState() {
		panels.Set(appState);

		switch (appState)
		{
			case AppState.Home:
				gameManager.shouldExecute = false;
				if (!mainMenuSequenceController.shouldExecute) {
					mainMenuSequenceController.Reset();
				}
				mainMenuSequenceController.Play();
				PlayMenuMusic();
				break;
			case AppState.Settings:
				gameManager.shouldExecute = false;
				backgroundMusicSlider.value = PlayerPrefs.GetFloat("BgmVolume");
				backgroundMusicSlider.GetComponent<AudioSetting>().OnValueChanged();
				soundEffectsSlider.value = PlayerPrefs.GetFloat("SfxVolume");
				soundEffectsSlider.GetComponent<AudioSetting>().OnValueChanged();
				break;
			case AppState.Paused:
				gameManager.shouldExecute = false;
				Time.timeScale = 0;
				PauseGameMusic();
				break;
			case AppState.Resume:
				gameManager.shouldExecute = true;
				appState = AppState.InGame;
				panels.Set(appState);
				ResumeGameMusic();
				if (Time.timeScale == 0) {
					Time.timeScale = 1;
				}
				break;
			case AppState.InGame:
				gameManager.shouldExecute = true;
				gameManager.PerformReset();
				PlayGameMusic();
				mainMenuSequenceController.Stop();
				mainMenuSequenceController.Reset();
				break;
			case AppState.Quitting:
				gameManager.shouldExecute = true;
				Application.CancelQuit();
				panels.Set(appState);
				break;
			case AppState.GameOver:
				gameManager.shouldExecute = false;
				gameMusic.Stop();
				var score = gameManager.score;
				var lastBest = PlayerPrefs.GetInt("lastBest");
				scoreLabel.text = score.ToString();
				lastBestLabel.text = lastBest.ToString();
				if (score > lastBest) {
					PlayerPrefs.SetInt("lastBest", score);
				}
				break;
			default:
				break;
		}
	}

	public void PlayMenuMusic() {
		if (!menuMusic.isPlaying) {
			menuMusic.Play();
		}
		if (gameMusic.isPlaying) {
			gameMusic.Stop();
		}
	}

	public void PlayGameMusic() {
		if (!gameMusic.isPlaying) {
			gameMusic.Play();
		}
		if (menuMusic.isPlaying) {
			menuMusic.Stop();
		}
	}

	public void PauseGameMusic() {
		gameMusic.Pause();
	}

	public void ResumeGameMusic() {
		gameMusic.UnPause();
	}

	void OnApplicationQuit() {
		if (!allowQuit) {
			appState = AppState.Quitting;
			UpdateState();
		}
	}

	public void TryQuitGame() {
		if (allowQuit) {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
			Application.Quit();
		}
		else {
			appState = AppState.Quitting;
			UpdateState();
		}
	}

	public void QuitGame() {
		allowQuit = true;
		TryQuitGame();
	}

	public void CancelQuitGame() {
		allowQuit = false;
		appState = AppState.Home;
		UpdateState();
	}
}

[System.Serializable]
public enum AppState {
	Home,
	Settings,
	Paused,
	InGame,
	GameOver,
    Resume,
    Quitting,
}

[System.Serializable]
public class Panels {
	public GameObject home;
	public GameObject settings;
	public GameObject paused;
	public GameObject inGame;
	public GameObject gameOver;
	public GameObject quitDialog;

	public void Set(AppState appState) {
		home.SetActive(false);
		settings.SetActive(false);
		paused.SetActive(false);
		inGame.SetActive(false);
		gameOver.SetActive(false);
		quitDialog.SetActive(false);

		switch (appState)
		{
			case AppState.Home:
				home.SetActive(true);
				break;
			case AppState.Settings:
				settings.SetActive(true);
				break;
			case AppState.Paused:
				paused.SetActive(true);
				break;
			case AppState.InGame:
				inGame.SetActive(true);
				break;
			case AppState.Quitting:
				quitDialog.SetActive(true);
				break;
			case AppState.GameOver:
				gameOver.SetActive(true);
				break;
			default:
				break;
		}
	}
}