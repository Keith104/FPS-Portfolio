using UnityEngine;

public class ZoomIn : MonoBehaviour
{
     [SerializeField] Camera playerCam; // the player's camera child

     [SerializeField] int inFOV;  // the fully zoomed in zoom level
     [SerializeField] int outFOV; // regular zoom level
    [SerializeField] int lerpSpeed;

    bool isZooming;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        if (Input.GetMouseButtonDown(1)) isZooming = true;
        else if (Input.GetMouseButtonUp(1)) isZooming = false;

        float target = isZooming ? inFOV : outFOV;

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, target, lerpSpeed * Time.deltaTime);
    }
}
