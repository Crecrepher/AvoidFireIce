using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public Palate palate;
    public Vector2 minPosition = new Vector2(-10f, -10f); 
    public Vector2 maxPosition = new Vector2(10f, 10f);  
    public float dragSpeed = 2;                   

    private Vector3 lastMousePosition;

    void Update()
    {
        if (!palate.isSwipeMod)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;

                transform.Translate(-delta.x * dragSpeed * Time.deltaTime, -delta.y * dragSpeed * Time.deltaTime, 0);

                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, minPosition.x, maxPosition.x);
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, minPosition.y, maxPosition.y);
                transform.position = clampedPosition;

                lastMousePosition = Input.mousePosition;
            }
        }
       
    }
}
