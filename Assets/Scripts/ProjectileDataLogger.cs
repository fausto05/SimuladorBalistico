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

        // Distancia horizontal del disparo
        float distance = Vector3.Distance(startPos, impactPoint);

        int pieces = CountFallenPieces();

        // Acierto si pegó a un objeto con tag "Target"
        bool hit = collision.gameObject.CompareTag("Target");

        // Obtenemos referencia al ShotManager
        ShotManager manager = FindFirstObjectByType<ShotManager>();

        ShotResult result = new ShotResult(
            GameObject.FindFirstObjectByType<ProjectileLauncher>().angleSlider.value,
            GameObject.FindFirstObjectByType<ProjectileLauncher>().forceSlider.value,
            GameObject.FindFirstObjectByType<ProjectileLauncher>().massDropdown.value + 1,
            hit,
            distance,
            pieces
        );

        manager.SaveShot(result);

        Debug.Log($"Guardado en Firebase: {result.angle}°, {result.force} fuerza, {result.mass} masa, Hit: {result.hitTarget}, Dist: {result.distance}, Piezas: {result.pieces}");
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
