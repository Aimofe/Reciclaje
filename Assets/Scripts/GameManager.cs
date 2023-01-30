using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class GameManager : MonoBehaviour
//{
//    public GameObject[] basura= new GameObject[3];
//    int residuo;
//    public GameObject testin;
//    bool cambio = false;
//    public GameObject timer;
//    public Text resultado;

//    void Start()
//    {
//        SetBasura();
//        timer.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (cambio==false)
//        {
//            if (residuo == testin.GetComponent<testin>().numresiduo)
//            {
                

//                StartCoroutine(nameof(Wait));
              
//            }
//            else
//            {
//                if (testin.GetComponent<testin>().numresiduo < 3)
//                {
//                    resultado.text = "Incorrecto";
//                    StartCoroutine(nameof(Incorrecto));
                   
//                }
//            }
//        }
//    }
//    void SetBasura()
//    {
        
//        residuo = Random.Range(0, 3);// 0 papel, 1 vidrio, 2 plastico
//        Debug.Log("residuo"+residuo);
//        basura[residuo].SetActive(true);
//    }
//    IEnumerator Wait()
//    {
//        resultado.text = "Correcto";
//        timer.SetActive(true);
//        //Debug.Log("Cambio");
//        cambio = true;
//        testin.GetComponent<testin>().cambio = cambio;
//        basura[residuo].SetActive(false);
       
//        //yield return new WaitForSeconds(0.3f);
       
//        resultado.text = "";
//        yield return new WaitForSeconds(3f);
//        SetBasura();
//        cambio = false;
//        testin.GetComponent<testin>().cambio = cambio;
//        timer.GetComponent<Timer>().contador.text = "";
//        timer.SetActive(false);

//    }
//    IEnumerator Incorrecto()
//    {
       
//        cambio = true;
//        testin.GetComponent<testin>().cambio = cambio;
//        yield return new WaitForSeconds(0.3f);
//        resultado.text = "";
//        yield return new WaitForSeconds(3f);
  
//        cambio = false;
//        testin.GetComponent<testin>().cambio = cambio;

//    }
//}
