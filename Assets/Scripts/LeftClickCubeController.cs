using UnityEngine;
using System.Collections;

public class LeftClickCubeController : MonoBehaviour 
{
    private int scoreValue = 1;
    private GameController gameController;
    public GameObject explosion;

    private float radius = 2.5f;
    private float force = 1.0f;

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
        if(!gameController.getGameOver())
        {
            if(Input.GetMouseButtonDown(0))
            {
                
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                foreach (Collider c in colliders)
                {
                    if (c.GetComponent<Rigidbody>() == null) continue;

                    else
                    {
                        c.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, 1, ForceMode.Impulse);
                    }
                }
                
                gameController.addScore(scoreValue);
                Destroy(gameObject);
                Instantiate(explosion, transform.position, transform.rotation);
                gameController.updateClickSucceed();
            }
        }
    }
}
