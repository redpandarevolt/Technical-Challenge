using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject itemToSpawn;
	public float frequency;
	public float spawnDistance;

	// Use this for initialization
	void Start ()
	{
        StartCoroutine( spawnItem() );
	}

	private IEnumerator spawnItem()
    {
		while( true )
		{
			Vector3 spawnPos = transform.position;
			float posX = Random.Range( spawnPos.x - spawnDistance, spawnPos.x + spawnDistance );
			float posZ = Random.Range( spawnPos.z - spawnDistance, spawnPos.z + spawnDistance );
			float posY = transform.position.y + 1; // Terrain.activeTerrain.SampleHeight( new Vector3( posX, spawnPos.y, posZ ) );
			spawnPos = new Vector3( posX, posY, posZ );
			Instantiate( itemToSpawn, spawnPos, Quaternion.Euler(
				new Vector3( 0, Random.Range(0, 360), 0) ) );
	        yield return new WaitForSeconds(frequency);
		}
	}
}
