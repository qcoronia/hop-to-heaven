using UnityEngine;
using System.Collections;

public class UIThatFollowsMusicBeat : MonoBehaviour
{
	public AudioSource audioSource;
	public BeatFollowingUi[] beatFollowingUis;

	[Header("Digital Audio Processing")]
	public float energyCeiling;
	public float energyFloor;
	[Range(0, 256)]
	public int listeningRangeLeft;
	[Range(0, 256)]
	public int listeningRangeRight;

	[Header("Ui Transformation")]
	public float uiScaleMin;
	public float uiScaleMax;

	void Start() {
		for (var i = 0; i < beatFollowingUis.Length; i++) {
			beatFollowingUis[i].initScale = beatFollowingUis[i].ui.localScale;
		}
	}

	void Update() {
		var fft = new float [256];
		audioSource.GetSpectrumData(fft, 0, FFTWindow.BlackmanHarris);

		var listeningRange = Mathf.Abs(listeningRangeLeft - listeningRangeRight);
		var energy = 0f;
		for (var i = listeningRangeLeft; i < listeningRangeRight; i++) {
			energy = energy + (fft[i] * 1000f);
		}

		energy = energy / Mathf.Max(1, listeningRange);
		energy = Mathf.Clamp(energy, energyFloor, energyCeiling);
		energy = energy - energyFloor;
		var energyPercent = energy / (energyCeiling - energyFloor);
		var uiScaleRange = Mathf.Abs(uiScaleMin - uiScaleMax);
		var uiScaleDelta = uiScaleRange * energyPercent;
		uiScaleDelta = uiScaleMin + uiScaleDelta;

		for (var i = 0; i < beatFollowingUis.Length; i++) {
			beatFollowingUis[i].ui.localScale = beatFollowingUis[i].initScale + (Vector3.one * uiScaleDelta);
		}
	}
}

[System.Serializable]
public class BeatFollowingUi
{
	public RectTransform ui;
	[HideInInspector]
	public Vector3 initScale;
}