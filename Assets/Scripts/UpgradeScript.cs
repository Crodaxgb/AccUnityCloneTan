using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Ball"))
        {
            if(gameObject.tag.Equals("HealthUpgrade"))
            {
                GameManagerScript.instance.GetHealthUpgrade();
            }
            else if(gameObject.tag.Equals("BallUpgrade"))
            {
                GameManagerScript.instance.GetBallUpgrade();
            }
            Destroy(gameObject);
        }
    }
}
