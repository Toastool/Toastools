using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCutomize : MonoBehaviour
{
    [SerializeField]
    private Sprite afterItem;

    public void changeShirts () {
        GameObject.Find("Shirts").GetComponent<SpriteRenderer>().sprite = afterItem;
    }

    public void changePants() {
        GameObject.Find("Pants").GetComponent<SpriteRenderer>().sprite = afterItem;
    }
}
