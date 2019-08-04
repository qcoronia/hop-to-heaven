using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (Slider))]
public class AudioSetting : MonoBehaviour {
	public AudioMixer masterMixer;
	public string exposedParamName;
	public string playerPrefName;
	[Range(0F, 1F)]
	public float falloffPercent;

	public float mixerVolumeMax = 0F;
	public float mixerVolumeMin = -80F;
	public float mixerVolumeLength = 80F;

	private Slider slider;

	void Start() {
		var currentValue = PlayerPrefs.GetFloat(playerPrefName);
		slider = transform.GetComponent<Slider>();
		slider.value = currentValue;

		mixerVolumeLength = Mathf.Abs(mixerVolumeMin - mixerVolumeMax);
	}

	public void OnValueChanged() {
		if (slider == null) {
			slider = transform.GetComponent<Slider>();
			var currentValue = PlayerPrefs.GetFloat(playerPrefName);
			slider.value = currentValue;
		}

		var value = slider.value;
		masterMixer.SetFloat(exposedParamName, value);
		PlayerPrefs.SetFloat(playerPrefName, value);
	}
}
