using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    public void HitByPlayer()
    {
        ScoreManager.instance.AddScore(1);
        Destroy(gameObject);
    }
}

