using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text contador;
    float tiempo=3;
    // Start is called before the first frame update
    void Start()
    {
        contador.text = " " + tiempo;
    }

    // Update is called once per frame
    void Update()
    {
        tiempo -= Time.deltaTime;
        contador.text = " " + tiempo.ToString("f0");
        if (tiempo<=0)
        {
            contador.text = "";
        }
    }
}
