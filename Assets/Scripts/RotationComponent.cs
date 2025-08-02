using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [Tooltip("El eje del slice a rotar (X, Y o Z)")]
    public Axis sliceAxis = Axis.Y;

    [Tooltip("–1 = capa negativa, 0 = interna media, +1 = positiva")]
    [Range(-1, 1)]
    public int sliceIndex = 1;

    [Tooltip("Horario o antihorario respecto al eje emergente")]
    public bool rotateClockwise = true;

    [Tooltip("Referencia a CubeManager (puede autodetectarse)")]
    public CubeManager cubeManager;

    /// <summary>
    /// Llama al método de rotación 90° para este slice
    /// </summary>
    public void Rotate()
    {
        if (cubeManager != null)
            cubeManager.RotateSlice(sliceAxis, sliceIndex, rotateClockwise);
    }
}