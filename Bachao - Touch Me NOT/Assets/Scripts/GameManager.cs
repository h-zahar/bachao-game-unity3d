using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject powerupPrefab;

    private float xBound = 13;
    private float zBound = 13;
    private float zSpawnPos = 14;

    private int enemyCount;
    public int waveCount = 1;
    private int startSpawningPowerup = 5;


    public static int score;

    private TextMeshProUGUI showGameName;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI gameOverText;
    private TextMeshProUGUI showScoreText;

    private Button startButton;
    private Button quitButton;
    private Button mainMenuButton;

    public bool isGameOver = false;
    public bool isGameOn = false;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        showGameName = GameObject.Find("Show Game Name").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
        gameOverText = GameObject.Find("Game Over Text").GetComponent<TextMeshProUGUI>();
        showScoreText = GameObject.Find("Show Score Text").GetComponent<TextMeshProUGUI>();

        startButton = GameObject.Find("Start Button").GetComponent<Button>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        mainMenuButton = GameObject.Find("Main Menu Button").GetComponent<Button>();

        showGameName.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        StartCoroutine(PrePlay());



        startButton.onClick.AddListener(StartGame);


        InitiateScreen();

    }

    IEnumerator PrePlay()
    {
        yield return new WaitForSeconds(1.5f);

        showGameName.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        startButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        isGameOn = true;

        showGameName.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);


        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGameOn)
        {
            SpawnManager();

            if (isGameOver)
            {

                ClearScreenAtEnd();

            }

        }


    }



    public void SpawnManager()
    {

        enemyCount = FindObjectsOfType<EnemyBehavior>().Length;

        if (enemyCount == 0 && !isGameOver)
        {
            if (waveCount >= startSpawningPowerup)
            {
                SpwanPowerupPrefabWave();
            }

            for (int i = 0; i < waveCount; i++)
            {
                SpawnEnemyPrefabsWave();
            }
            waveCount++;
        }


    }


    void SpawnEnemyPrefabsWave()
    {
        int minIndex = 0;
        int maxIndex = 6;
        int generateIndex = GenerateIndex(minIndex, maxIndex);

        if (generateIndex >= (maxIndex / 2))
        {
            Instantiate(enemyPrefabs[generateIndex], XGenerateRandomPos(), enemyPrefabs[generateIndex].transform.rotation);
        }

        if (generateIndex < (maxIndex / 2))
        {
            Instantiate(enemyPrefabs[generateIndex], negXGenerateRandomPos(), enemyPrefabs[generateIndex].transform.rotation);
        }

    }

    void SpwanPowerupPrefabWave()
    {
        Instantiate(powerupPrefab, GenerateRandomPos(), powerupPrefab.transform.rotation);
    }

    int GenerateIndex(int minIndex, int maxIndex)
    {
        return Random.Range(minIndex, maxIndex);
    }

    float XGeneratePos()
    {
        return Random.Range(-xBound, xBound);
    }

    float ZGeneratePos()
    {
        return Random.Range(-zBound, zBound);
    }

    Vector3 XGenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, zSpawnPos);
    }

    Vector3 negXGenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, -zSpawnPos);
    }

    Vector3 GenerateRandomPos()
    {
        return new Vector3(XGeneratePos(), 0.5000001f, ZGeneratePos());
    }


    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;

        scoreText.text = "Score: " + score;
    }


    private void InitiateScreen()
    {
        for (int i = 0; i < 6; i++)
        {

            enemyPrefabs[i].SetActive(true);

        }


        mainMenuButton.gameObject.SetActive(false);
    }

    private void ClearScreenAtEnd()
    {

        StartCoroutine(ClearAfterSeconds());

    }

    IEnumerator ClearAfterSeconds()
    {
        yield return new WaitForSeconds(2.4f);
        Destroy(GameObject.FindWithTag("Enemy"));
        Destroy(GameObject.FindWithTag("Powerup"));
        scoreText.text = "";
        gameOverText.text = "Game Over";

        yield return new WaitForSeconds(2.5f);
        showScoreText.text = "Score: " + score;

        yield return new WaitForSeconds(1);
        mainMenuButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

    }




    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exitGame()
    {
        Application.Quit();
    }



}