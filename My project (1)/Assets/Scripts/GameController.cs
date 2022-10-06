using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject pickups;
    private Vector3 prevPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

        int mindex = -1;
        float mindist = 0;
        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            GameObject pickup = pickups.transform.GetChild(i).gameObject;
            float distance = (pickup.transform.position - player.transform.position).magnitude;
            if (pickup.activeSelf && distance < mindist)
            {
                mindex = i;
                mindist = distance;
            }
        }

        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            GameObject pickup = pickups.transform.GetChild(i).gameObject;
            if (i == mindex) pickup.GetComponent<Renderer>().material.color = Color.blue;
            else pickup.GetComponent<Renderer>().material.color = Color.white;
        }


        
        

    }
}
