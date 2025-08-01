using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [SerializeField][Tooltip("Direccion de rotacion en sentido de las agujas del reloj")]
    private bool rotateClockwise = true;
    [SerializeField][Tooltip("Velocidad de rotacion")]
    private float rotationSpeed = 5.0f;
    [SerializeField][Tooltip("Eje de rotacion")]
    private Vector3 rotationAxis = Vector3.up;
    
    
    //angulos que rota la cara
    private float rotationAngle = 90.0f;
    private float amountRotated = 0.0f;
    private bool isRotating = false;
    private int rotationDirection;

    void Start()
    {
        Rotate();
    }
    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(rotationAxis, rotationDirection * rotationSpeed * Time.deltaTime);
            amountRotated += rotationSpeed * Time.deltaTime;
            if (amountRotated >= rotationAngle)
            {
                amountRotated = 0;
                isRotating = false;
            }
        }   
    }
    public void changeRotationDirection()
    {
        if(!isRotating)
            rotateClockwise = !rotateClockwise;
    }
    public void Rotate()
    {
        if (!isRotating)
        {
            isRotating = true;
            rotationDirection = rotateClockwise ? 1 : -1;
        }
    }
}
