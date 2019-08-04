using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (Slider))]
public class SliderSetting : MonoBehaviour {
	public string playerPrefName;

	public SliderChangeEvent OnChange;

	private Slider slider;

	void Start() {
		var currentValue = PlayerPrefs.GetFloat(playerPrefName);
		slider = transform.GetComponent<Slider>();
		slider.value = currentValue;

		OnChange = OnChange ?? new SliderChangeEvent();
	}

	public void OnValueChanged() {
		if (slider == null) {
			slider = transform.GetComponent<Slider>();
			var currentValue = PlayerPrefs.GetFloat(playerPrefName);
			slider.value = currentValue;
		}

		var value = slider.value;
		OnChange = OnChange ?? new SliderChangeEvent();
		OnChange.Invoke(value);
		PlayerPrefs.SetFloat(playerPrefName, value);
	}
}

public class SliderChangeEvent : UnityEvent<float> {
}