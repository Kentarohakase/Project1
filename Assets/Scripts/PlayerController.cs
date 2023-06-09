using UnityEngine;

public class PlayerController : MonoBehaviour
{
 public float speed = 6.0f;  // Geschwindigkeit des Spielers
 public float sprintSpeedMultiplier = 1.5f;  // Multiplikator für die Sprintgeschwindigkeit
 public float jumpSpeed = 8.0f;  // Sprunghöhe des Spielers
 public float gravity = 20.0f;  // Gravitation des Spielers
 public float respawnHeight = -10.0f;  // Höhe, bei der der Spieler wiederbelebt wird
 public float deceleration = 0.5f;  // Verzögerung der Geschwindigkeitsabnahme
 public float acceleration = 0.5f;  // Beschleunigung der Geschwindigkeitszunahme

 private CharacterController controller;  // Referenz auf den Character Controller
 private Vector3 moveDirection = Vector3.zero;  // Aktuelle Bewegungsrichtung des Spielers
 private bool isGrounded;  // Gibt an, ob der Spieler den Boden berührt
 private bool isSprinting;  // Gibt an, ob der Spieler sprintet

 private void Start()
 {
  controller = GetComponent<CharacterController>();  // Character Controller-Komponente abrufen
 }

 private void Update()
 {
  UpdateInput();  // Aktualisiere die Spielersteuerung
  HandleMovement();  // Behandle die Spielerbewegung
  ApplyGravity();  // Wende die Gravitation an
  MovePlayer();  // Bewege den Spieler
  CheckGrounded();  // Überprüfe, ob der Spieler den Boden berührt
 }

 private void UpdateInput()
 {
  float horizontalInput = Input.GetAxis( "Horizontal" );  // Horizontaler Eingabewert (-1 bis 1)
  float verticalInput = Input.GetAxis( "Vertical" );  // Vertikaler Eingabewert (-1 bis 1)
  isSprinting = Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift );  // Überprüfe, ob die Shift-Taste gedrückt wird

  Vector3 targetMoveDirection = new Vector3( horizontalInput , 0 , verticalInput );  // Zielflugrichtung basierend auf der Eingabe erstellen
  targetMoveDirection = transform.TransformDirection( targetMoveDirection );  // Die Flugrichtung entsprechend der Spielerrotation anpassen
  targetMoveDirection.Normalize();  // Normalisiere die Flugrichtung, um eine gleichmäßige Geschwindigkeit sicherzustellen

  float targetSpeed = speed;  // Zielspeed auf die Standardgeschwindigkeit setzen
  if ( isSprinting )
  {
   targetSpeed *= sprintSpeedMultiplier;  // Wenn der Spieler sprintet, wird die Geschwindigkeit entsprechend des Multiplikators erhöht
  }

  moveDirection = Vector3.Lerp( moveDirection , targetMoveDirection * targetSpeed , acceleration * Time.deltaTime );  // Bewegungsrichtung mithilfe der Lerp-Funktion allmählich aktualisieren
 }

 private void HandleMovement()
 {
  if ( isGrounded && Input.GetButton( "Jump" ) )
  {
   moveDirection.y = jumpSpeed;  // Wenn der Spieler auf dem Boden ist und die Sprungtaste gedrückt wird, wird eine vertikale Geschwindigkeit für den Sprung hinzugefügt
  }
  else if ( isGrounded )
  {
   moveDirection.y = 0;  // Wenn der Spieler auf dem Boden ist und keine Bewegungstaste gedrückt wird, wird die vertikale Geschwindigkeit auf 0 gesetzt, um den Spieler stabil zu halten
  }
 }

 private void ApplyGravity()
 {
  moveDirection.y -= gravity * Time.deltaTime;  // Gravitation auf die vertikale Geschwindigkeit anwenden, um den Spieler nach unten zu ziehen
 }

 private void MovePlayer()
 {
  controller.Move( moveDirection * Time.deltaTime );  // Den Spieler mithilfe des Character Controllers bewegen
 }

 private void CheckGrounded()
 {
  isGrounded = controller.isGrounded;  // Überprüfen, ob der Spieler den Boden berührt

  if ( isGrounded && moveDirection.y < 0 )
  {
   moveDirection.y = -0.5f;  // Eine leichte negative vertikale Geschwindigkeit hinzufügen, um den Spieler auf dem Boden zu halten
  }
 }

 private void Respawn()
 {
  Vector3 startPosition = Vector3.zero;  // Die gewünschte Startposition für die Wiederbelebung des Spielers festlegen (hier: (0, 0, 0))
  controller.enabled = false;  // Den Character Controller deaktivieren, um Interaktionen zu verhindern
  transform.position = startPosition;  // Die Position des Spielers auf die Startposition setzen
  controller.enabled = true;  // Den Character Controller aktivieren, um Interaktionen wieder zu ermöglichen
 }
}
