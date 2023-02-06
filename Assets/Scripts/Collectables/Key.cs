using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickable
{
    public override void Pickup()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.CollectKey();
        UIManager.onScoreUpdate();
        
        Destroy(this.gameObject);
    }
}
