using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
 public GameObject platformPrefab;
 public GameObject player;
 public float spawnRate = 1.5f;
 public float minGap = 1.0f;
 public float maxGap = 3.0f;

 private float platformDepth;
 private Vector3 nextSpawnPoint;

 // Start wird vor dem ersten Bildupdate aufgerufen
 void Start()
 {
  // Die Tiefe der Plattform ermitteln
  platformDepth = platformPrefab.GetComponent<BoxCollider>().size.z;

  // Der erste Spawn-Punkt ist der Anfangspunkt des Spielers
  nextSpawnPoint = player.transform.position;

  // Die erste Plattform erzeugen
  SpawnPlatform();
 }

 // Update wird einmal pro Frame aufgerufen
 void Update()
 {
  // Wenn der Spieler nahe genug an der nächsten Spawnposition ist, spawnen wir die nächste Plattform
  if ( player.transform.position.z > nextSpawnPoint.z - platformDepth )
  {
   SpawnPlatform();
  }
 }

 void SpawnPlatform()
 {
  // Erzeuge eine neue Plattform am nächsten Spawn-Punkt
  GameObject newPlatform = Instantiate( platformPrefab , nextSpawnPoint , Quaternion.identity );

  // Erhöhe den nächsten Spawn-Punkt um die Tiefe der Plattform
  nextSpawnPoint.z += platformDepth + Random.Range(minGap, maxGap);
 }
}
