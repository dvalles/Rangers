using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputTester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ControllerManager test = new ControllerManager();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.anyKeyDown) {
			ControllerManager.instance.AddPlayer(ControllerInputWrapper.Buttons.Start);

//			ControllerManager.instance.AllowPlayerRemoval(ControllerInputWrapper.Buttons.B);
		}

	}
}
