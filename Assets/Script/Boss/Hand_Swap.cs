using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Swap : MonoBehaviour
{
   // [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite _openHand,_openHandGlow, _closedHand;

    public void OpenHand()
    {
        GetComponent<SpriteRenderer>().sprite = _openHand;
    }

    public void openhandGlow()
    {
        GetComponent<SpriteRenderer>().sprite = _openHandGlow;
    }

    public void ClosedHand()
    {
        GetComponent<SpriteRenderer>().sprite = _closedHand;
    }

}
