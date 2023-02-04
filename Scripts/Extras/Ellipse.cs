using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Ellipse : MonoBehaviour
{
    public Vector2 radius = new Vector2(1f, 1f);
    public float width = 1f;
    public float rotationAngle = 45;
    public int resolution = 500;

    private Vector3[] positions;
    private LineRenderer self_lineRenderer;


    void OnValidate()
    {
        UpdateEllipse();
    }

    public void UpdateEllipse()
    {
        if (self_lineRenderer == null)
            self_lineRenderer = GetComponent<LineRenderer>();

        self_lineRenderer.positionCount = resolution + 3;

        self_lineRenderer.startWidth = width;
        self_lineRenderer.endWidth = width; 


        AddPointToLineRenderer(0f, 0);
        for (int i = 1; i <= resolution + 1; i++)
        {
            AddPointToLineRenderer((float)i / (float)(resolution) * 2.0f * Mathf.PI, i);
        }
        AddPointToLineRenderer(0f, resolution + 2);
    }

    void AddPointToLineRenderer(float angle, int index)
    {
        Quaternion pointQuaternion = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        Vector3 pointPosition;

        pointPosition = new Vector3(radius.x * Mathf.Cos(angle), 
            0.0f, radius.y * Mathf.Sin(angle));
        pointPosition = pointQuaternion * pointPosition;

        self_lineRenderer.SetPosition(index, pointPosition);
    }
}