using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

enum DebugState
{
    Normal,
    Distance,
    Vision
}

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject pickups;
    private Vector3 prevPlayerPos;
    private Vector3 playerVelocity;
    public TextMeshProUGUI closestPickupDist;
    private LineRenderer lineRenderer;
    private DebugState debugState;
    public TextMeshProUGUI debug;
    public TextMeshProUGUI PlayerPosition;
    public TextMeshProUGUI PlayerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        prevPlayerPos = player.transform.position;
        playerVelocity = Vector3.zero;
        debugState = DebugState.Normal;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
    }

    void cleanupDebug(DebugState curState)
    {
        switch (curState)
        {
            case DebugState.Distance:
                PlayerPosition.text = "";
                PlayerVelocity.text = "";
                selectPickup(pickups, -1, Color.white);
                closestPickupDist.text = "";
                break;
            case DebugState.Vision:
                selectPickup(pickups, -1, Color.white);
                lineRenderer.SetPosition(0, player.transform.position);
                lineRenderer.SetPosition(1, player.transform.position);
                break;
            default:
                break;
        }
    }

    static DebugState incrementDebug(DebugState curState)
    {
        switch (curState){
            case DebugState.Normal:
                return DebugState.Distance;
            case DebugState.Distance:
                return DebugState.Vision;
            case DebugState.Vision:
                return DebugState.Normal;
        }
        return DebugState.Normal;
    }

    void selectPickup(GameObject pickups, int index, Color highlightColor)
    {
        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            GameObject ith_pickup = pickups.transform.GetChild(i).gameObject;
            if (i == index)
            {
                ith_pickup.GetComponent<Renderer>().material.color = highlightColor;

            }
            else ith_pickup.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void runDistanceDebug()
    {
        lineRenderer.SetPosition(0, player.transform.position);
        int mindex = -1;
        float mindist = float.MaxValue;
        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            GameObject ith_pickup = pickups.transform.GetChild(i).gameObject;
            float distance = (ith_pickup.transform.position - player.transform.position).magnitude;
            if (ith_pickup.activeSelf && distance < mindist)
            {
                mindex = i;
                mindist = distance;
            }
        }

        closestPickupDist.text = "Closest Pickup Distance: " + mindist.ToString();
        if (mindist == float.MaxValue)
        {
            closestPickupDist.text = "No Pickups";
            lineRenderer.SetPosition(1, player.transform.position);
        }
        else
        {
            lineRenderer.SetPosition(1, pickups.transform.GetChild(mindex).transform.position);
        }

        selectPickup(pickups, mindex, Color.blue);
        


        PlayerPosition.text = "Position: " + player.transform.position.ToString();
        PlayerVelocity.text = "Velocity: " + playerVelocity.ToString() + " Speed: " + playerVelocity.magnitude.ToString();

    }

    void runVisionDebug()
    {
        lineRenderer.SetPosition(0, player.transform.position);
        lineRenderer.SetPosition(1, player.transform.position + playerVelocity);
        float minAngle = float.MaxValue;
        float minDist = float.MaxValue;
        int mindex = -1;
        for (int i = 0; i < pickups.transform.childCount; i++)
        {
            GameObject ith_pickup = pickups.transform.GetChild(i).gameObject;
            Vector3 playerToPickup = ith_pickup.transform.position - player.transform.position;
            float angle = Vector3.Angle(playerVelocity, playerToPickup);
            if (angle < minAngle)
            {
                minAngle = angle;
                mindex = i;
            }
            else if (angle==minAngle && playerToPickup.magnitude < minDist)
            {
                minDist = playerToPickup.magnitude;
                mindex = i;
            }
        }

        if (mindex >= 0)
        {
            pickups.transform.GetChild(mindex).transform.LookAt(player.transform);
        }

        selectPickup(pickups, mindex, Color.green);
    }

    void FixedUpdate()
    {
        Vector3 curPlayerPos = player.transform.position;
        playerVelocity = (curPlayerPos - prevPlayerPos) / Time.fixedDeltaTime;
        prevPlayerPos = curPlayerPos;

        switch (debugState)
        {
            case DebugState.Distance:
                runDistanceDebug();
                break;
            case DebugState.Vision:
                runVisionDebug();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            cleanupDebug(debugState);
            debugState = incrementDebug(debugState);
        }
    }
}
