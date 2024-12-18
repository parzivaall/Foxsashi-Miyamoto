using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aipath;

    // Update is called once per frame
    void Update()
    {
        if(aipath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (aipath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
