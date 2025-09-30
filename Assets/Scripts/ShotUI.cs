using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShotUI : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject resultPrefab;

    private ShotManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<ShotManager>();
    }

    public void ShowResults()
    {
        Debug.Log("Cargando resultados...");

        if (contentPanel == null || resultPrefab == null)
        {
            Debug.LogError("Faltan referencias en ShotUI.");
            return;
        }

        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        manager.LoadShots(results =>
        {
            if (results == null)
            {
                Debug.Log("No hay resultados en Firebase todavía.");
                return;
            }

            foreach (var kv in results)
            {
                ShotResult r = kv.Value;
                GameObject entry = Instantiate(resultPrefab, contentPanel);
                entry.GetComponent<TMP_Text>().text =
                    $"Ángulo: {r.angle:F1} | Fuerza: {r.force:F1} | Masa: {r.mass} | Hit: {r.hitTarget} | Dist: {r.distance:F2} | Piezas: {r.pieces}";
            }
        });
    }
}
