using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodge : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
       // GetComponent<Enemy>();

        //assign to animator
    }

  
    void OnTriggerEnter2D(Collider2D other)
    {
        //if other is laser destroy laser and then us 
        if (other.tag == "Player_Laser" )
        {

            transform.GetComponentInParent<Enemy>().DodgeLaser();
            StartCoroutine(DodgeCoolDown());

        }
       
    }
    IEnumerator DodgeCoolDown()
    {
        transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        transform.gameObject.SetActive(true);
    }



}
