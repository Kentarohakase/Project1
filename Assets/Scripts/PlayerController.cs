using UnityEngine;

public class PlayerController : MonoBehaviour
{
 public float speed = 6.0f;  // Player movement speed
 public float sprintSpeedMultiplier = 1.5f;  // Multiplier for sprint speed
 public float jumpSpeed = 8.0f;  // Jump height of the player
 public float gravity = 20.0f;  // Gravity applied to the player
 public float respawnHeight = -10.0f;  // Height at which the player respawns
 public float deceleration = 0.5f;  // Rate of speed deceleration
 public float acceleration = 0.5f;  // Rate of speed acceleration

 private CharacterController controller;  // Reference to the Character Controller component
 private Vector3 moveDirection = Vector3.zero;  // Current movement direction of the player
 private bool isGrounded;  // Indicates if the player is grounded
 private bool isSprinting;  // Indicates if the player is sprinting

 private void Start()
 {
  controller = GetComponent<CharacterController>();  // Get the Character Controller component
 }

 private void Update()
 {
  UpdateInput();  // Update player input
  HandleMovement();  // Handle player movement
  ApplyGravity();  // Apply gravity
  MovePlayer();  // Move the player
  CheckGrounded();  // Check if the player is grounded
 }

 private void UpdateInput()
 {
  float horizontalInput = Input.GetAxis( "Horizontal" );  // Horizontal input value (-1 to 1)
  float verticalInput = Input.GetAxis( "Vertical" );  // Vertical input value (-1 to 1)
  isSprinting = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );  // Check if the Shift key is pressed

  Vector3 targetMoveDirection = new Vector3( horizontalInput , 0 , verticalInput );  // Create target movement direction based on input
  targetMoveDirection = transform.TransformDirection( targetMoveDirection );  // Adjust the move direction based on player's rotation
  targetMoveDirection.Normalize();  // Normalize the move direction to ensure consistent speed

  float targetSpeed = speed;  // Set target speed to the default speed
  if ( isSprinting )
  {
   targetSpeed *= sprintSpeedMultiplier;  // If the player is sprinting, increase the speed based on the multiplier
  }

  moveDirection = Vector3.Lerp( moveDirection , targetMoveDirection * targetSpeed , acceleration * Time.deltaTime );  // Gradually update move direction using Lerp function
 }

 private void HandleMovement()
 {
  if ( isGrounded && Input.GetButton( "Jump" ) )
  {
   moveDirection.y = jumpSpeed;  // If the player is grounded and the jump button is pressed, add vertical speed for jumping
  }
  else if ( isGrounded )
  {
   moveDirection.y = 0;  // If the player is grounded and no movement button is pressed, set the vertical speed to 0 to keep the player grounded
  }
 }

 private void ApplyGravity()
 {
  moveDirection.y -= gravity * Time.deltaTime;  // Apply gravity to the vertical speed to pull the player downwards
 }

 private void MovePlayer()
 {
  controller.Move( moveDirection * Time.deltaTime );  // Move the player using the Character Controller
 }

 private void CheckGrounded()
 {
  isGrounded = controller.isGrounded;  // Check if the player is grounded

  if ( isGrounded && moveDirection.y < 0 )
  {
   moveDirection.y = -0.5f;  // Add a slight negative vertical speed to keep the player grounded
  }
 }

 private void Respawn()
 {
  Vector3 startPosition = Vector3.zero;  // Set the desired respawn position for the player (here: (0, 0, 0))
  controller.enabled = false;  // Disable the Character Controller to prevent interactions
  transform.position = startPosition;  // Set the player's position to the respawn position
  controller.enabled = true;  // Enable the Character Controller to enable interactions again
 }
}
