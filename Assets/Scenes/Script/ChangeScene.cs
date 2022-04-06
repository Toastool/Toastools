using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private int state = 0;

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
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (state == 1) {
                SceneManager.LoadScene("OtherScene");
            }
        }
    }
}