using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingFireLaser : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Player _player;
    private float _distance;
    [SerializeField] private float _distanceBetween;




    private Animator _deathAnim;
    [SerializeField] private AudioClip _enemyExplosionAudio;
    [SerializeField] private AudioSource _audioSource;




    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        // this creates a defualt access to the player script
        _player = GameObject.Find("Player").GetComponent<Player>();

        //null check

        if (_player == null)
        {
            Debug.LogError("_player is null");
        }

        _deathAnim = gameObject.GetComponent<Animator>();

        if (_deathAnim == null)
        {
            Debug.LogError("_deathAnim is Null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy null");
        }
        else
        {
            _audioSource.clip = _enemyExplosionAudio;
        }


    }




    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            KamikazeTime();
        }
        //if laser position is greater than 8 on the y distroy the object
        if (transform.position.y > 5.0f || transform.position.y < -5.0f || transform.position.x < -9.0f || transform.position.x > 9.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }




    }


    public void KamikazeTime()
    {


        _distance = Vector3.Distance(transform.position, _player.transform.position);
        Vector3 direction = _player.transform.position - transform.position;

        if (_distance >= _distanceBetween)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (_distance < _distanceBetween)
        {

            _speed = 15f;
            transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);


        }


        if (transform.position.y <= -6.0f)
        {
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
        }





    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            other.transform.GetComponent<Player>().Damage();
            _speed = 0;

            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.0f);
           
        }
        
    }


}
