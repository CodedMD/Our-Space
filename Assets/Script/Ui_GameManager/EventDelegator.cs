using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDelegator : MonoBehaviour
{
    public delegate void MovePowerupsTowardsPlayer();
    public static event MovePowerupsTowardsPlayer movePowerupsTowardsPlayer;
    public delegate void DontMoveTowardsPlayer();
    public static event DontMoveTowardsPlayer dontMoveTowardsPlayer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (movePowerupsTowardsPlayer != null)
            {
                movePowerupsTowardsPlayer();
            }
            
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (dontMoveTowardsPlayer != null)
            {
                dontMoveTowardsPlayer();
            }
                
        }
        
    }
}
