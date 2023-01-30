using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testin : MonoBehaviour
{
	public static WebCamTexture camTex;
	
	static Texture2D img, rImg, bImg;// kImg, roImg, goImg, boImg, koImg, ORimg, ANDimg, SUMimg, AVGimg;
	
	static Color color;
	static float brillo;
	public bool cambio;
	public int numresiduo=4;

	public List<Vector2> posicion = new List<Vector2>();
	bool contains = false;
	public GameObject timer;
	public Text resultado;

	public GameObject[] basura = new GameObject[3];
	int residuo;
	void Start()
	{
		SetBasura();
		cambio = false;
		WebCamDevice[] devices = WebCamTexture.devices;
		WebCamDevice cam = devices[0];
		camTex = new WebCamTexture(cam.name, 400, 400);
		GetComponent<Renderer>().material.mainTexture =camTex;
		camTex.Play();
		
		img = new Texture2D(camTex.width, camTex.height);
		//initialize textures
		rImg = new Texture2D(img.width, img.height);
		bImg = new Texture2D(img.width, img.height);
	}

	void Update()
	{
		GetWebCamImage();
	}

	void GetWebCamImage()
	{
		img.SetPixels(camTex.GetPixels());
        if (cambio==false)
        {
			CalculateEdges();
		}
		
	}
	float verdeDiferencia;
	float amarilloDiferencia;
	float azulDiferencia;
	float rojoDiferencia;
	void SetBasura()
	{

		residuo = Random.Range(0, 3);// 0 papel, 1 vidrio, 2 plastico
		
		basura[residuo].SetActive(true);
	}
	void CalculateEdges()
	{
		int azul=0;
		int amarillo=0;
		int verde=0;
		int xverde=0;
		int xamarillo = 0;
		int xazul = 0;
		int yverde=0;
		int yamarillo = 0;
		int yazul = 0;
		int nocolor = 0;
		posicion.Clear();
		//calculate new textures
		for (int x = 0; x < img.width; x++)
		{
			for (int y = 0; y < img.height; y++)
			{
				detectarColor(x, y);
			}
		}

		
		var pixelData = bImg.GetPixels();
      
		Comparecolor();

		void detectarColor(int x, int y)
        {
			

			color = img.GetPixel(x, y);
			
			bImg.SetPixel(x, y, new Color(color.r, color.g, color.b));
			brillo = color.r + color.g + color.b;
			brillo /= 3f;


			Vector3 colorimgG = new Vector3(0, color.g, 0);
			Vector3 colorimgR = new Vector3(color.r, 0, 0);
			Vector3 colorimgY = new Vector3(color.r, color.g, 0);
			Vector3 colorimgB = new Vector3(0, 0, color.b);


			Vector3 magnitudVERDE = new Vector3(0, 1, 0);
			Vector3 magnitudAmarillo = new Vector3(1, 1, 0);
			Vector3 magnitudAzul = new Vector3(0, 0, 1);
			Vector3 magnitudRojo = new Vector3(1, 0, 0);
			verdeDiferencia = Mathf.Abs(magnitudVERDE.magnitude - colorimgG.magnitude);
			amarilloDiferencia = Mathf.Abs(magnitudAmarillo.magnitude - colorimgY.magnitude);
			azulDiferencia = Mathf.Abs(magnitudAzul.magnitude - colorimgB.magnitude);
			rojoDiferencia = Mathf.Abs(magnitudRojo.magnitude - colorimgR.magnitude);


			
			if (verdeDiferencia <= 0.5 && amarilloDiferencia > verdeDiferencia && verdeDiferencia < azulDiferencia && rojoDiferencia > verdeDiferencia)
			{
				
				verde++;
				xverde += x;
				yverde += y;
	
				rImg.SetPixel(x, y, new Color(0, color.g, 0));
			}
			else
			{
				if (amarilloDiferencia <= 0.5 && amarilloDiferencia < azulDiferencia)
				{
					amarillo++;
					xamarillo += x;
					yamarillo += y;

					rImg.SetPixel(x, y, new Color(color.r, color.g, 0));
				}
				else
				{
					if (azulDiferencia <= 0.5 && rojoDiferencia > azulDiferencia)
					{
						azul++;
						xazul += x;
						yazul += y;
						rImg.SetPixel(x, y, new Color(0, 0, color.b));
					}
					else
					{
						nocolor++;
						rImg.SetPixel(x, y, new Color(0, 0, 0));
					}
				}
			}
		}

		
		void Comparecolor()
        {
			if (verde > amarillo && verde > azul || verde > nocolor)
			{
				Color colorblue = new Color(0, 1, 0);
				Guardarposicion(verde, xverde, yverde, posicion,colorblue);
				contenedores(10, img.width/3, posicion, 1);
			}
			else
			{
				if (amarillo > azul || amarillo > nocolor)
				{
					Color colorblue = new Color(1, 1, 0);
					Guardarposicion(amarillo, xamarillo, yamarillo, posicion,colorblue);
					contenedores(img.width/3,  img.width / 2+50, posicion, 2);
				}
				else
				{
                    if (azul>verde)
                    {
						Color colorblue = new Color(0, 0, 1);
						Guardarposicion(azul, xazul, yazul, posicion, colorblue);
						contenedores(img.width /2+50, img.width -20, posicion, 0);
						//Debug.Log("azul");
						//numresiduo = 0;
					}
                    else
                    {
						numresiduo = 4;
						contains = false;
					}




				}
			}
		}
		void Guardarposicion(int pixelcolor, int xcolor, int ycolor, List<Vector2> poscolor,Color colorimg)
		{
			for (int x = 0; x < 25; x++)
			{
				for (int i = 0; i < 25; i++)
				{
					Vector2 newpos = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor + i);
					Vector2 newpos2 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor - i);
					Vector2 newpos3 = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor - i);
					Vector2 newpos4 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor + i);
					poscolor.Add(newpos);
					poscolor.Add(newpos2);
					poscolor.Add(newpos3);
					poscolor.Add(newpos4);
				
						bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor + i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor - i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor - i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor + i, colorimg);
					
				}

			}

		}
		
		void contenedores(int equis, int ancho,List<Vector2> poscolor, int nresiduo)
		{
			
			for (int x = equis; x < ancho; x++)
			{
				for (int i = img.height / 4; i < img.height / 2+img.height / 6; i++)
				{
					bImg.SetPixel(x, i, new Color(1, 1, 1));
					
					Vector2 posicioncubo = new Vector2(x, i);
				
					if (poscolor.Contains(posicioncubo) == true)
					{
						
						contains = true;
						
						break;
					}
                    else
                    {
						contains = false;
                    }

				}
			}
            if (contains==true)
            {
			
				numresiduo = nresiduo;
				if (cambio == false)
				{
					if (residuo == numresiduo)
					{
						StartCoroutine(nameof(Wait));

					}
					else
					{
						if (numresiduo < 3)
						{
							resultado.text = "Incorrecto";
							StartCoroutine(nameof(Incorrecto));

						}
					}
				}
			}
		}
		rImg.Apply();
		//gImg.Apply();
		bImg.Apply();
		//kImg.Apply();
		
		
	}
	IEnumerator Wait()
	{
		resultado.text = "Correcto";
		timer.SetActive(true);
	
		cambio = true;

		basura[residuo].SetActive(false);


		
		yield return new WaitForSeconds(3f);
		resultado.text = "";
		SetBasura();
		cambio = false;

		timer.GetComponent<Timer>().contador.text = "";
		timer.SetActive(false);

	}
	IEnumerator Incorrecto()
	{

		cambio = true;
		resultado.text = "Incorrecto";
		yield return new WaitForSeconds(0.3f);
		
		yield return new WaitForSeconds(3f);
		resultado.text = "";

		cambio = false;


	}
	// Update is called once per frame
	void OnGUI()
	{
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        
        GUILayout.Label("color");

        GUILayout.Label(rImg);
        GUILayout.Label("blue");
        GUILayout.Label(bImg);

        GUILayout.EndVertical();

       

        GUILayout.EndHorizontal();
    }

 
}
