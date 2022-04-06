using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWebViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject WebViewPrefab;

    private int state = 0;

    private void Awake() {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        state = 1;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        //Debug.Log(gameObject.name + "Test");
    }

    private void OnCollisionExit2D(Collision2D collision) {
        state = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
            if (state == 1)
                Instantiate(WebViewPrefab);
    }
}
