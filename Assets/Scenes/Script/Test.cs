using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    //상호작용 확인용 스크립트 였습니다.(필요없음)
    [SerializeField]
    private Color color;
    private SpriteRenderer spriteRenderer;

    private int state = 0;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        state = 1;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        //Debug.Log(gameObject.name + "Test");
    }

    private void OnCollisionExit2D(Collision2D collision) {
        spriteRenderer.color = Color.white;
        state = 0;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (state == 1) {
                spriteRenderer.color = color;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z)) {
            if (spriteRenderer.color == color) {
                spriteRenderer.color = Color.white;
            }
        }
    }
}
