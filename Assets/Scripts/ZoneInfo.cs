using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneInfo : ScriptableObject {
	public new string name;
	public float zoneStart;
	public float zoneEnd;
	public List<Platform> platforms;
}