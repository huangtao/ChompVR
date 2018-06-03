using UnityEngine;
using System.Collections;

public class TutorialStart : MonoBehaviour {

	void Start () {
        ApplicationModel.GameState = GameState.Paused;
    }
	
	void StartGame () {
        ApplicationModel.GameState = GameState.Playing;
    }
}
