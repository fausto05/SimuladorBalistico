using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ShotUI : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject resultPrefab;
    private ShotManager manager;

    private bool isLoading = false; 

    void Start()
    {
        manager = FindFirstObjectByType<ShotManager>();
        if (manager == null)
        {
            return;
        }

        ShowResults();
    }

    public void ShowResults()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;
        
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        if (manager == null)
        {
            isLoading = false;
            return;
        }

        manager.LoadShots(results =>
        {
            isLoading = false;

            if (results == null || results.Count == 0)
            {
                return;
            }

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
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }

    public void ClearResultsUI()
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);
    }
}

