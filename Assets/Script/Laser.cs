using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //translate laser up
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //if laser position is greater than 8 on the y distroy the object
        if (transform.position.y >= 8.0f)
        {
            Destroy(gameObject);
        }
    }
}
