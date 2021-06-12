using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Here comes the Player prefab")]
    public GameObject PlayerPrefab;
    public GameObject Player;


    [Header("Here comes the enemy prefab")]
    public GameObject EnemyPrefab;

    [Header("Here are all Enemy Prefabs")]
    public List<GameObject> Enemys = new List<GameObject>();

    [Header("Is the counter of the waveround")]
    public int WaveCounter = 0;

    [Header("Is the counter of the Enemys (How much enemys on which wave)")]
    public List<int> EnemyCounterWaveBased = new List<int>();

    [Header("Enemy Spawning range,  the higher the more further")]
    public float SpawnRange = 20;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // Start Game
        Player = Instantiate(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);

        StartCoroutine(Wave());
    }
   
    public IEnumerator Wave()
    {
        //Create Enemy's in count of how much per round
        for (int i = 0; i < EnemyCounterWaveBased[WaveCounter]; i++)
        {
            SpawnEnemy();
        }

        WaveCounter++;
        yield return new WaitForSeconds(0.1f);

    }

    public void GameEnd()
    {

        SceneManager.LoadScene(0);
    }

    // Start is called before the first frame update
    private void SpawnEnemy()
    {
        //Getting two values for x and y 
        float x = Random.Range(0, SpawnRange);
        float z = Random.Range(0, SpawnRange);

        //Create's Enemy        //The create the temp position for the enemy prefab
        Enemys.Add(Instantiate(EnemyPrefab, new Vector3(x, 0.5f, z), Quaternion.identity));
    }

   
}
