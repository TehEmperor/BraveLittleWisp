using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssController : MonoBehaviour
{
   Enemie[] enemies;
   PlayerController player;

    private void Start()
    {
        enemies = FindObjectsOfType<Enemie>();
        player = FindObjectOfType<PlayerController>();
    }


    private void FixedUpdate()
    {
        ChasePlayerLight();
    }

    private void ChasePlayerLight()
    {
        if(player == null) return;
        foreach(Enemie enemie in enemies)
        {
            enemie.RoamingUpdate(player.transform.position, player.GetLightAsFracture());
        }
        
    }

}
