using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour {
    public Transform target;
    public float distanceAhead;
    public float distanceInterval;
    public float horizontalOffset;
    public int instanceLimit;
    public float distanceToLatestPosition;

    [Header("Prefab Refrences")]
    public Transform[] prefabs;

    private Vector3 latestPosition;

    private Quaternion qIdentity;
    private Vector3 v3Up;
    private Vector3 v3Right;

    void Start() {
        qIdentity = Quaternion.identity;
        v3Up = Vector3.up;
        v3Right = Vector3.right;
    }

    void Update() {
        if(!target) {
            return;
        }

        if(latestPosition.y > target.position.y + distanceAhead) {
            return;
        }

        RemoveObsoletes();

        var randomIndex = Mathf.FloorToInt(Random.value * (prefabs.Length));
        var leftOrRight = Mathf.Sign((Random.value * 2F) - 1F);

        var randomOffset = Random.value;
        randomOffset = (randomOffset * randomOffset * randomOffset) - 1F;
        var spawnPosition = latestPosition + (v3Right * (randomOffset * (horizontalOffset * leftOrRight)));
        var spawn = Instantiate(prefabs[randomIndex], spawnPosition, qIdentity) as Transform;
        spawn.SetParent(transform);
        spawn.SetAsFirstSibling();

        var platform = spawn.GetComponent<Platform>();
        if(platform.forceToCenter) {
            spawn.position = new Vector3(0F, spawn.position.y, spawn.position.z);
        }
        if(platform.forceToEdge) {
            spawn.position = new Vector3(horizontalOffset * leftOrRight, spawn.position.y, spawn.position.z);
        }

        if(transform.childCount > instanceLimit) {
            var oldestSpawn = transform.GetChild(transform.childCount - 1);
            Destroy(oldestSpawn.gameObject);
        }

        var randomDistance = Random.Range(platform.minIntervalOverride, distanceInterval);
        latestPosition = latestPosition + (v3Up * randomDistance);
    }

    public void PerformReset() {
        latestPosition = Vector3.zero;
        for(var i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void RemoveObsoletes() {
        for(var i = 0; i < transform.childCount; i++) {
            var child = transform.GetChild(i);
            if(child.position.y < (latestPosition.y - distanceToLatestPosition)) {
                Destroy(child.gameObject);
            }
        }
    }
}
