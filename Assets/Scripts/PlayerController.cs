using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
 // Configurable parameters for speed, jump height, gravity, respawn height, acceleration rate, and deceleration rate
 public float speed = 6.0F;
 public float jumpSpeed = 8.0F;
 public float gravity = 20.0F;
 public float respawnHeight = -10.0F;
 public float acceleration = 0.5f;
 public float deceleration = 0.5f;
 

 private Vector3 moveDirection = Vector3.zero;  // The current movement direction
 private Vector3 startPosition;  // The original position for respawning
 private CharacterController controller;  // Reference to the character controller
 private float horizontalInput;  // Current horizontal input (from -1 to 1)
 private float verticalInput;  // Current vertical input (from -1 to 1)
 private float currentSpeed = 0.0f;  // Current speed, will increase with acceleration

 void Start()
 {
  // Save the starting position and get the character controller
  startPosition = transform.position;
  controller = GetComponent<CharacterController>();
 }
 private void Update()
 {
  // Check if the player has fallen below the respawn height
  if ( transform.position.y < respawnHeight )
  {
   Respawn();
  }
 }
 void FixedUpdate()
 {
  // Update the inputs and handle movement
  UpdateInput();
  HandleMovement();



  // Execute the movement
  controller.Move( moveDirection * Time.deltaTime );
 }

 private void UpdateInput()
 {
  // Retrieve and save the horizontal and vertical input
  horizontalInput = Input.GetAxis( "Horizontal" );
  verticalInput = Input.GetAxis( "Vertical" );
 }

 private void HandleMovement()
 {
  // Prepare the move if a movement key is pressed
  if ( horizontalInput != 0 || verticalInput != 0 )
  {
   PrepareMove();
  }
  // Decelerate the move if the player is on the ground and no movement key is pressed
  else if ( controller.isGrounded )
  {
   Decelerate();
  }

  // Jump if the player is on the ground and the jump key is pressed
  if ( controller.isGrounded && Input.GetButton( "Jump" ) )
  {
   Jump();
  }

  // Apply gravity
  ApplyGravity();
 }

 private void PrepareMove()
 {
  // Calculate the desired movement direction based on player input
  Vector3 targetMoveDirection = new Vector3( horizontalInput , 0 , verticalInput );
  targetMoveDirection.Normalize();
  targetMoveDirection = transform.TransformDirection( targetMoveDirection );
  targetMoveDirection *= currentSpeed;

  // Increase current speed based on acceleration
  currentSpeed += acceleration * Time.deltaTime;
  currentSpeed = Mathf.Min( currentSpeed , speed );  // Cap current speed at the defined speed

  // Update the horizontal and vertical components of the movement direction
  moveDirection.x = targetMoveDirection.x;
  moveDirection.z = targetMoveDirection.z;
 }

 private void Jump()
 {
  // Set the vertical component of the movement direction to the jump height
  moveDirection.y = jumpSpeed;
 }

 private void ApplyGravity()
 {
  // Apply gravity to the vertical component of the movement direction
  moveDirection.y -= gravity * Time.deltaTime;
 }

 private void Respawn()
 {
  // Reset the player position to the starting position
  transform.position = startPosition;
 }

 private void Decelerate()
 {
  // Decelerate the horizontal and vertical components of the movement direction
  moveDirection.x = Mathf.Lerp( moveDirection.x , 0 , deceleration * Time.deltaTime );
  moveDirection.z = Mathf.Lerp( moveDirection.z , 0 , deceleration * Time.deltaTime );
  // Also decelerate the current speed
  currentSpeed = Mathf.Lerp( currentSpeed , 0 , deceleration * Time.deltaTime );
 }
}
