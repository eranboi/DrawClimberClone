using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  Transform LegSlotLeft, LegSlotRight;
  new Rigidbody rigidbody;

  void Awake(){
    rigidbody = GetComponent<Rigidbody>();
  }
  
  void Start()
  {
    LegSlotLeft = transform.GetChild(0);
    LegSlotRight = transform.GetChild(1);
    
    rigidbody.isKinematic = true;

    LineToMesh.DrawingEndedAction += StartGame;
  }

  void StartGame(){
    rigidbody.isKinematic = false;
  }









}
