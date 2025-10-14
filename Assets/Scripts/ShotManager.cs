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
                    Debug.Log("Firebase devolvi� una respuesta de texto vac�a o 'null'.");
                    onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    return;
                }

                Dictionary<string, ShotResult> results = null;
                try
                {
                    // �Aqu� es donde intentamos la deserializaci�n manual con Newtonsoft.Json!
                    results = JsonConvert.DeserializeObject<Dictionary<string, ShotResult>>(responseHelper.Text);

                    if (results != null)
                    {
                        Debug.Log($"DESERIALIZACI�N MANUAL (Newtonsoft.Json) EXITOSA. Elementos: {results.Count}");
                        onLoaded?.Invoke(results);
                    }
                    else
                    {
                        Debug.LogWarning("DESERIALIZACI�N MANUAL (Newtonsoft.Json) result� en null. Esto es inusual para un diccionario no vac�o.");
                        onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    }
                }
                catch (JsonSerializationException ex) // Captura errores espec�ficos de Json.NET
                {
                    Debug.LogError("ERROR DE SERIALIZACI�N JSON: " + ex.Message);
                    Debug.LogError("Ruta del error: " + ex.Path);
                    Debug.LogError("L�nea: " + ex.LineNumber + ", Posici�n: " + ex.LinePosition);
                    Debug.LogError("JSON original que caus� el error: " + responseHelper.Text);
                    onLoaded?.Invoke(null);
                }
                catch (Exception e) // Captura cualquier otro error
                {
                    Debug.LogError("ERROR INESPERADO DURANTE LA DESERIALIZACI�N: " + e.Message);
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
            Debug.LogError("Error de conexi�n general al cargar resultados: " + error.Message);
            onLoaded?.Invoke(null);
        });
    }
}


