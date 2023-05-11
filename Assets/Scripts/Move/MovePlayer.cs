using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public int ID;
    public int speed = 5;
    private Vector2 moveInputVector = Vector2.zero;
    
   
    void Start()
    {
        
    }

  
    void Update()
    {
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
         Vector3 moveDirection = transform.up *  moveInputVector.y +
                                 transform.right * moveInputVector.x;
         // moveDirection.Normalize();
         transform.position += moveDirection * speed*Time.deltaTime;
    }
    
    
}
