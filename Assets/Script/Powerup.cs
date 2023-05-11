using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    //ID for Powers 0 = trple Shot, 1 = Speed, 2 = Shield
    [SerializeField] private int powerupID;

    //Speed
    [SerializeField] private float _speed = 3.0f;


    //Audio
    [SerializeField] private AudioClip _powerupAudio;

    private bool _moveCloser;

    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }




    // Update is called once per frame
    void Update()
    {

        if (_moveCloser==true)
        {
            if(Vector3.Distance(_player.transform.position, transform.position) > 0)
            {
                transform.position += (Vector3)( _speed * Time.deltaTime * (_player.transform.position - transform.position).normalized);
            }
        }

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            //respawn at top with a new random x position
            //Instantiate(transform.position, new Vector3(0,7,0), Quaternion.identity);
            //transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
            Destroy(this.gameObject);
        }

    }


    public void OnEnable()
    {
        EventDelegator.movePowerupsTowardsPlayer += MoveCloserTopPlayer;
        EventDelegator.dontMoveTowardsPlayer += DontMoveTowardsPlayer;
    }

    public void OnDisable()
    {
        EventDelegator.movePowerupsTowardsPlayer -= MoveCloserTopPlayer;
        EventDelegator.dontMoveTowardsPlayer -= DontMoveTowardsPlayer;
    }

    public void MoveCloserTopPlayer()
    {
        _moveCloser = true;
    }

    public void DontMoveTowardsPlayer()
    {
        _moveCloser = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            //createing local variable reference called player, calling on the player script
            Player player = other.transform.GetComponent<Player>();
            UIManager uiManager = other.transform.GetComponent<UIManager>();

           // Powerup powerup = GameObject.Find("Power_up" ).GetComponentInChildren<Powerup>();

            AudioSource.PlayClipAtPoint(_powerupAudio, transform.position);
            if(_powerupAudio != null)
            {
                Debug.LogError("Power Up Played");
            }
            //if player is !=not equal to null grab the player script
            if (player!= null)
            {
                
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
                    case 6:
                        player.NegativePowerup();
                        break;
                    case 7:
                        player.HomingFire();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }

            }
            //other.transform.GetComponent<Player>().Damage();
            Destroy(gameObject);
        }
        if (other.tag == "Enemy_Laser")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
           
        }
    }
}
