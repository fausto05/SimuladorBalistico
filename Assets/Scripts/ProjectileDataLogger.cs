using UnityEngine;

public class ProjectileDataLogger : MonoBehaviour
{
    private float startTime;
    private Vector3 startPos;

    private bool hasCollided = false;

    // Agregamos referencias cacheadas para ShotManager, ShotUI y ProjectileLauncher
    private ShotManager _shotManager;
    private ShotUI _shotUI;
    private ProjectileLauncher _projectileLauncher;

    void Awake() // Usamos Awake para obtener las referencias antes que Start
    {
        _shotManager = FindFirstObjectByType<ShotManager>();
        _shotUI = FindFirstObjectByType<ShotUI>();
        _projectileLauncher = FindFirstObjectByType<ProjectileLauncher>();

        if (_shotManager == null) Debug.LogError("ShotManager no encontrado en la escena.");
        if (_shotUI == null) Debug.LogError("ShotUI no encontrado en la escena.");
        if (_projectileLauncher == null) Debug.LogError("ProjectileLauncher no encontrado en la escena.");
    }

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
        bool hitTarget = collision.gameObject.CompareTag("Target");

        // Usamos la referencia cacheada a ProjectileLauncher
        if (_projectileLauncher == null)
        {
            Debug.LogError("ProjectileLauncher no está disponible para obtener los valores del disparo. No se guardará el resultado.");
            return;
        }

        ShotResult result = new ShotResult(
            _projectileLauncher.angleSlider.value,
            _projectileLauncher.forceSlider.value,
            _projectileLauncher.massDropdown.value + 1, // Accede al valor del Dropdown correctamente
            hitTarget,
            distance,
            pieces
        );

        // Usamos la referencia cacheada a ShotManager
        if (_shotManager != null)
        {
            // Llama a SaveShot y proporciona un callback (lambda) que se ejecutará al finalizar el guardado
            _shotManager.SaveShot(result, () => {
                Debug.Log("Guardado en Firebase completo. Actualizando UI sin duplicar.");
                if (_shotUI != null)
                {
                    _shotUI.ClearResultsUI();  // limpia los anteriores
                                               // Espera un pequeño delay antes de recargar para que Firebase actualice su base
                    _shotUI.Invoke(nameof(_shotUI.ShowResults), 0.3f);
                }
            });
        }
        else
        {
            Debug.LogError("No se pudo guardar el shot porque ShotManager no está disponible.");
        }

        Debug.Log($"Guardado en Firebase (inicialmente): {result.angle:F1}°, {result.force:F1} fuerza, {result.mass:F1} masa, Hit: {result.hitTarget}, Dist: {result.distance:F2}, Piezas: {result.pieces}");
        // Opcional: Destruir el proyectil después de la colisión para un nuevo ciclo
        // Destroy(gameObject, 0.1f); // Esto destruiría el proyectil después de un corto retraso
    }

    int CountFallenPieces()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        int count = 0;
        foreach (GameObject t in targets)
        {
            // Ajusta este umbral si tus "piezas caídas" tienen una posición Y diferente
            if (t.transform.position.y < 0.5f)
                count++;
        }
        return count;
    }
}

