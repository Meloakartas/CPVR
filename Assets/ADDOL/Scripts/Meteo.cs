using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Meteo
{
    public delegate void OnWeatherLoaded(List<Weather> weather);
    public static event OnWeatherLoaded onWeatherLoaded;

    public List<Weather> weather;
    public int CityID;
    public Main main;

    private static Meteo inst;
    
    public static Meteo Inst
    {
        get
        {
            if (inst == null) inst = new Meteo();
            return inst;
        }
    }

    public void UpdateValuesFromJsonString(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, Inst);
        if (Inst.weather.Count > 0)
        {
            Inst.weather[0].temperature = Inst.main.temp;
            Debug.Log(Inst.weather[0].temperature);
        }
            
        onWeatherLoaded(Inst.weather);
    }

    public void UpdateValuesFromJsonFile(string filePath)
    {
        Inst.UpdateValuesFromJsonString(System.IO.File.ReadAllText(filePath));
    }
}

[System.Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
    public string icon;
    public float temperature;
}

[System.Serializable]
public class Main
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public int pressure;
    public int humidity;
    public int sea_level;
    public int grnd_level;
}
