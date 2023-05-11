using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Asteroid_Spawn : MonoBehaviour
{
    //Speed
    [SerializeField] float _speed = 15f;
    

     //private Animator _asteroidAnim;
    [SerializeField] private GameObject _explosionPrefab;


    //Other Scripts Hooks
    private SpawnManager _spawnManager;
   




    // Start is called before the first frame update
    void Start()
    {
        //impulseSource = GetComponent<CinemachineImpulseSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("_spawnManager is Null on Astroid");
        }
    }




    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
       
        // if bottom screen 
        if (transform.position.y <= -6.0f)
        {
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            
        }
        // rotate object on the zed axis
    }




    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            //CameraShake._instance.CameraShakeTrigger(impulseSource);
            other.transform.GetComponent<Player>().Damage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
           
            Destroy(gameObject,.1f);
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser")
        {
            //CameraShake._instance.CameraShakeTrigger(impulseSource);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject,0.1f);
            _spawnManager.StartSpawning();

        }
        if(other.tag == "Wipe_Out_Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
           // Destroy(other.gameObject);
            Destroy(gameObject, 0.1f);
            _spawnManager.StartSpawning();
        }

    }

    
}
