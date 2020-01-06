using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
    public Material SunnySkyBox;
    public Material CloudySkyBox;


    private void Awake()
    {
        Meteo.onWeatherLoaded += new Meteo.OnWeatherLoaded(WeatherLoaded);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WeatherLoaded(List<Weather> weathers)
    {
        if(weathers.Count > 0)
        {
            Weather weather = weathers[0];
            Debug.Log(weather.main);
            switch (weather.main)
            {
                /*case "Rain":
                    RenderSettings.skybox = RainySkyBox;
                    break;*/
                case "Clouds":
                    RenderSettings.skybox = CloudySkyBox;
                    break;
                case "Clear":
                    RenderSettings.skybox = SunnySkyBox;
                    break;
                /*case "Wind":
                    RenderSettings.skybox = WindySkyBox;
                    break;
                case "Fog":
                    RenderSettings.skybox = FoggySkyBox;
                    break;
                case "Thunderstorm":
                    RenderSettings.skybox = ThunderSkyBox;
                    break;
                case "Drizzle":
                    RenderSettings.skybox = FoggySkyBox;
                    break;
                case "Snow":
                    RenderSettings.skybox = SnowySkyBox;
                    break;
                case "Atmosphere":
                    RenderSettings.skybox = ClearSkyBox;
                    break;*/
            }
        }
    }
}
