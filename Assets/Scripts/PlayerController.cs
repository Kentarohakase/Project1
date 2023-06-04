using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
 public float speed = 6.0F;
 public float jumpSpeed = 8.0F;
 public float gravity = 20.0F;
 public float respawnHeight = -10.0F;

 private Vector3 moveDirection = Vector3.zero;
 private Vector3 startPosition;
 private CharacterController controller;

 void Start()
 {
  // Speichert die Startposition und den Charakter-Controller
  startPosition = transform.position;
  controller = GetComponent<CharacterController>();
 }

 void Update()
 {
  if ( controller.isGrounded )
  {
   moveDirection = new Vector3( Input.GetAxis( "Horizontal" ) , 0 , Input.GetAxis( "Vertical" ) );
   moveDirection = transform.TransformDirection( moveDirection );
   moveDirection *= speed;

   if ( Input.GetButton( "Jump" ) )
   {
    moveDirection.y = jumpSpeed;
   }
  }

  moveDirection.y -= gravity * Time.deltaTime;
  controller.Move( moveDirection * Time.deltaTime );

  if ( transform.position.y < respawnHeight )
  {
   transform.position = startPosition;
  }
 }
}
