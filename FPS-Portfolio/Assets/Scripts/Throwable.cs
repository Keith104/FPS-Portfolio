using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] Rigidbody throwRB;

    [SerializeField] bool isSpining;
    [SerializeField] bool isSticky;

    [SerializeField] int forceMult;
    [SerializeField] int bouncyness;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawCurvedRay();
    }

    void Throw()
    {
        if (Input.GetMouseButton(0))
        {
            DrawCurvedRay();

            // adds instantaneous force in the forward direction of camera
            throwRB.AddForce(playerCam.transform.forward * forceMult);
        }
    }

    void updateThrowableUI()
    {

    }

    void DrawCurvedRay()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red);
    }
}
