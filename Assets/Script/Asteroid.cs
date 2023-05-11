using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Asteroid : MonoBehaviour
{
    //Speed
    [SerializeField] float _speed = 3f;
    [SerializeField] float _rotationSpeed = 15f;


    [SerializeField] private GameObject _explosionPrefab;


    //Other Scripts Hooks

    // Start is called before the first frame update
    void Start()
    {
        //impulseSource = GetComponent<CinemachineImpulseSource>();
        //
    }




    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RainJudgement());
        // if bottom screen 
       
       
    }

    IEnumerator RainJudgement()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime, Space.Self);
        yield return new WaitForSeconds(3.0f);
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        if (transform.position.y <= -6.0f)
        {

            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            Destroy(this.gameObject);

        }

    }




    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            
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
            

        }
        if(other.tag == "Wipe_Out_Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.1f);
            
        }

    }

    
}
