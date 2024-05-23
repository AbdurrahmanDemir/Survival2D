using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    public Vector2 minMaxXY;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player == null)
            return;

        Vector3 targetPos = player.transform.position;
        targetPos.z = -10;

        targetPos.x = Mathf.Clamp(targetPos.x, -minMaxXY.x, minMaxXY.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -minMaxXY.y, minMaxXY.y);

        transform.position=targetPos;
    }
}
