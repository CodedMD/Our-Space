using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyContainer;
    [SerializeField]
    private GameObject enemyPrefab;
    private bool stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    { // start a loop
        while (stopSpawning == false)
        {
            //vector3 = enemyprefab postospawn = refernce for position to spawn
            //new Vector3(Random.Range(-8f, 8f), 7, 0) = spawn enemyprefab in this random range
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
           // spawn (enemyprefab, to postoSpwn , defualt rotation)
           GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            //enemy spawn . position. container = enemyContainer . position
            newEnemy.transform.parent = enemyContainer.transform;
           //wait for (float 5.0f)
            yield return new WaitForSeconds(5.0f);
            //yield return null;

        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }






}
