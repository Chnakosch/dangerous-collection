using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject player;
    public GameObject car;
    public GameObject zombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnZombie", 5.0f, 5.0f - (car.GetComponent<CarScript>().collectedStuff.Count * 2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnZombie() {
        GameObject randomSpawn;
        do {
            randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Count)];
        } while (Vector3.Distance(randomSpawn.transform.position, player.transform.position) < 30.0f);

        Instantiate(zombiePrefab, randomSpawn.transform.position, Quaternion.identity);
    }
}
