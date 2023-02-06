using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Pickable
{
    
    
    public override void Pickup()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.OpenTreasure();
        
        Destroy(this.gameObject);
    }
}
