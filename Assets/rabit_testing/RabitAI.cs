using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RabitAI : MonoBehaviour {

	// General state machine variables
	private GameObject predator;
	private GameObject home;
	private Animator animator;
	private float currentDistance;

	// Patrol state variables
	public float stoppingDistance;
	public float unsafeDistance;
	private Vector3 goal;
	private bool threat = false;
	private UnityEngine.AI.NavMeshAgent navMeshAgent;

	private void Awake() {
		animator = gameObject.GetComponent<Animator>();
		navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		navMeshAgent.stoppingDistance = stoppingDistance;
        StartCoroutine(resetGoal(15.0f));
	}

	private void Update() {
		//First we check distance from the predator
		predator = getClosestObject( GameObject.FindGameObjectsWithTag( "Predator" ) );
		currentDistance = predator != null ?
			Vector3.Distance(predator.transform.position, transform.position) :
			Mathf.Infinity;
		animator.SetFloat("DistanceFromPredator", currentDistance);

		if( currentDistance < unsafeDistance )
		{
			threat = true;
			home = getClosestObject( GameObject.FindGameObjectsWithTag( "Home" ) );
			navMeshAgent.SetDestination( home.transform.position );
		}

		// stop at home
		if( threat &&
			Vector3.Distance( transform.position, home.transform.position ) < stoppingDistance)
		{
			animator.SetBool( "isHome", true );
			Destroy( gameObject );
		}

		// only walk when moving
		if( Vector3.Distance( transform.position, goal ) < stoppingDistance )
		{
			animator.SetBool( "isIdle", true );
		}
		else
		{
			animator.SetBool( "isIdle", false );
		}
	}

	private void OnCollisionEnter( Collision col )
	{
		if( col.gameObject.CompareTag( "Predator" ) )
		{
			navMeshAgent.isStopped = true;
			animator.SetBool( "isCaught", true );
			Destroy( gameObject, 3 );
		}
	}

	private IEnumerator killRabbit()
	{
		navMeshAgent.isStopped = true;
		animator.SetBool( "isCaught", true );
		yield return new WaitForSeconds( 2.0f );
		Destroy( gameObject );
	}

	private GameObject getClosestObject( GameObject[] objects )
	{
		GameObject closest = null;
		float closestDist = Mathf.Infinity;
		Vector3 currPos = transform.position;
		foreach( GameObject obj in objects )
		{
			Vector3 dirToTarget = obj.transform.position - currPos;
			float dSqrToTarget = dirToTarget.sqrMagnitude;
			if( dSqrToTarget < closestDist )
			{
				closestDist = dSqrToTarget;
				closest = obj;
			}
		}
		return closest;
	}

	private IEnumerator resetGoal(float waitTime)
    {
		while( !threat )
		{
			Vector3 pos = transform.position;
			float radius = 100.0f;
			goal = new Vector3(
				Random.Range( pos.x - radius, pos.x + radius ),
				Terrain.activeTerrain.SampleHeight(transform.position),
				Random.Range( pos.z - radius, pos.z + radius ));
			navMeshAgent.SetDestination(goal);
	        yield return new WaitForSeconds(waitTime);
		}
	}
}
