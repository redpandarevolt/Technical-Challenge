//using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
	[RequireComponent(typeof (ThirdPersonCharacter))]
	public class AIMonsterControl : MonoBehaviour
	{
		public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
		public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Transform target;                                    // target to aim for
		public float RunDistance = 100;
		public float attackDistance = 3;

		private GameObject mRabbit;
		private Animator anim;
		private bool isPrey = false;
		private bool isJump = false;
		private float BunnyKills = 0;
		private float mJumpTimer = 0;
		private int mRandom = 5;
		private float timer = 0;

		private void Start()
		{
			// get the components on the object we need ( should not be null due to require component so no need to check )
			agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();
			anim = GetComponent<Animator> ();

			agent.updateRotation = false;
			agent.updatePosition = true;

		}


		private void Update()
		{
			if (mJumpTimer <= mRandom) {
				mJumpTimer += Time.deltaTime;
				isJump = false;
			} else {
				mJumpTimer = 0;
				mRandom = Random.Range (5, 20);
				isJump = true;
			}


			float dist = Mathf.Infinity;
			GameObject mPrey = FindClosestEnemy( GameObject.FindGameObjectsWithTag( "Prey" ) );
			if (mPrey == null) {
				return;
			}
			//set distance of closest prey
			if (mPrey != null) {
				dist = (transform.position - mPrey.transform.position).magnitude;
				SetTarget (mPrey.transform);
			}

			//Target is really far, walk toward them
			if (dist >= RunDistance) {
				anim.SetBool ("isRunning", false);
				anim.SetBool ("isWalking", true);
				anim.SetBool ("isAttacking", false);
				agent.SetDestination (target.position);
			}
			//Target is within running range
			else if (dist >= attackDistance) {
				anim.SetBool ("isRunning", true);
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isAttacking", false);
				agent.SetDestination (target.position);
			} 
			//Target is withing striking range
			else {
				anim.SetBool ("isRunning", false);
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isAttacking", true);
				agent.SetDestination (target.position);
			}
				

			if (agent.remainingDistance > agent.stoppingDistance)
				character.Move (agent.desiredVelocity, false, isJump);
			else {
				character.Move (Vector3.zero, false, isJump);
			}
		}






		public void SetTarget(Transform target)
		{
			this.target = target;
		}


		private GameObject FindClosestEnemy( GameObject[] prey )
		{

			if (prey.Length == 0) {
				Debug.Log ("no prey found");
				return null;
			}
			GameObject closest = null;
			float closestDist = Mathf.Infinity;
			Vector3 currPos = transform.position;
			foreach( GameObject tar in prey )
			{
				Vector3 dist = tar.transform.position - currPos;
				float distMag = dist.magnitude;
				if( distMag < closestDist )
				{
					closestDist = distMag;
					closest = tar;
				}
			}

			return closest;
		}

		private void OnCollisionEnter( Collision col )
		{
			if (timer <= 2) {
				timer += Time.deltaTime;
			} else {
				if (col.gameObject.CompareTag ("Prey")) {
					BunnyKills++;
					Debug.Log (BunnyKills);
					transform.localScale = Vector3.one * (1 + (BunnyKills / 10));
					timer = 0;
				}
			}
		}

	}
}