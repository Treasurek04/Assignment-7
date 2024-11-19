using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public Text waveText;
    public Text messageText;
    public Text startText;
    public GameObject player;

    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRange = 9;

    private bool gameStarted = false;
    private bool isGameOver = false;

    private int waveNumber = 1;
    private int enemyCount;

    private int enemiesToSpawn;
    public float enemySpeedIncreaseFactor = 1.1f;

    void Start()
    {
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            startText.text = "Press Spacebar to Start!";
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }

        if (waveText != null)
        {
            waveText.text = "Wave: " + waveNumber;
        }
    }

    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            gameStarted = true;
            if (startText != null) startText.gameObject.SetActive(false);
            if (messageText != null) messageText.gameObject.SetActive(false);
            SpawnEnemyWave(waveNumber);
            SpawnPowerup(1);
        }

        if (!gameStarted) return;

        if (player != null && player.transform.position.y < -10 && !isGameOver)
        {
            ShowMessage("You Lose! Press R to Restart!");
            if (Input.GetKeyDown(KeyCode.R))
            {
                isGameOver = true;
                RestartGame();
            }
        }

        if (!isGameOver)
        {
            WinGame();
            NextWave();
        }
    }

    public void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + waveNumber;
        }
    }

    public void NextWave()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0 && !isGameOver)
        {
            waveNumber++;
            UpdateWaveUI();
            enemiesToSpawn = waveNumber;
            SpawnEnemyWave(enemiesToSpawn);
            SpawnPowerup(1);

            IncreaseEnemySpeed();
        }
    }

    private void IncreaseEnemySpeed()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                enemyScript.speed *= enemySpeedIncreaseFactor;
            }
        }
    }

    public void WinGame()
    {
        if (waveNumber >= 10 && !isGameOver)
        {
            isGameOver = true;
            ShowMessage("You Win! Press R to Restart!");

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = message;
        }
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        Debug.Log("Spawning " + enemiesToSpawn + " Enemies...");
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    public void SpawnPowerup(int powerupsToSpawn)
    {
        Debug.Log("Spawning " + powerupsToSpawn + " Powerups...");
        for (int i = 0; i < powerupsToSpawn; i++)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }

    public Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
