using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float moveSpeed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody2D rigid2D;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //moveDirection = new Vector3(x, y, 0);
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        rigid2D.velocity = new Vector3(x, y, 0) * moveSpeed;
    }
}
