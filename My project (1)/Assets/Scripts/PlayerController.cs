using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Vector2 moveValue;
    public float speed;
    private int count;
    private int numPickups = 8;
    private Vector3 prevPlayerPos;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;




    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
        prevPlayerPos = transform.position;
    }

    void FixedUpdate() {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

        
    }

    void OnMove(InputValue value) {
        moveValue = value.Get<Vector2>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if(count >= numPickups){
            winText.text = "You win!";
        }
    }
}