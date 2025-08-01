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
    /*[SerializeField][Tooltip("Eje de rotacion")]
    private Vector3 rotationAxis = Vector3.up;*/
    [SerializeField][Tooltip("Tamaño overlap")]
    Vector3 halfExtents = Vector3.zero;
    [SerializeField]
    private GameObject centerRoom;
    
    private CubeManager cubeManager;
    private float amountRotated = 0.0f;
    private int rotationDirection;
    private Collider[] facesToRotate;
    private bool isFaceRotating = false;
    private Vector3 finalRotation;
    
    //booleano para testear rapido
    //public bool rotate = false;
    
    private void Start()
    {
        cubeManager = transform.parent.GetComponent<CubeManager>();
    }

    void Update()
    {
        /*if (rotate)
            Rotate();*/
        
        if (isFaceRotating) //tiene que ser especificamente esta cara la que este rotando para aplicar esto
        {
            foreach (Collider face in facesToRotate)
            {
                face.transform.RotateAround(centerRoom.transform.position, (transform.position - centerRoom.transform.position).normalized, rotationDirection * rotationSpeed * Time.deltaTime);
            }
            amountRotated += rotationSpeed * Time.deltaTime;
            if (amountRotated >= rotationAngle)
            {
                isFaceRotating = false;
                cubeManager.setCubeRotation(false);
                //el snapeo a un angulo mas feo que jamas vera nadie
                foreach (Collider face in facesToRotate)
                {
                    face.transform.RotateAround(centerRoom.transform.position, (transform.position - centerRoom.transform.position).normalized, -rotationDirection * (amountRotated-rotationAngle));
                }
                amountRotated = 0;
            }
        }   
    }
    public void Rotate()
    {
        if (!cubeManager.isCubeRotating())
        {
            //rotate = false;
            isFaceRotating = true;
            CheckCurrentFaces();
            cubeManager.setCubeRotation(true);
            rotationDirection = rotateClockwise ? 1 : -1;
            
            //calculo de la rotacion final
            finalRotation = transform.rotation.eulerAngles + (transform.position - centerRoom.transform.position).normalized * rotationAngle * rotationDirection;
        }
    }

    void CheckCurrentFaces()
    {
        facesToRotate = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, 64, QueryTriggerInteraction.Collide);
    } 
}
