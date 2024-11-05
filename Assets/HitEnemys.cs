using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemys : MonoBehaviour
{

    public void HitByPlayer()
    {
        //ScoreManager.instance.AddScore(1);
        Destroy(gameObject);
    }
}
