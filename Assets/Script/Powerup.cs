using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Powerup : MonoBehaviour
{
    //ID for Powers 0 = trple Shot, 1 = Speed, 2 = Shield
    [SerializeField] private int powerupID;

    //Speed
    [SerializeField] private float _speed = 3.0f;


    //Audio
    [SerializeField] private AudioClip _powerupAudio;
   




    // Start is called before the first frame update
    void Start()
    {

        
    }




    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            //transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            Destroy(gameObject);
        }

    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //createing local variable reference called player, calling on the player script
            Player player = other.transform.GetComponent<Player>();
            UIManager uiManager = other.transform.GetComponent<UIManager>();



            AudioSource.PlayClipAtPoint(_powerupAudio, transform.position);
            if(_powerupAudio != null)
            {
                Debug.LogError("Power Up Played");
            }
            //if player is !=not equal to null grab the player script
            if (player!= null)
            {
                //if powerup id is 0
                //if (powerupID == 0)
                {
                //    player.TripleShotActive();
                }
                //else if powerup is 1
              //  else if (powerupID == 1)
                {
                    //play speed powerup
               //     Debug.Log(" Play speed");
                }
                
                //else if powerup is 2
               // else if (powerupID == 2)
                {
                    // play shield powerup
               //    Debug.Log(" Play Shield");
                }
                
                switch (powerupID)
                {
                    case 0:
                       // _audioSource.Play();
                        player.TripleShotActive();
                        break;
                    case 1:
                       // _audioSource.Play();
                        player.SpeedupPowerupActive();
                         break;
                    case 2:
                        //_audioSource.Play();
                        player.ShieldPowerupActive();
                        //uiManager.UpdateShield(3);
                        break;
                    case 3:
                        player.LifePowerupActive();
                        break;
                    case 4:
                        player.AmmoPowerupActive();
                        break;
                    case 5:
                        player.WipeOutPowerupActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;


                }

            }
            //other.transform.GetComponent<Player>().Damage();
            Destroy(gameObject);
        }
    }
}
