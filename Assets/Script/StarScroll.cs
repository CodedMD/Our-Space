using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      MeshRenderer mesh =  GetComponent<MeshRenderer>();

        Material material = mesh.material;
       Vector2 offSet = material.mainTextureOffset;
        offSet.y += Time.deltaTime/10;
        material.mainTextureOffset = offSet;


    }
}
