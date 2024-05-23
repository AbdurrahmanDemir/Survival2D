using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    Rigidbody2D rb;
    public MobileJoystick mobileJoystick;


    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rb.velocity = mobileJoystick.GetMoveVector() * moveSpeed * Time.deltaTime;

    }
}
