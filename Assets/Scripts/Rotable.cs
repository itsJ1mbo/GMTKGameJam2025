using System;
using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [SerializeField][Tooltip("Direccion de rotacion en sentido de las agujas del reloj")]
    private bool rotateClockwise = true;
    [SerializeField]
    private float rotationAngle = 90.0f;
    [SerializeField][Tooltip("Velocidad de rotacion")]
    private float rotationSpeed = 5.0f;
    [SerializeField][Tooltip("Eje de rotacion")]
    private Vector3 rotationAxis = Vector3.up;
    [SerializeField][Tooltip("Tamaño overlap")]
    Vector3 halfExtents = Vector3.zero;
    [SerializeField]
    private GameObject centerRoom;

    private float amountRotated = 0.0f;
    private bool isRotating = false;
    private int rotationDirection;
    private Collider[] facesToRotate;
    
    public bool rotate = false;
    
    void Update()
    {
        if(rotate)
            Rotate();
        
        if (isRotating)
        {
            foreach (Collider face in facesToRotate)
            {
                face.transform.RotateAround(centerRoom.transform.position, rotationAxis, rotationDirection * rotationSpeed * Time.deltaTime);
            }
            amountRotated += rotationSpeed * Time.deltaTime;
            if (amountRotated >= rotationAngle)
            {
                amountRotated = 0;
                isRotating = false;
            }
        }   
    }
    public void Rotate()
    {
        if (!isRotating)
        {
            CheckCurrentFaces();
            rotate = false;
            isRotating = true;
            rotationDirection = rotateClockwise ? 1 : -1;
        }
    }

    void CheckCurrentFaces()
    {
        facesToRotate = Physics.OverlapBox(transform.position, halfExtents, Quaternion.identity, 64, QueryTriggerInteraction.Collide);
    } 
}
