using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICall : MonoBehaviour
{
    private bool isAPICallMade = false;
    public string MeteoConfigFilePath;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAPICallMade)
        {
            StartCoroutine(GetText());
            isAPICallMade = true;
        }
    }

    IEnumerator GetText()
    {
        while (true)
        {
            Meteo.Inst.UpdateValuesFromJsonString(System.IO.File.ReadAllText(MeteoConfigFilePath));
            Debug.Log(Meteo.Inst.CityNameAndCountry);
            Debug.Log(AppConfig.Inst.WebAPILink);
            using (UnityWebRequest www = UnityWebRequest.Get(String.Format(AppConfig.Inst.WebAPILink, Meteo.Inst.CityNameAndCountry)))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Meteo.Inst.UpdateValuesFromJsonString(www.downloadHandler.text);
                }
            }
            yield return new WaitForSeconds(300f);
        }
    }
}
    