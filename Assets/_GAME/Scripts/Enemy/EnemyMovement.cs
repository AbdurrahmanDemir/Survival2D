using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    Player player;



    [Header("Settings")]
    [SerializeField] private float moveSpeed;


    public void Update()
    {
        if(!player) return;
        Move();
        
    }
   
    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    private void Move()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;
    }
    
    
}
