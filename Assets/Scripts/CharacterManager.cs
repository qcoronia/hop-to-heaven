using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(InputManager))]
public class CharacterManager : MonoBehaviour {
	[Header("Movement")]
	public float horizontalSpeed;
	public float groundDetectorLength;
	public float maxUpwardVelocity;
	public float maxDownwardVelocity;
	public float verticalVelocityChangeSpeed;
	public float horizontalLimit;
	public float maxSteerAngle;
	public float steerSmoothingFactor;

	[Header("Jumping")]
	public float jumpHeight;
	public float jumpDuration;
	public float jumpEquilibriumMargin;
	public float springMultiplier;
	public AudioSource sfxJump;
	public AudioSource sfxSpring;
	public AudioSource sfxPoof;

	[Space]
	public float awakeDelay;
	public bool isAwake;

	[Header("Events")]
	public UnityEvent OnHit;
	public UnityEvent OnFall;
	public UnityEvent OnJump;

	private float verticalVelocity;
	private bool isRising = false;
	private Vector3 lastLandPoint;
	private bool isHit = false;

    private Vector3 v3Up;
    private Vector3 v3Right;
    private Vector3 v3Down;

    void Start() {
        v3Up = Vector3.up;
        v3Right = Vector3.right;
        v3Down = Vector3.down;

		OnHit = OnHit ?? new UnityEvent();
		OnFall = OnFall ?? new UnityEvent();
		OnJump = OnJump ?? new UnityEvent();
	}

	public void WakeUp() {
		isAwake = true;
	}

	public void PerformReset() {
		isHit = false;
	}

	void Move(float factor) {
		if(isHit) {
			return;
		}

		var finalPosition = transform.position + (v3Right * (factor * (horizontalSpeed * Time.deltaTime)));
		transform.position = new Vector3(Mathf.Clamp(finalPosition.x, -horizontalLimit, horizontalLimit), finalPosition.y, finalPosition.z);

		// var previousEulerAngles = transform.eulerAngles;
		var newEulerAngles = v3Up * -(factor * maxSteerAngle);
		//var refinedNewEulerAngles = Vector3.Lerp(previousEulerAngles, newEulerAngles, steerSmoothingFactor * Time.deltaTime);
		transform.eulerAngles = newEulerAngles;
	}

	void Update() {
		if(!isAwake) {
			return;
		}

		if(isHit) {
			return;
		}

		if(isRising) {
			var targetElevationPoint = lastLandPoint.y + jumpHeight;
			var currentElevation = transform.position.y;
			var finalElevation = Mathf.Lerp(currentElevation, targetElevationPoint, jumpDuration * Time.deltaTime);
			transform.position = new Vector3(transform.position.x, finalElevation, 0F);
			var distanceToElevationPoint = Mathf.Abs(targetElevationPoint - currentElevation);
			if(distanceToElevationPoint <= jumpEquilibriumMargin) {
				isRising = false;
				verticalVelocity = 0F;
				OnFall.Invoke();
			}
			return;
		}

		verticalVelocity -= verticalVelocityChangeSpeed * Time.deltaTime;
		verticalVelocity = Mathf.Clamp(verticalVelocity, maxDownwardVelocity, maxUpwardVelocity);
		transform.position = transform.position + (v3Up * (verticalVelocity * Time.deltaTime));
	}

	void FixedUpdate() {
		if(!isAwake) {
			return;
		}

		if(isRising) {
			return;
		}

		if(isHit) {
			return;
		}

		RaycastHit hit;
		if(Physics.Raycast(transform.position, v3Down, out hit, groundDetectorLength)) {
			lastLandPoint = hit.point;
			isRising = true;
			CheckHits(hit);
			OnJump.Invoke();
		}
	}

	void CheckHits(RaycastHit hit) {
		if(hit.transform.tag == "BreakablePlatform") {
			var breakablePlatform = hit.transform.GetComponent<BreakablePlatform>();
			breakablePlatform.Break();
			sfxPoof.Play();
		}

		if(hit.transform.tag == "Spring") {
			lastLandPoint = hit.point + (v3Up * (jumpHeight * springMultiplier));
			var cloudAnimator = hit.transform.GetComponentInParent<Animator>();
			if (cloudAnimator != null) {
				cloudAnimator.Play("CloudBounce");
			}
			sfxSpring.Play();
			// var springAnimators = hit.transform.GetComponentsInChildren<Animator>();
			// if (springAnimators.Length > 0) {
			// 	springAnimators[0].Play("CloudBounce");
			// }
		}

		if(hit.transform.tag == "Platform") {
			var cloudAnimator = hit.transform.GetComponent<Animator>();
			if (cloudAnimator != null) {
				cloudAnimator.Play("CloudBounce");
			}
			var cloudPuff = transform.Find("CloudPuff");
			if (cloudPuff != null) {
				var cloudPuffPartSys = cloudPuff.GetComponent<ParticleSystem>();
				if (cloudPuffPartSys != null) {
					cloudPuffPartSys.Play();
				}
			}
			sfxJump.Play();
		}

		if(hit.transform.tag == "BouncableEnemy") {
			var enemyAnimator = hit.transform.GetComponent<Animator>();
			if (enemyAnimator != null) {
				enemyAnimator.Play("BouncedOn");
			}
		}

		if(hit.transform.tag == "Cannon") {
			isRising = false;
			isHit = true;
			OnHit.Invoke();
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "Cannon") {
			var destructible = other.transform.GetComponent<Destructible>();
			destructible.StartDestroy();
			isRising = false;
			isHit = true;
			OnHit.Invoke();
		}
		if(other.transform.tag == "BouncableEnemy") {
			var enemyAnimator = other.transform.GetComponent<Animator>();
			if (enemyAnimator != null) {
				enemyAnimator.Play("Dropping");
			}
			var animator = transform.GetComponent<Animator>();
			if (animator != null) {
				animator.Play("Dropping");
			}
			isRising = false;
			isHit = true;
			OnHit.Invoke();
		}
	}

#if UNITY_EDITOR	
	void OnDrawGizmos() {
        Gizmos.color = Color.green;
		if(!isRising) {
			Gizmos.DrawRay(transform.position, v3Down * groundDetectorLength);
		}

		Gizmos.DrawWireSphere(lastLandPoint, 0.5F);
		Gizmos.color = Color.blue;
        Gizmos.DrawRay(lastLandPoint, (v3Up * jumpHeight));
		Gizmos.DrawWireSphere(lastLandPoint + (v3Up * jumpHeight), 0.5F);

		Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position - (v3Up * -1F), (transform.forward * -1.5F));
    }
#endif
}