using UnityEngine;
using System.Collections;

public class SelectCubeController : MonoBehaviour 
{
    private GameController gameController;
    private int scoreValue = 1;

	// Use this for initialization
	void Start () 
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (gameController.IsWithinSelectionBounds(gameObject))
        {
            gameController.addScore(scoreValue);
            Destroy(gameObject);
        }
	}
}
