using UnityEngine;
using System.Collections;

/* This class will handle input for the entire game and will work off of the Game State to determine which scripts to run */
public class InputManager : MonoBehaviour {

	private CharacterControllerLogic characterControllerScript;
	private ThirdPersonCamera thirdPersonCameraScript;
	private Weapon weaponScript;
	private GameStates gameState = GameStates.Live;

	private bool paused = false;

	public enum GameStates
	{
		Live,
		Paused
	}

	// Use this for initialization
	void Start () {
		characterControllerScript = (CharacterControllerLogic) GameObject.Find ("Beta").GetComponent(typeof(CharacterControllerLogic));
//		weaponScript = (Weapon) GameObject.Find ("Weapon").GetComponent(typeof(Weapon));
		//CharacterState script = (CharacterState) collidedTransform.GetComponent(typeof(CharacterState));
	}
	
	// Update is called once per frame
	void Update () {
		/*
		switch (gameState) {
			case GameStates.Live :
				//characterControllerScript.Move();
				break;
			case GameStates.Paused :
				break;
		}*/

		if (Input.GetButtonDown("Start") == true)
		{
			if (paused) {
				paused = false;
				Time.timeScale = 1;
			}
			else {
				Time.timeScale = 0;
				paused = true;
			}
		}

	}
}
