using UnityEngine;
using System.Collections;

public class MinimapController : MonoBehaviour 
{
    private int scoreValue = 1;
    private GameController gameController;
    public GameObject explosion;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }


    void OnMouseOver()
    {
        if (!gameController.getGameOver())
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameController.addScore(scoreValue);
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);
                gameController.updateClickSucceed();
            }
        }
    }
}
