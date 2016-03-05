using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour 
{
    // game design
    public float startWait = 3.0f;
    public float spawnWait;
    public float gameDuration;

    // game GUI (score, restart, accuracy)
    public GUIText scoreText;
    public GUIText countdownText;
    public GUIText restartText;
    public GUIText accuracyText;
    private int score = 0;
    private float accuracy = 0.0f;
    private float nbClickSucceed = 0.0f;
    private float nbClick = 0.0f;

    // game objects
    public GameObject leftClickCube;
    public GameObject rightClickCube;
    public GameObject minimapSquare;
    public GameObject selectionCube;

    // game info
    private float timeElapsed = 0.0f;
    private bool restart = false;
    private bool quit = false;
    private bool gameOver = false;
    private int isWaves = 0;

    // selection rectangle
    bool isSelecting = false;
    Vector3 mousePosition1;

    void Awake()
    {
        spawnWait = PlayerPrefs.GetFloat("spawnDelay");
        isWaves = PlayerPrefs.GetInt("checked");

        Debug.Log("is waves : " + isWaves);
    }

	void Start () 
    {
        StartCoroutine(SpawnWaves());
        restartText.text = "";
        countdownText.text = "";
	}
	
	void Update () 
    {
        if(timeElapsed < gameDuration)
        {
            // Starting countdown
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0)
            {
                countdownText.text = "3";
                countdownText.fontSize = 34;
            }
            if (timeElapsed > 1)
            {
                countdownText.text = "2";
                countdownText.fontSize = 40;
            }
            if (timeElapsed > 2)
            {
                countdownText.text = "1";
                countdownText.fontSize = 50;
            }
            if (timeElapsed > 3)
            {
                countdownText.text = "GO !";
            }
            if (timeElapsed > 3.5)
                countdownText.text = "";
        }

        // restart level
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }

        // quit level back to menu
        if (quit)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Application.LoadLevel("_menu");
            }
        }

        // accuracy
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            nbClick++;
            updateAccuracy();
        }

        // selection rectangle
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
            isSelecting = false;
	}
    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Assets.Scripts.Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Assets.Scripts.Utils.DrawScreenRect(rect, new Color(0.5f, 0.8f, 0.5f, 0.25f));
            Assets.Scripts.Utils.DrawScreenRectBorder(rect, 2, new Color(0.5f, 0.8f, 0.5f));
        }
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
        var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        bool res = false;
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        // is each corner of the cube inside the selection ?
        bool contain1 = viewportBounds.Contains(camera.WorldToViewportPoint(new Vector3(gameObject.transform.position.x - (gameObject.transform.localScale.x / 2), gameObject.transform.position.y - (gameObject.transform.localScale.y / 2), 0)));
        bool contain2 = viewportBounds.Contains(camera.WorldToViewportPoint(new Vector3(gameObject.transform.position.x + (gameObject.transform.localScale.x / 2), gameObject.transform.position.y + (gameObject.transform.localScale.y / 2), 0)));
        bool contain3 = viewportBounds.Contains(camera.WorldToViewportPoint(new Vector3(gameObject.transform.position.x - (gameObject.transform.localScale.x / 2), gameObject.transform.position.y + (gameObject.transform.localScale.y / 2), 0)));
        bool contain4 = viewportBounds.Contains(camera.WorldToViewportPoint(new Vector3(gameObject.transform.position.x + (gameObject.transform.localScale.x / 2), gameObject.transform.position.y - (gameObject.transform.localScale.y / 2), 0)));

        if (contain1 && contain2 && contain3 && contain4)
            res = true;

        return res;
    }

    
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);

        if (isWaves == 0) // if continuous spawn (spawn once every spawnWait)
        {
            Debug.Log("waves = 0");
            while (timeElapsed < gameDuration)
            {
                Debug.Log("spawn");
                spawnItem();

                yield return new WaitForSeconds(spawnWait);
            }
        }
        else // if waves spawn (spawn 3 then wait for user to clear the waves)
        {
            Debug.Log("nbClickSucceed : " + nbClickSucceed);
            
            do
            {
                float temp = score;
                spawnItem();
                spawnItem();
                spawnItem();
                yield return new WaitForSeconds(0.05f);

                Debug.Log("spawn x3");


                while (temp + 3 != score)
                {
                    yield return 0;
                }


            } while (timeElapsed < gameDuration);
        }
        

        gameOver = true;
        restart = true;
        quit = true;
        restartText.text = "Press 'R' to restart\nand 'Q' to get back to menu";
    }

    void spawnItem()
    {
        Vector3 spawnPositionSquare = new Vector3(Random.Range(-9.25f, 9.1f), Random.Range(5.0f, -2.2f), -1.0f);
        Vector3 spawnPositionSelectionSquare = new Vector3(Random.Range(-8.76f, 8.76f), Random.Range(4.54f, -1.67f), -1.0f);
        Vector3 spawnPositionMinimap = new Vector3(Random.Range(-9.1f, -6.85f), Random.Range(-5.0f, -2.9f), -1.0f);

        float rand = Random.value;
        if (rand < 0.3f)
        {
            Instantiate(leftClickCube, spawnPositionSquare, Quaternion.identity);
        }
        else if (rand < 0.5f && rand > 0.3f)
        {
            Instantiate(selectionCube, spawnPositionSelectionSquare, Quaternion.identity);
        }
        else if (rand < 0.7f && rand > 0.5f)
        {
            Instantiate(rightClickCube, spawnPositionSquare, Quaternion.Euler(0.0f, 0.0f, 45));
        }
        else
        {
            Instantiate(minimapSquare, spawnPositionMinimap, Quaternion.identity);
        }
    }

    void updateScore()
    {
        scoreText.text = "Score : " + score;
    }

    public void updateClickSucceed()
    {
        nbClickSucceed++;
    }

    public void updateAccuracy()
    {
        accuracy = nbClickSucceed / nbClick;
        accuracyText.text = "Accuracy : " + accuracy * 100 + "%";
    }

    public void addScore(int newScoreValue)
    {
        score += newScoreValue;
        updateScore();
    }

    public bool getGameOver()
    {
        return gameOver;
    }

    public void setValue(float spawnDelay)
    {
        spawnWait = spawnDelay;
    }

    public bool getIsSelecting()
    {
        return isSelecting;
    }
}
