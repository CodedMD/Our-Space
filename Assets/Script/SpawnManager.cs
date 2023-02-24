using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyContainer;
    //[SerializeField] private GameObject _powerupContainer;
    [SerializeField]
    private GameObject _enemyPrefab;
    private bool _stopSpawning = false;
    [SerializeField] private GameObject [] powerUps;
    //[SerializeField] private GameObject _speedpowerUpPrefab;
    //[SerializeField] private GameObject _shieldpowerUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSpawning()
    {
       
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnRoutine()
    { // start a loop
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            
            //vector3 = enemyprefab postospawn = refernce for position to spawn
            //new Vector3(Random.Range(-8f, 8f), 7, 0) = spawn enemyprefab in this random range
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
           // spawn (enemyprefab, to postoSpwn , defualt rotation)
           GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            //enemy spawn . position. container = enemyContainer . position
            newEnemy.transform.parent = _enemyContainer.transform;
           //wait for (float 5.0f)
            yield return new WaitForSeconds(5.0f);
            //yield return null;

        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
           
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerups = Random.Range(0, 4);
            Instantiate(powerUps[randomPowerups], posToSpawn, Quaternion.identity);
            //newPowerUp.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }






}
