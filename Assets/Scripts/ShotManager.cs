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
                onSaved?.Invoke();
            })
            .Catch(error => {
            });
    }

    public void LoadShots(System.Action<Dictionary<string, ShotResult>> onLoaded)
    {
        RestClient.Get(baseUrl + "shots.json").Then(responseHelper => 
        {
            if (responseHelper.StatusCode == 200)
            {
                if (string.IsNullOrEmpty(responseHelper.Text) || responseHelper.Text == "null")
                {
                    onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    return;
                }

                Dictionary<string, ShotResult> results = null;
                try
                {
                    results = JsonConvert.DeserializeObject<Dictionary<string, ShotResult>>(responseHelper.Text);

                    if (results != null)
                    {
                        onLoaded?.Invoke(results);
                    }
                    else
                    {
                        onLoaded?.Invoke(new Dictionary<string, ShotResult>());
                    }
                }
                catch (JsonSerializationException) 
                {
                    onLoaded?.Invoke(null);
                }
                catch (Exception) 
                {
                    onLoaded?.Invoke(null);
                }
            }
            else
            {
                onLoaded?.Invoke(null);
            }
        }).Catch(error =>
        {
            onLoaded?.Invoke(null);
        });
    }
}


