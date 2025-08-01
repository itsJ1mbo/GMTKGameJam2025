using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private bool cubeRotating = false;
    
    public bool isCubeRotating() { return cubeRotating; }
    public void setCubeRotation(bool onoff) { cubeRotating = onoff; }
}
