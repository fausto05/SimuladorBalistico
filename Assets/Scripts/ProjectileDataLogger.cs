using UnityEngine;

public class ProjectileDataLogger : MonoBehaviour
{
    private float startTime;
    private Vector3 startPos;

    private bool hasCollided = false;

    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        float flightTime = Time.time - startTime;
        Vector3 impactPoint = collision.contacts[0].point;
        Vector3 relativeVelocity = collision.relativeVelocity;
        float impulse = collision.impulse.magnitude;

        int pieces = CountFallenPieces();

        Debug.Log
        (
            $"Tiempo de vuelo: {flightTime:F2}s\n" +
            $"Punto de impacto: {impactPoint}\n" +
            $"Velocidad relativa: {relativeVelocity.magnitude:F2}\n" +
            $"Impulso: {impulse:F2}\n" +
            $"Piezas derribadas: {pieces}"
        );
    }

    int CountFallenPieces()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        int count = 0;
        foreach (GameObject t in targets)
        {
            if (t.transform.position.y < 0.5f) 
                count++;
        }
        return count;
    }
}
