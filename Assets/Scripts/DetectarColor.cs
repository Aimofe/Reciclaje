using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectarColor : MonoBehaviour
{
	public static WebCamTexture camTex;
	static Texture2D img, colorImg, posImg;
	static Color color;
			
	public int numresiduo = 4;
	int residuo;

	float verdeDiferencia;
	float amarilloDiferencia;
	float azulDiferencia;
	float rojoDiferencia;

	//Posiciones de los pixeles de los colores
	public List<Vector2> posicion = new List<Vector2>();
		
	bool contains = false;
	
	//Establece si se esta cambiando de residuo
	public bool cambio;

	public GameObject[] basura = new GameObject[3];
	public GameObject timer;

	public Text resultado;
		
	void Start()
	{
		SetBasura();
		cambio = false;
		WebCamDevice[] devices = WebCamTexture.devices;
		WebCamDevice cam = devices[0];
		camTex = new WebCamTexture(cam.name, 400, 400);
		GetComponent<Renderer>().material.mainTexture = camTex;
		camTex.Play();

		img = new Texture2D(camTex.width, camTex.height);
		//initialize textures
		colorImg = new Texture2D(img.width, img.height);
		posImg = new Texture2D(img.width, img.height);
	}

	void Update()
	{
		GetWebCamImage();
	}

	void GetWebCamImage()
	{
		img.SetPixels(camTex.GetPixels());
		if (cambio == false)
		{
			Imagen();
		}
	}
	//Asigna un residuo aleatoriamente 
	void SetBasura()
	{
		residuo = Random.Range(0, 3);// 0 papel, 1 vidrio, 2 plastico
			
		basura[residuo].SetActive(true);
	}
	void Imagen()
	{
		int azul = 0;
		int amarillo = 0;
		int verde = 0;
		int xverde = 0;
		int xamarillo = 0;
		int xazul = 0;
		int yverde = 0;
		int yamarillo = 0;
		int yazul = 0;
		int nocolor = 0;

		posicion.Clear();

		for (int x = 0; x < img.width; x++)
		{
			for (int y = 0; y < img.height; y++)
			{
				detectarColor(x, y);
			}
		}
		Comparecolor();

		//Determina de que color son los pixels de la imagen
		void detectarColor(int x, int y)
		{
			color = img.GetPixel(x, y);

			posImg.SetPixel(x, y, new Color(color.r, color.g, color.b));

			Vector3 colorimgG = new Vector3(0, color.g, 0);
			Vector3 colorimgR = new Vector3(color.r, 0, 0);
			Vector3 colorimgY = new Vector3(color.r, color.g, 0);
			Vector3 colorimgB = new Vector3(0, 0, color.b);


			Vector3 vecVerde = new Vector3(0, 1, 0);
			Vector3 vecAmarillo = new Vector3(1, 1, 0);
			Vector3 vecAzul = new Vector3(0, 0, 1);
			Vector3 vecRojo = new Vector3(1, 0, 0);

			verdeDiferencia = Mathf.Abs(vecVerde.magnitude - colorimgG.magnitude);
			amarilloDiferencia = Mathf.Abs(vecAmarillo.magnitude - colorimgY.magnitude);
			azulDiferencia = Mathf.Abs(vecAzul.magnitude - colorimgB.magnitude);
			rojoDiferencia = Mathf.Abs(vecRojo.magnitude - colorimgR.magnitude);

			if (verdeDiferencia <= 0.5 && amarilloDiferencia > verdeDiferencia && verdeDiferencia < azulDiferencia && rojoDiferencia > verdeDiferencia)
			{
				verde++;
				xverde += x;
				yverde += y;

				colorImg.SetPixel(x, y, new Color(0, color.g, 0));
			}
			else
			{
				if (amarilloDiferencia <= 0.5 && amarilloDiferencia < azulDiferencia)
				{
					amarillo++;
					xamarillo += x;
					yamarillo += y;

					colorImg.SetPixel(x, y, new Color(color.r, color.g, 0));
				}
				else
				{
					if (azulDiferencia <= 0.4 && rojoDiferencia > azulDiferencia)
					{
						azul++;
						xazul += x;
						yazul += y;

						colorImg.SetPixel(x, y, new Color(0, 0, color.b));
					}
					else
					{
						nocolor++;
						colorImg.SetPixel(x, y, new Color(0, 0, 0));
					}
				}
			}
		}

		//Comprueba que color tiene más pixeles
		void Comparecolor()
		{
			if (verde > amarillo && verde > azul || verde > nocolor)
			{
				Color colorblue = new Color(0, 1, 0);
				Guardarposicion(verde, xverde, yverde, posicion, colorblue);
				contenedores(10, img.width / 3, posicion, 1);
			}
			else
			{
				if (amarillo > azul || amarillo > nocolor)
				{
					Color colorblue = new Color(1, 1, 0);
					Guardarposicion(amarillo, xamarillo, yamarillo, posicion, colorblue);
					contenedores(img.width / 3, img.width / 2 + 50, posicion, 2);
				}
				else
				{
					if (azul > verde)
					{
						Color colorblue = new Color(0, 0, 1);
						Guardarposicion(azul, xazul, yazul, posicion, colorblue);
						contenedores(img.width / 2 + 50, img.width - 20, posicion, 0);
					}
					else
					{
						numresiduo = 4;
						contains = false;
					}
				}
			}
		}

		//Guarda las posiciones que ocupara el objeto en la imagen
		void Guardarposicion(int pixelcolor, int xcolor, int ycolor, List<Vector2> poscolor, Color colorimg)
		{
			for (int x = 0; x < 25; x++)
			{
				for (int i = 0; i < 25; i++)
				{
					//Se crean nuevas posiciones a partir del centro del objeto
					Vector2 newpos = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor + i);
					Vector2 newpos2 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor - i);
					Vector2 newpos3 = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor - i);
					Vector2 newpos4 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor + i);
					poscolor.Add(newpos);
					poscolor.Add(newpos2);
					poscolor.Add(newpos3);
					poscolor.Add(newpos4);
					
					posImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor + i, colorimg);
					posImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor - i, colorimg);
					posImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor - i, colorimg);
					posImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor + i, colorimg);
				}
			}
		}

		//define las posiciones que ocupan los contenedores en la imagen y compara si alguna posicion coincide con la del objeto
		void contenedores(int equis, int ancho, List<Vector2> poscolor, int nresiduo)
		{
			for (int x = equis; x < ancho; x++)
			{
				for (int i = img.height / 4; i < img.height / 2 + img.height / 6; i++)
				{
					posImg.SetPixel(x, i, new Color(1, 1, 1));
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
			//Si algunas posiciones coinciden deteminara el tipo de residuo y comparara si coincide con el tipo de residuo que se ha establecido al principio
			if (contains == true)
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
		//quita el residuo, muestra el texto  y el temporizador
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

			yield return new WaitForSeconds(3f);
			resultado.text = "";

			cambio = false;

		}
	}
}