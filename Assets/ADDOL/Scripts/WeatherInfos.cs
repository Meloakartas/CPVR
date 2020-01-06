using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherInfos : MonoBehaviour
{
    public Text Main;
    public Text Temperature;
    public Text FeelsLike;
    public Text TempMin;
    public Text TempMax;
    public Text Pressure;
    public Text Humidity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        this.gameObject.SetActive(false);
        Meteo.onWeatherLoaded += new Meteo.OnWeatherLoaded(UpdateTextsInfosDatas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTextsInfosDatas(List<Weather> weathers)
    {
        if (weathers.Count > 0)
        {
            Main meteoDetails = Meteo.Inst.main;
            Main.text = weathers[0].main;
            Temperature.text = meteoDetails.temp.ToString();
            FeelsLike.text = meteoDetails.feels_like.ToString();
            TempMin.text = meteoDetails.temp_min.ToString();
            TempMax.text = meteoDetails.temp_max.ToString();
            Pressure.text = meteoDetails.pressure.ToString();
            Humidity.text = meteoDetails.humidity.ToString();
        }
    }
}
