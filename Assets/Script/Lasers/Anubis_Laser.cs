using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Anubis_Laser : MonoBehaviour
{

    private EdgeCollider2D edgeCollider;
    private LineRenderer myLine;
    //[SerializeField]private float _speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = this.GetComponent<EdgeCollider2D>();
        myLine = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      
        SetEdgeCollider(myLine);
    }
    void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point < lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }

        edgeCollider.SetPoints(edges);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage();




        }
    }



}
