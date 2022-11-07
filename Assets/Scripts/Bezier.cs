using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Vector3[] CalculateCurvePoints(int numPoints, Vector3 attacker, Vector3 victim, float height){
        Vector3[] positions = new Vector3[numPoints];

        Vector3 middleHeight = (attacker + victim)/2;
        middleHeight = new Vector3(middleHeight.x, middleHeight.y + height, middleHeight.z);

        float step = 1 / (float)numPoints, t = 0;
        for(int i = 0; i < numPoints; i++){
            positions[i] = CalculateQuadraticBezierPoint(t, attacker, middleHeight, victim);
            t += step;
        }

        return positions;
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}