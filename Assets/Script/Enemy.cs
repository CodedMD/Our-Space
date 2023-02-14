using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down 4 meters per seconds
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        // if bottom screen 
       if (transform.position.y <= -6.0f)
        {
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f),7,0);
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //if other is player damage player then destroy us 
        if (other.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();
            Destroy(gameObject);
        }
        //if other is laser destroy laser and then us 
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }

  


}
