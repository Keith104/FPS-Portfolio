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
        outFOV = (int)playerCam.fieldOfView;
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

        // Change later for when it becomes a setting option
        inFOV = outFOV - 20;

        float target = isZooming ? inFOV : outFOV;

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, target, lerpSpeed * Time.deltaTime);
    }
}
