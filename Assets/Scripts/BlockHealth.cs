using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockHealth : MonoBehaviour
{
    private int health = 4;
    //Text reference which is laid on the destructable objects
    TextMeshPro textRef;
    void Start()
    {
        //Get the text reference from the object
        textRef = GetComponentInChildren<TextMeshPro>();
        //Set its text to the health's value
        textRef.text = health.ToString();        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the collided object has a 'ball' tag
        if(collision.transform.tag.Equals("Ball"))
        {
            //Call the decrease health
            DecreaseHealth();
        }
    }

    /// <summary>
    /// Decreases the destructable's health and checks if it needs to be destroyed
    /// </summary>
    private void DecreaseHealth()
    {
        health--;
        //Update the text
        textRef.text = health.ToString();
        //If the health reached zero destroy the object from the scene.
        if (health<=0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SetColor();
        }
    }

    public void SetHealth(int health)
    {
        this.health = health;
        SetColor();
    }

    public void SetColor()
    {

        if (health <= 2)
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255) / 255;
        }
        else if (health <= 10)
        {
            GetComponent<SpriteRenderer>().color = new Color(10, 100, 200, 255) / 255;
        }
        else if (health < 20)
        {
            GetComponent<SpriteRenderer>().color = new Color(60, 20, 90, 255) / 255;
        }
    }
}
