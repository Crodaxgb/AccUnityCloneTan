using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    Rigidbody2D rigidBodyRef;
    private float ballSpeed = 10;
    private int collisionCount = 0;

    void Awake()
    {
        //get the rigidbody reference
        rigidBodyRef = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Sets the velocity of the rigidbody component so that it can move in the specified direction
    /// </summary>
    /// <param name="direction"></param>
    public void SetVelocity(Vector3 direction)
    {

        rigidBodyRef.velocity = direction * ballSpeed;
    }

    /// <summary>
    /// In order to avoid of getting a 180 degree on a collision.
    /// This will result effected balls staying in a line.
    /// Also this will turn their colors to redish.
    /// </summary>
    public void IncreaseCollision()
    {
        collisionCount++;
        if(collisionCount % 10 == 0 && collisionCount > 0  )
        {
            rigidBodyRef.velocity += new Vector2(0, 0.1f);
            GetComponent<SpriteRenderer>().color -= new Color(0, 50, 50, 0) / 255;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IncreaseCollision();
    }
}
