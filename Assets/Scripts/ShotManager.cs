using UnityEngine;
using Proyecto26;
using System.Collections.Generic;

public class ShotManager : MonoBehaviour
{
    private string baseUrl = "https://simuladoresunity-default-rtdb.firebaseio.com/";

    public void SaveShot(ShotResult shot)
    {
        string key = System.DateTime.Now.Ticks.ToString();
        RestClient.Put(baseUrl + "shots/" + key + ".json", shot);
    }

    public void LoadShots(System.Action<Dictionary<string, ShotResult>> onLoaded)
    {
        RestClient.Get<Dictionary<string, ShotResult>>(baseUrl + "shots.json").Then(response =>
        {
            onLoaded?.Invoke(response);
        }).Catch(error =>
        {
            Debug.LogError("Error cargando resultados: " + error.Message);
            onLoaded?.Invoke(null);
        });
    }
}
