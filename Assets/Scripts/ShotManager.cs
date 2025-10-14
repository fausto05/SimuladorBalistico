using UnityEngine;
using Proyecto26;
using System.Collections.Generic;
using System;
using Newtonsoft.Json; 

public class ShotManager : MonoBehaviour
{
    private string baseUrl = "https://simuladoresunity-default-rtdb.firebaseio.com/";

    public void SaveShot(ShotResult shot, Action onSaved = null)
    {
        string key = System.DateTime.Now.Ticks.ToString();
        RestClient.Put(baseUrl + "shots/" + key + ".json", shot)
            .Then(response => {
                Debug.Log("Shot guardado exitosamente en Firebase.");
                onSaved?.Invoke();
            })
            .Catch(error => {
                Debug.LogError("Error guardando shot: " + error.Message);
            });
    }

    public void LoadShots(System.Action<Dictionary<string, ShotResult>> onLoaded)
    {
        RestClient.Get(baseUrl + "shots.json").Then(responseHelper => // Notar que no es Get<T>
        {
            if (responseHelper.StatusCode == 200)
            {
                Debug.Log("--- INICIO DEBUG LOADSHOTS MANUAL ---");
                Debug.Log("RAW JSON DE FIREBASE para /shots.json: " + responseHelper.Text);

                if (string.IsNullOrEmpty(responseHelper.Text) || responseHelper.Text == "null")
                {
                    Debug.Log("Firebase devolvió una respuesta de texto vacía o 'null'.");
                    onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    return;
                }

                Dictionary<string, ShotResult> results = null;
                try
                {
                    // ¡Aquí es donde intentamos la deserialización manual con Newtonsoft.Json!
                    results = JsonConvert.DeserializeObject<Dictionary<string, ShotResult>>(responseHelper.Text);

                    if (results != null)
                    {
                        Debug.Log($"DESERIALIZACIÓN MANUAL (Newtonsoft.Json) EXITOSA. Elementos: {results.Count}");
                        onLoaded?.Invoke(results);
                    }
                    else
                    {
                        Debug.LogWarning("DESERIALIZACIÓN MANUAL (Newtonsoft.Json) resultó en null. Esto es inusual para un diccionario no vacío.");
                        onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    }
                }
                catch (JsonSerializationException ex) // Captura errores específicos de Json.NET
                {
                    Debug.LogError("ERROR DE SERIALIZACIÓN JSON: " + ex.Message);
                    Debug.LogError("Ruta del error: " + ex.Path);
                    Debug.LogError("Línea: " + ex.LineNumber + ", Posición: " + ex.LinePosition);
                    Debug.LogError("JSON original que causó el error: " + responseHelper.Text);
                    onLoaded?.Invoke(null);
                }
                catch (Exception e) // Captura cualquier otro error
                {
                    Debug.LogError("ERROR INESPERADO DURANTE LA DESERIALIZACIÓN: " + e.Message);
                    Debug.LogError("JSON original: " + responseHelper.Text);
                    onLoaded?.Invoke(null);
                }
                Debug.Log("--- FIN DEBUG LOADSHOTS MANUAL ---");
            }
            else
            {
                Debug.LogError($"Error de red o servidor al cargar shots: HTTP {responseHelper.StatusCode} - {responseHelper.Error}");
                onLoaded?.Invoke(null);
            }
        }).Catch(error =>
        {
            Debug.LogError("Error de conexión general al cargar resultados: " + error.Message);
            onLoaded?.Invoke(null);
        });
    }
}


