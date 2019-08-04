using UnityEngine;
using System.Collections;

public class CharacterAnimationManager : MonoBehaviour {

	public Transform visualContainer;

	public string[] jumpingAnimations;
	public string[] fallingAnimations;

	private CharacterManager characterManager;
	private Animator animator;
	private AnimationType currentAnimationType;

	void Start() {
		characterManager = transform.GetComponent<CharacterManager>();
		characterManager.OnFall.AddListener(PlayFallingAnimation);
		characterManager.OnJump.AddListener(PlayJumpingAnimation);

		var animators = visualContainer.GetComponentsInChildren<Animator>();
		if(animators.Length <= 0) {
			Debug.LogError("No Animators");
		}

		if(fallingAnimations.Length <= 0 || jumpingAnimations.Length <= 0) {
			Debug.LogError("No Animations");
		}

		animator = animators[0];
	}

	void PlayFallingAnimation() {
		switch (currentAnimationType)
		{
			case AnimationType.FallingAnimation:
				return;
			case AnimationType.JumpingAnimation:
				// var randomIndex = Mathf.FloorToInt(Random.value * (fallingAnimations.Length));
				// var randomAnimation = fallingAnimations[randomIndex];
				animator.SetBool("IsFalling", false);
				//animator.Play(randomAnimation);
				currentAnimationType = AnimationType.FallingAnimation;
				break;
			default:
				break;
		}
	}

	void PlayJumpingAnimation() {
		switch (currentAnimationType)
		{
			case AnimationType.JumpingAnimation:
				return;
			case AnimationType.FallingAnimation:
				var randomIndex = Mathf.FloorToInt(Random.value * (jumpingAnimations.Length));
				var randomAnimation = jumpingAnimations[randomIndex];
				animator.SetBool("IsFalling", true);
				animator.Play(randomAnimation);
				currentAnimationType = AnimationType.JumpingAnimation;
				break;
			default:
				break;
		}
	}
}

public enum AnimationType {
	JumpingAnimation,
	FallingAnimation,
}