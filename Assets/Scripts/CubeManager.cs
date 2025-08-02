using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eje lógico para las capas: X (izq‑der), Y (arriba‑abajo), Z (front‑back).
/// </summary>
public enum Axis { X, Y, Z }

public class CubeManager : MonoBehaviour
{
    [SerializeField] private bool _restrictRotations = true;
    private bool cubeRotating = false;

    // Lista interna para cada cubie con referencia a Transform + coordenadas lógicas
    private readonly List<Cubie> cubies = new List<Cubie>();

    // Ajusta si deseas que poste turno sea más lento o rápido (deg/s)
    [SerializeField] private float turnSpeed = 270f;

    private void Awake()
    {
        // Al principio se asume que todos los cubies están como hijos directos.
        foreach (Transform t in transform)
        {
            Vector3Int coord = RoundCoord(t.localPosition);
            cubies.Add(new Cubie { tr = t, coord = coord });
        }
    }

    public bool isCubeRotating() => cubeRotating && _restrictRotations;
    public void setCubeRotation(bool onoff) { cubeRotating = onoff; }

    /// <summary>
    /// Invoca una rotación de 90° sobre cualquier capa.
    /// layerIndex: -1, 0, +1 según corresponda en el axis.
    /// axis: el eje del slice (X/Y/Z).
    /// clockwise: true = +90° (CW al observador).
    /// </summary>
    public void RotateSlice(Axis axis, int layerIndex, bool clockwise)
    {
        if (cubeRotating && _restrictRotations) return;
        StartCoroutine(RotateSliceRoutine(axis, layerIndex, clockwise));
    }

    private IEnumerator RotateSliceRoutine(Axis axis, int layerIndex, bool clockwise)
    {
        cubeRotating = true;
        Debug.Log("Rotating slice " + name);
        List<Transform> slice = FilterSlice(axis, layerIndex);
        GameObject pivot = new GameObject($"SlicePivot_{axis}{layerIndex}");
        pivot.transform.SetParent(transform, worldPositionStays: true);
        pivot.transform.localPosition = Vector3.zero;
        pivot.transform.localRotation = Quaternion.identity;

        foreach (var t in slice) t.SetParent(pivot.transform, worldPositionStays: true);

        Vector3 rotAxis = axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.up
        };

        float targetAngle = clockwise ? 90f : -90f;
        float rotated = 0f;

        while (Mathf.Abs(rotated) < 0.9999f * Mathf.Abs(targetAngle))
        {
            float step = Mathf.Sign(targetAngle) * turnSpeed * Time.deltaTime;
            if (Mathf.Abs(rotated + step) > Mathf.Abs(targetAngle))
                step = targetAngle - rotated;

            pivot.transform.Rotate(rotAxis, step, Space.Self);
            rotated += step;
            yield return null;
        }

        // Snap final exacto
        pivot.transform.localRotation =
            Quaternion.AngleAxis(targetAngle, rotAxis) * pivot.transform.localRotation;

        foreach (var t in slice) t.SetParent(transform, worldPositionStays: true);
        Destroy(pivot);

        // Recalcular coordenadas lógicas
        foreach (var c in cubies) c.coord = RoundCoord(c.tr.localPosition);

        cubeRotating = false;
    }

    private Vector3Int RoundCoord(Vector3 localPos)
    {
        // Redondeo cercano a -1, 0 o +1 (inclusivo para futuros cubos más grandes)
        return new Vector3Int(
            Mathf.RoundToInt(localPos.x),
            Mathf.RoundToInt(localPos.y),
            Mathf.RoundToInt(localPos.z)
        );
    }

    private List<Transform> FilterSlice(Axis axis, int layerIndex)
    {
        var list = new List<Transform>(9);
        foreach (var c in cubies)
        {
            int val = axis switch
            {
                Axis.X => c.coord.x,
                Axis.Y => c.coord.y,
                Axis.Z => c.coord.z,
                _ => 0
            };
            if (val == layerIndex) list.Add(c.tr);
        }
        return list;
    }

    private class Cubie { public Transform tr; public Vector3Int coord; }
}
