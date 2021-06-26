using System;
using System.Collections.Generic;
using UnityEngine;

public class LineToMesh : MonoBehaviour
{
  [SerializeField]Vector3[] LRPositions = {};
  LineRenderer lineRenderer;
  [SerializeField]GameObject colliderPF;
  [SerializeField] private Material material;

  private Transform leftArmSlot, rightArmSlot;
  private GameObject legLeft, legRight;
  public List<Vector3> fingerPositions;
  private Vector3 lastMousePos;
  private Vector3 currentMousePos;
  public static Action DrawingEndedAction;

  void Awake(){
    lineRenderer = GetComponent<LineRenderer>();
  }

  void Start()
  {
    leftArmSlot = GameObject.Find("LeftArmSlot").transform;
    rightArmSlot = GameObject.Find("RightArmSlot").transform;
  }


  void Update(){
    currentMousePos = Input.mousePosition;
    currentMousePos.z = 10.63f;
    currentMousePos = Camera.main.ScreenToWorldPoint(currentMousePos);

    DrawLine();
  }
  void DrawLine(){
    
    if(Input.GetMouseButtonDown(0)){
      lineRenderer.positionCount = 0;
      fingerPositions.Clear();
      lastMousePos = currentMousePos;
      fingerPositions.Add(currentMousePos);
      lineRenderer.positionCount =  fingerPositions.Count;
      lineRenderer.SetPositions(fingerPositions.ToArray());

    }
    else if(Input.GetMouseButton(0)){
      float distance = Vector2.Distance(currentMousePos, lastMousePos);

      if(distance > .2f){
        Debug.Log(distance);
        lastMousePos = currentMousePos;

        fingerPositions.Add(lastMousePos);
        lineRenderer.positionCount = fingerPositions.Count;

        lineRenderer.SetPositions(fingerPositions.ToArray());


      }
    }
    else if(Input.GetMouseButtonUp(0)){
      ClearLegs();
      GetLineRendererPositions();
      CreateColliders();
      CreateLegComponents();

      DrawingEndedAction?.Invoke();

      //lineRenderer.enabled = false;
    }
  }

  void ClearLegs(){
    Destroy(legLeft);
    Destroy(legRight);
  }

  void GetLineRendererPositions(){
    //Debug.Log(lineRenderer.positionCount);
    LRPositions = new Vector3[lineRenderer.positionCount];
    lineRenderer.GetPositions(LRPositions);

    //Get the distance from zero
    // Substract and make it relative to the point zero.
    float deltaX = LRPositions[0].x;
    float deltaY = LRPositions[0].y;

    for (int i = 0; i < LRPositions.Length; i++)
    {
      Vector3 position = LRPositions[i];
      Vector3 newPosition = new Vector3(position.x - deltaX, position.y - deltaY, 0);

      LRPositions[i] = newPosition;
    }
    
  }

  void CreateColliders()
  {
     legLeft = new GameObject();
    legLeft.name = "Leg";
    Vector3 currentPos = new Vector3(-50,-50,-50);
    float distance = 0.5f;

    foreach(Vector3 position in LRPositions){
      
      if(currentPos != new Vector3(-50,-50,-50)){
        distance = Vector3.Distance(position, currentPos);
      }
      if(distance > .25f){

      position.Set(position.x, position.y, 0);
      Instantiate(colliderPF, position, Quaternion.identity, legLeft.transform);
      currentPos = position;
      }
    }

    //Clone the first one and rotate it 180 deg. around the z-axis to it'll look like the other leg.
    legRight = Instantiate(legLeft, Vector3.zero, Quaternion.Euler(0, 0, 180));

    //Set them as childs of the arm slots
    legRight.transform.parent = rightArmSlot;
    legLeft.transform.parent = leftArmSlot;

    legRight.transform.localScale = new Vector3(1, 1, 1);
    legLeft.transform.localScale = new Vector3(1, 1, 1);

    //Legs position
    legRight.transform.localPosition = new Vector3(0.2f, 0.2f, 0.2f);
    legLeft.transform.localPosition = new Vector3(0.2f, 0.2f, 0.2f);


  }

  void CreateLegComponents(){
    legLeft.AddComponent<LineRenderer>().positionCount = lineRenderer.positionCount;
    legLeft.GetComponent<LineRenderer>().useWorldSpace = false;
    legLeft.GetComponent<LineRenderer>().SetPositions(LRPositions);
    legLeft.GetComponent<LineRenderer>().startWidth = .2f;
    legLeft.GetComponent<LineRenderer>().endWidth = .2f;
    legLeft.GetComponent<LineRenderer>().material = material;

    legRight.AddComponent<LineRenderer>().positionCount = lineRenderer.positionCount;
    legRight.GetComponent<LineRenderer>().useWorldSpace = false;
    legRight.GetComponent<LineRenderer>().SetPositions(LRPositions);
    legRight.GetComponent<LineRenderer>().startWidth = .2f;
    legRight.GetComponent<LineRenderer>().endWidth = .2f;
    legRight.GetComponent<LineRenderer>().material = material;
  }


}
