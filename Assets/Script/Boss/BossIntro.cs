using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossIntro : MonoBehaviour
{
    private float _speed = 3f;
 
    private SpawnManager _spawnManager;
    [SerializeField] Sprite _orbOne, _orbTwo;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(14, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        BossStart();
    }

    public void BossStart()
    {
        StartCoroutine(OrbBossintro());
    }

    IEnumerator OrbBossintro()
    {
        transform.gameObject.SetActive(true);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x <= 0)
        {
            _speed = 0;
            yield return new WaitForSeconds(2);
            GetComponent<SpriteRenderer>().sprite = _orbTwo;
            yield return new WaitForSeconds(1);
            _speed = 7;
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if(transform.position.y >= 7.5)
        {
            Destroy(gameObject);
            _spawnManager.SpawnBossWave();
        }
    }
}
