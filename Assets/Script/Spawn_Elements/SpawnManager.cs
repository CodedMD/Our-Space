using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SpawnManager : MonoBehaviour
{

    // New Wave Spawning Class 
    [System.Serializable]
    public class Waves
   {
        // this tells the wave array how tohow to arrange itself
        public string waveName;
       public int numberOfWaves;
        public GameObject[] typeOfEnemies;
        public float spawnInterval;

    }


    //the creation and display of the wave class 
    [SerializeField] private Waves[] _waves;
    // keeps track of the current waves left within each wave
    [SerializeField] private Waves _currentWave;
    //tracks what wave we are on
    [SerializeField] private int _currentWaveNumber;

    private float _nextSpawnTime;
    private bool _canSpawn = true;




    //Enemy spawn
    [SerializeField] private GameObject _enemyContainer;
    // [SerializeField] private GameObject[] _enemyPrefab;

    [System.Serializable]
    public class PowerupWaves
    {
        // this tells the wave array how tohow to arrange itself
        public string waveName;
        public int numberOfWaves;
        public GameObject[] typeOfPowerups;
        public float spawnInterval;

    }
    //Powerups
    [SerializeField] private GameObject _powerupContainer;
   // [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private PowerupWaves[] _powerups;
    // keeps track of the current waves left within each wave
    [SerializeField] private PowerupWaves _currentPowerupWave;
    [SerializeField] private int _currentPowerupWaveNumber;
    private float _powerupNextSpawnTime;
    private bool _powerupCanSpawn = true;


    //Spawn bool Switch
    private bool _stopSpawning = false;
    private bool _stopEnemySpawning = false;
    private bool _stopPowerupSpawning = false;

    //Boss Spawn
    private bool _bossCanSpawn = false;
    [SerializeField] private GameObject _bossAnubisPrefab;
    private bool _isBossOrbSpawn = false;
    [SerializeField] private GameObject _bossOrbPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (_powerupContainer == null)
        {
            Debug.LogError("powerup container null");
        }
        if (_stopSpawning == true)
        {
            Debug.LogError("We are Not Spawning");
        }

    }

    // Update is called once per frame
    void Update()
    {


        _currentPowerupWave = _powerups[_currentPowerupWaveNumber];
        GameObject[] totalPowerups = GameObject.FindGameObjectsWithTag("Power_up" );
        // current wave equals our waves current number
        _currentWave = _waves[_currentWaveNumber];
        //total enemies equals find the game object with the tag enemy 
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        //if total enemies count is zero and canspwn is false and the current wave number plus one does not equal the wave 
        if (totalEnemies.Length == 0 && !_canSpawn && _currentWaveNumber+1 != _waves.Length)
        {
            //start the next wave
            SpawnNextWave();
            
        }
        if (totalPowerups.Length == 0 && !_canSpawn && _currentPowerupWaveNumber + 1 != _waves.Length)
        {
            //start the next wave
            SpawnNextWave();
            
        }
        if (_bossCanSpawn == true)
        {
            _canSpawn = false;
            _stopEnemySpawning = true;
        }
        

    }

 
    public void SpawnNextWave()
    {
        //add one to the current wave number
        _currentWaveNumber++;
        _currentPowerupWaveNumber++;
        //you can now spawn
        _canSpawn = true;
        _powerupCanSpawn = true;

    }

 public void SpawnBossWave()
    {
        _canSpawn = false;
        _bossCanSpawn = true;
        StartCoroutine(BossCanSpawn());


    }

    IEnumerator BossCanSpawn()
    {
        yield return new WaitForSeconds(3f);
        if (_bossCanSpawn == true)
        {
            
            GameObject boss = Instantiate(_bossAnubisPrefab, transform.position, Quaternion.identity);
            boss.transform.parent = _enemyContainer.transform;
            //  _bossCanSpawn = false;
            _stopEnemySpawning = true;
        }
        _bossCanSpawn = false;

    }

    public void SpawnBossOrg()
    {
        _stopEnemySpawning = true;
         _canSpawn = false;
        _isBossOrbSpawn = true;
        StartCoroutine(BossOrbSpawn());
        StopCoroutine(BossOrbSpawn());
    }

    IEnumerator BossOrbSpawn()
    {
        yield return new WaitForSeconds(3f);
        if (_isBossOrbSpawn == true)
        {

            GameObject bossOrb = Instantiate(_bossOrbPrefab, transform.position, Quaternion.identity);
            bossOrb.transform.parent = _enemyContainer.transform;

        }
        _isBossOrbSpawn = false;

    }


    // Spawning Function
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }
    

    


    IEnumerator SpawnRoutine()
    { // start a loop

        yield return null;
              
        while (_stopEnemySpawning == false )
        {
           // if canspawn is true and the next spawn time is less than time by frame
            if (_canSpawn && _nextSpawnTime < Time.time)
            {
                //RandomEnemy means current wave of enemies is any random enemy within the array
                GameObject randomEnemy = _currentWave.typeOfEnemies[Random.Range(0, _currentWave.typeOfEnemies.Length)];
                //Postospawn means any creation should be made within the x-axis between -8f, 8f, and the y-axis 7, z-axis 0
                Vector3 posToSpawn = new Vector3(Random.Range(-5f, 5f), 5, 0);
                //newenmy means create random enemy at the postospawn with no rotation
                GameObject newEnemy = Instantiate(randomEnemy, posToSpawn, Quaternion.identity);
                //Spawn enemies in enemy container
                newEnemy.transform.parent = _enemyContainer.transform;
                //whatever the current wave is subtract one from the number of wave within a wave.
                _currentWave.numberOfWaves--;
                //next spawn time means current frame plus current wave and the set spawn interval 
                _nextSpawnTime = Time.time + _currentWave.spawnInterval;
                //Once the current wave number is zero 
                if (_currentWave.numberOfWaves == 0)
                {
                    //You can not spawn
                    _canSpawn = false;
                }
            }
           
            yield return new WaitForSeconds(5.0f);
           
           
        }
       
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_stopPowerupSpawning == false)
        {
            if (_powerupCanSpawn && _powerupNextSpawnTime < Time.time)
            {
                
                GameObject randomPowerups = _currentPowerupWave.typeOfPowerups[Random.Range(0, _currentPowerupWave.typeOfPowerups.Length)];
                
                Vector3 posToSpawn = new Vector3(Random.Range(-5f, 5f), 5, 0);
              
                GameObject newPowerups = Instantiate(randomPowerups, posToSpawn, Quaternion.identity);
             
                newPowerups.transform.parent = _powerupContainer.transform;
              
                _currentPowerupWave.numberOfWaves--;
               
                _powerupNextSpawnTime = Time.time + _currentPowerupWave.spawnInterval;
               
                //Once the current wave number is zero 
                if (_currentPowerupWave.numberOfWaves == 0)
                {
                    //You can not spawn
                    _powerupCanSpawn = false;
                }
            }
            yield return new WaitForSeconds(5.0f);
        }
       
    }


    //Players Death Functions
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }






}
