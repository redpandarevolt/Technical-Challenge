using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	// public Button mCamBut;
	public Text mStatText;
	public UnityStandardAssets.Cameras.FreeLookCam mAutoCam;

	private GameObject[] mMonsters;
	private GameObject[] mRabbits;
	private int mCurrentCam;
	private int mMaxCam = 0;

	// Use this for initialization
	void Start () {
		mMonsters = GameObject.FindGameObjectsWithTag ("Predator");
		mRabbits = GameObject.FindGameObjectsWithTag ("Prey");
		mMaxCam = mMonsters.Length - 1;
		// mCamBut.onClick.AddListener (delegate {ButtonClicked ();});
	}


	// Update is called once per frame
	void Update () {
		updateArrays ();
		updateStatText ();
		if (Input.GetKeyDown("space"))
        {
            changeCamera();
        }
	}

	void updateArrays(){
		mMonsters = GameObject.FindGameObjectsWithTag ("Predator");
		mRabbits = GameObject.FindGameObjectsWithTag ("Prey");
	}

	void updateStatText(){
		int mRabbitCount = mRabbits.Length;
		mStatText.text = "Rabbits Alive: " + mRabbitCount;
	}

	void changeCamera() {
	//cycle monster count
		mCurrentCam++;
		if (mCurrentCam > mMaxCam)
			mCurrentCam = 0;

		mAutoCam.SetTarget(mMonsters[mCurrentCam].transform );
	}
}
