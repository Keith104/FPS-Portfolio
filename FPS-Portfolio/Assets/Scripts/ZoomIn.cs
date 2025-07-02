using UnityEngine;

public class ZoomIn : MonoBehaviour
{
     [SerializeField] Camera playerCam; // the player's camera child

     [SerializeField] int inFOV;  // the fully zoomed in zoom level
     [SerializeField] int outFOV; // regular zoom level

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          Zoom();
    }
     void Zoom() // this does a Perspective Zoom
     {
          // GetMouseButtonDown(1) checks if the right mouse button is pressed
          if (Input.GetMouseButtonDown(1))
          {
               playerCam.fieldOfView = inFOV;
          }
          else if (Input.GetMouseButtonUp(1))
          {
               playerCam.fieldOfView = outFOV;
          }
     }
}
