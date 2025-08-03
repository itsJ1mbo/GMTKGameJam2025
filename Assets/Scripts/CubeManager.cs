using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eje lógico para las capas: X (izq-der), Y (arriba-abajo), Z (front-back).
/// </summary>
public enum Axis { X, Y, Z }

public class CubeManager : MonoBehaviour
{
    [Header("Configuración de lockeo")]
    [SerializeField] private bool _restrictRotations = true;
    private bool cubeRotating = false;

    [Header("Velocidad de giro (grados/seg)")]
    [SerializeField] private float turnSpeed = 270f;
    
    [Header("Tag que identifica a cada cubie")]
    [SerializeField] private string cubieTag = "Cubie";

    // Lista interna para cada cubie con referencia a Transform + coordenadas lógicas
    private readonly List<Cubie> cubies = new List<Cubie>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag(cubieTag))
            {
                Vector3Int coord = RoundCoord(child.localPosition);
                cubies.Add(new Cubie { tr = child, coord = coord });
            }
        }
    }

    /// <summary>
    /// Gira 90° el slice definido por (axis, layerIndex).
    /// layerIndex ∈ {-1, 0, +1}. clockwise=true → +90°.
    /// </summary>
    public void RotateSlice(Axis axis, int layerIndex, bool clockwise)
    {
        if (cubeRotating && _restrictRotations) return;
        if(GetComponent<AudioSource>())
            GetComponent<AudioSource>().Play();
        StartCoroutine(RotateSliceRoutine(axis, layerIndex, clockwise));
    }

    private IEnumerator RotateSliceRoutine(Axis axis, int layerIndex, bool clockwise)
    {
        cubeRotating = true;

        // 1) Filtramos cubies que pertenecen al slice por su coord
        var slice = new List<Transform>();
        foreach (var c in cubies)
        {
            int v = axis == Axis.X ? c.coord.x
                  : axis == Axis.Y ? c.coord.y
                  : /*Z*/            c.coord.z;
            if (v == layerIndex) 
                slice.Add(c.tr);
        }

        if (slice.Count == 0)
        {
            cubeRotating = false;
            yield break;
        }

        // 2) Creamos un pivot vacío en el centro del cubo
        var pivot = new GameObject($"Pivot_{axis}{layerIndex}");
        pivot.transform.SetParent(transform, false);        // sin mover worldPos
        pivot.transform.localPosition = Vector3.zero;
        pivot.transform.localRotation = Quaternion.identity;

        // 3) Parentamos los cubies al pivot (manteniendo su posición mundial)
        foreach (var t in slice)
            t.SetParent(pivot.transform, true);

        // 4) Preparamos Slerp
        Vector3 rotAxis = axis == Axis.X ? Vector3.right
                         : axis == Axis.Y ? Vector3.up
                         : Vector3.forward;
        float targetAngle = clockwise ? 90f : -90f;
        Quaternion startRot = pivot.transform.rotation;
        Quaternion endRot   = startRot * Quaternion.AngleAxis(targetAngle, rotAxis);
        float duration = Mathf.Abs(targetAngle) / turnSpeed;
        float elapsed = 0f;

        // 5) Ejecutamos Lerp en el tiempo dado
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            pivot.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        // 6) Aseguramos snap exacto
        pivot.transform.rotation = endRot;

        // 7) Re-parentamos los cubies de vuelta al root
        foreach (var t in slice)
            t.SetParent(transform, true);

        Destroy(pivot);

        // 8) Snap final de posiciones locales y recalcular coords
        foreach (var c in cubies)
        {
            // Redondear a -1,0,1
            Vector3 lp = c.tr.localPosition;
            lp.x = Mathf.Round(lp.x);
            lp.y = Mathf.Round(lp.y);
            lp.z = Mathf.Round(lp.z);
            c.tr.localPosition = lp;
            c.coord = RoundCoord(lp);
            if(GetComponent<AudioSource>())
                GetComponent<AudioSource>().Stop();
        }

        cubeRotating = false;
    }

    // Convierte cualquier Vector3 cercano a enteros en Vector3Int
    private Vector3Int RoundCoord(Vector3 v)
    {
        return new Vector3Int(
            Mathf.RoundToInt(v.x),
            Mathf.RoundToInt(v.y),
            Mathf.RoundToInt(v.z)
        );
    }

    // Clase interna ligera para trackear Transform + su coord lógica
    private class Cubie
    {
        public Transform    tr;
        public Vector3Int   coord;
    }
}
