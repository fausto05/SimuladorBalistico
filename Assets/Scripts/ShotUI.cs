using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ShotUI : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject resultPrefab;
    private ShotManager manager;

    private bool isLoading = false; // evita que se carguen dos veces seguidas

    void Start()
    {
        manager = FindFirstObjectByType<ShotManager>();
        if (manager == null)
        {
            Debug.LogError("ShotManager no encontrado en la escena.");
            return;
        }

        ShowResults();
    }

    public void ShowResults()
    {
        if (isLoading)
        {
            Debug.Log("Ya se está cargando la lista, espera un momento...");
            return;
        }

        isLoading = true;
        Debug.Log("Cargando resultados en UI...");

        // Limpia el contenido anterior
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        if (manager == null)
        {
            Debug.LogError("ShotManager no está inicializado en ShotUI.");
            isLoading = false;
            return;
        }

        manager.LoadShots(results =>
        {
            isLoading = false;

            if (results == null || results.Count == 0)
            {
                Debug.Log("No hay resultados en Firebase todavía para mostrar.");
                return;
            }

            Debug.Log($"Firebase devolvió {results.Count} resultados para mostrar en UI.");

            // Ordenar los resultados por clave (Ticks) de más reciente a más antiguo
            foreach (var kv in results.OrderByDescending(k => k.Key))
            {
                ShotResult r = kv.Value;
                GameObject entry = Instantiate(resultPrefab, contentPanel);
                TMP_Text textComponent = entry.GetComponentInChildren<TMP_Text>();

                if (textComponent != null)
                {
                    textComponent.text =
                        $"Ángulo: {r.angle:F1} | Fuerza: {r.force:F1} | Masa: {r.mass:F1} | Hit: {r.hitTarget} | Dist: {r.distance:F2} | Piezas: {r.pieces}";
                }
            }
        });
    }

    public void ClearResultsUI()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);
        Debug.Log("Resultados anteriores eliminados del ScrollView.");
    }
}

