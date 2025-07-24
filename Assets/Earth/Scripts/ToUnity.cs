using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToUnity : MonoBehaviour
{
    public void ChangeColor(string color)
    {
        Image image = GetComponent<Image>();
        switch(color.ToLower())
        {
            case "red":
                image.color = Color.red;
                break;
            case "blue":
                image.color = Color.blue;
                break;
            case "yellow":
                image.color = Color.yellow;
                break;
            default:
                image.color = Color.white;
                break;
        }
    }
}
