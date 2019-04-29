using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour {

	public float JumpPower = 20f;
	public float MoveSpeed = 20f;

	private Animator anim;
	private Movement move;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		move = GetComponent<Movement> ();
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			anim.SetBool ("isWalking", true);
			body.AddForce (1* MoveSpeed,0,0 , 0);
		} else {
			anim.SetBool ("isWalking", false);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			body.AddForce (0, 1 * JumpPower, 0, 0);
			anim.SetTrigger ("Jump");
		}
	}
}
