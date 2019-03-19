using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string musicFMOD = "event:/Music";
    FMOD.Studio.EventInstance musicEv;
    FMOD.Studio.ParameterInstance pointsParam;

    public int horizmove = 0;
    public int vertimove = 0;

    [FMODUnity.EventRef]
    public string vroomvroom = "event:/Flying";
    FMOD.Studio.EventInstance vroomvroomEv;
    FMOD.Studio.ParameterInstance vroomvroomParam;

    public GameObject[] hazards;
    public GameObject boss;
    public Vector3 spawnValues;
    public Vector3 squadStrikepos = new Vector3(9, 0, -15);
    public Quaternion squadStrikerot = Quaternion.identity;
    public int hazardCount = 10;
    public float spawnWait = 0.75f;
    public float startWait;
    public float waveWait;

    public GameObject squadStrike;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;


    private float timeLimit = 9.0f;

    private float timePlayed;

    private bool boss1 = false;
    private bool boss2 = false;
    private bool boss3 = false;

    private bool squadUsed = false;

    void Start()
    {

        startWait = 10;
        musicEv = FMODUnity.RuntimeManager.CreateInstance(musicFMOD); //<<====creating instances for FMOD events and parameters
        musicEv.getParameter("pointintensity", out pointsParam);
        musicEv.start();

        vroomvroomEv = FMODUnity.RuntimeManager.CreateInstance(vroomvroom);
        vroomvroomEv.getParameter("flyfly", out vroomvroomParam);
        vroomvroomEv.start();

        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "G300\nSPACE SHOOTER\n\nMove: W-A-S-D or arrows\nShoot: mouse click\n1-use Squad Strike: space";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());

    }

    void Update()
    {

        if (Input.GetButtonDown("Horizontal"))
        {
            horizmove = 1;
            vroomvroomParam.setValue(1);

        }

        if (Input.GetButtonUp("Horizontal"))
        {
            horizmove = 0;

        }

        if (Input.GetButtonDown("Vertical"))
        {
            vertimove = 1;
            vroomvroomParam.setValue(1);

        }

        if (Input.GetButtonUp("Vertical"))
        {
            vertimove = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (squadUsed == false)
            {
                Instantiate(squadStrike, squadStrikepos, squadStrikerot * Quaternion.Euler(0f, -90f, 0f));
                squadUsed = true;
            }
            else
            {
                return;
            }
        }

        if (horizmove == 0 && vertimove == 0)
        {

            vroomvroomParam.setValue(0);
        }

        if (timeLimit > 0)
        {

            timeLimit -= Time.deltaTime;
        }
       

        timePlayed += Time.deltaTime;

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                vroomvroomEv.start();
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                gameOverText.text = "";
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore() //changes FMOD params + spawns bosses based on score
    {
        scoreText.text = "Score: " + score;

        if (score >= 250)
        {
            pointsParam.setValue(1);
            hazardCount = 20;
            spawnWait = 0.8f;
        }
        if (score >= 700)
        {
            pointsParam.setValue(2);
            hazardCount = 30;
            spawnWait = .6f;
            if (boss1 == true)
            { 
                return; 
            }
            else
            {
                CreateBoss();
                boss1 = true;
            }
        }
        if (score >= 1000)
        {
            pointsParam.setValue(3);
            hazardCount = 100;
            spawnWait = 0.4f;
            if (boss2 == true)
            {
                return;
            }
            if (boss2 == false)
            {
                CreateBoss();
                boss1 = true;
            }
        }
        if (score >= 1300)
        {
            hazardCount = 1000;
            spawnWait = 0.25f;
            if (boss3 == true)
            {
                return;
            }
            if (boss3 == false)
            {
                CreateBoss();
                boss1 = true;
            }
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
        vroomvroomEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    void CreateBoss() //creates a boss
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
        Quaternion spawnRotation = Quaternion.identity;
        Instantiate(boss, spawnPosition, spawnRotation);
        Debug.Log("Spawned Boss");
        return;
    }
}
