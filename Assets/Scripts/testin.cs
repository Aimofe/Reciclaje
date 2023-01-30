using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testin : MonoBehaviour
{
	public static WebCamTexture camTex;
	//public float th = 0.09f;
	//System.Diagnostics.Stopwatch s;
	static Texture2D img, rImg, bImg;// kImg, roImg, goImg, boImg, koImg, ORimg, ANDimg, SUMimg, AVGimg;
	//static float[,] rL, gL, bL, kL, ORL, ANDL, SUML, AVGL;
	static Color color;
	static float brillo;
	public bool cambio;
	public int numresiduo=4;
	//public GameObject papel, plastico, vidrio;
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
		//GetWebCamImage();
		img = new Texture2D(camTex.width, camTex.height);
		//initialize textures
		rImg = new Texture2D(img.width, img.height);
		bImg = new Texture2D(img.width, img.height);
		//gImg = new Texture2D(img.width, img.height);
		//kImg = new Texture2D(img.width, img.height);
		//initialize gradient arrays
		//rL = new float[img.width, img.height];
		//gL = new float[img.width, img.height];
		//bL = new float[img.width, img.height];
		//kL = new float[img.width, img.height];
		//ORL = new float[img.width, img.height];
		//ANDL = new float[img.width, img.height];
		//SUML = new float[img.width, img.height];
		//AVGL = new float[img.width, img.height];
		////initialize final textures
		//roImg = new Texture2D(img.width, img.height);
		//goImg = new Texture2D(img.width, img.height);
		//boImg = new Texture2D(img.width, img.height);
		//koImg = new Texture2D(img.width, img.height);
		//ORimg = new Texture2D(img.width, img.height);
		//ANDimg = new Texture2D(img.width, img.height);
		//SUMimg = new Texture2D(img.width, img.height);
		//AVGimg = new Texture2D(img.width, img.height);
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
		//Debug.Log("residuo" + residuo);
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

		//Vector2 verdepos1 = new Vector2(xverde / verde, verde + i);
		var pixelData = bImg.GetPixels();
        //print("Total pixels" + pixelData.Length);

  //      if (verde>100)
  //      {
		//	Guardarposicion(verde, xverde, yverde, posicion);
		//	contenedores(10, 10, posicion, 1);
		//}
		Comparecolor();

		void detectarColor(int x, int y)
        {
			//bImg.SetPixel(x, y, new Color(color.r, 0, 0));

			color = img.GetPixel(x, y);
			//Debug.Log(color.g);
			//rImg.SetPixel(x, y, new Color(color.r, color.g, 0));
			//gImg.SetPixel(x, y, new Color(0, color.g, 0));
			bImg.SetPixel(x, y, new Color(color.r, color.g, color.b));
			brillo = color.r + color.g + color.b;
			brillo /= 3f;

			//kImg.SetPixel(x, y, new Color(brillo, brillo, brillo));

			Vector3 colorimgG = new Vector3(0, color.g, 0);
			Vector3 colorimgR = new Vector3(color.r, 0, 0);
			Vector3 colorimgY = new Vector3(color.r, color.g, 0);
			Vector3 colorimgB = new Vector3(0, 0, color.b);
			//Color verdemin = new Color(0.09, 0.576, 0.455);
			//Debug.Log = (colorimgY.magnitude);


			Vector3 magnitudVERDE = new Vector3(0, 1, 0);
			Vector3 magnitudAmarillo = new Vector3(1, 1, 0);
			Vector3 magnitudAzul = new Vector3(0, 0, 1);
			Vector3 magnitudRojo = new Vector3(1, 0, 0);
			verdeDiferencia = Mathf.Abs(magnitudVERDE.magnitude - colorimgG.magnitude);
			amarilloDiferencia = Mathf.Abs(magnitudAmarillo.magnitude - colorimgY.magnitude);
			azulDiferencia = Mathf.Abs(magnitudAzul.magnitude - colorimgB.magnitude);
			rojoDiferencia = Mathf.Abs(magnitudRojo.magnitude - colorimgR.magnitude);


			//Debug.Log(amarilloDiferencia+"amarillo "+verdeDiferencia+" verde "+azulDiferencia+" azul "+rojoDiferencia+" rojo "+brillo+ " brillo ");
			//Debug.Log(verdeDiferencia);
			if (verdeDiferencia <= 0.5 && amarilloDiferencia > verdeDiferencia && verdeDiferencia < azulDiferencia && rojoDiferencia > verdeDiferencia)
			{
				//Debug.Log(verde+"verde");
				verde++;
				xverde += x;
				yverde += y;
				
				

				//gL[x, y] = gradientValue(x, y, 1, img);
				rImg.SetPixel(x, y, new Color(0, color.g, 0));
			}
			else
			{
				if (amarilloDiferencia <= 0.5 && amarilloDiferencia < azulDiferencia)
				{
					//Debug.Log(amarillo+"amarillo");
					amarillo++;
					xamarillo += x;
					yamarillo += y;

					rImg.SetPixel(x, y, new Color(color.r, color.g, 0));
				}
				else
				{
					if (azulDiferencia <= 0.5 && rojoDiferencia > azulDiferencia)
					{
						//Debug.Log("azul"+azul);
						azul++;
						xazul += x;
						yazul += y;
						rImg.SetPixel(x, y, new Color(0, 0, color.b));
					}
					else
					{
						nocolor++;
						rImg.SetPixel(x, y, new Color(0, 0, 0));
						//Debug.Log("no color");
					}
				}
			}
		}

		
		void Comparecolor()
        {
			if (verde > amarillo && verde > azul || verde > nocolor)
			{
				//Debug.Log("verde");
				//numresiduo = 1;
				Color colorblue = new Color(0, 1, 0);
				Guardarposicion(verde, xverde, yverde, posicion,colorblue);
				contenedores(10, img.width/3, posicion, 1);
			}
			else
			{
				if (amarillo > azul || amarillo > nocolor)
				{
					//Debug.Log("amarillo");
					//numresiduo = 2;
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
					//bImg.SetPixel(50 + x, 50 + i, new Color(0, 0, 0));


					Vector2 newpos = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor + i);
					Vector2 newpos2 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor - i);
					Vector2 newpos3 = new Vector2(xcolor / pixelcolor + x, ycolor / pixelcolor - i);
					Vector2 newpos4 = new Vector2(xcolor / pixelcolor - x, ycolor / pixelcolor + i);
					poscolor.Add(newpos);
					poscolor.Add(newpos2);
					poscolor.Add(newpos3);
					poscolor.Add(newpos4);
					//if (x<=20 && i<=20)
     //               {
					//	bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor + i, new Color (0,0,0));
					//	bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor - i, new Color(0, 0, 0));
					//}
     //               else
                    //{
						//bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor, colorimg);
						//bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor , colorimg);

						bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor + i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor - i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor + x, ycolor / pixelcolor - i, colorimg);
						bImg.SetPixel(xcolor / pixelcolor - x, ycolor / pixelcolor + i, colorimg);
					//}
					
					//Vector2 verdepos = new Vector2(xverde / verde + x, yverde / verde + i);
					//posicion.Add(verdepos);
					//bImg.SetPixel(xverde / verde + x, yverde / verde + i, new Color(0, color.g, 0));


					//Vector2 posicioncubo = new Vector2(x, i);
					//if (posicion.Contains(verdepos) == true)
					//{
					//	Debug.Log("vidrio");
					//}   
				}

			}

		}
		
		void contenedores(int equis, int ancho,List<Vector2> poscolor, int nresiduo)
		{
			//vidrio.transform.position = new Vector3((xverde / verde)-img.width, (yverde / verde)-30, 0);
			for (int x = equis; x < ancho; x++)
			{
				for (int i = img.height / 4; i < img.height / 2+img.height / 6; i++)
				{
					bImg.SetPixel(x, i, new Color(1, 1, 1));
					//detectarColor(x,i);
					Vector2 posicioncubo = new Vector2(x, i);
					//Debug.Log(posicioncubo);
					if (poscolor.Contains(posicioncubo) == true)
					{
						
						contains = true;
						
						break;
					}
                    else
                    {
						contains = false;
                    }
					//else
					//{
					//	numresiduo = 4;
					//}
					//Comparecolor();
					//bImg.SetPixel(xverde / verde , yverde / verde, new Color(0, color.g, 0));
					//if (xverde/verde-x)
					//{

					//}

				}
			}
            if (contains==true)
            {
				//Debug.Log(nresiduo);
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
		
		//calculate gradient values
		for (int x = 0; x < img.width; x++)
		{
			for (int y = 0; y < img.height; y++)
			{
				//rL[x, y] = gradientValue(x, y, 0, rImg);
				//gL[x, y] = gradientValue(x, y, 1, gImg);
				//bL[x, y] = gradientValue(x, y, 2, bImg);
				//kL[x, y] = gradientValue(x, y, 2, kImg);
				//ORL[x, y] = (rL[x, y] >= th || gL[x, y] >= th || bL[x, y] >= th) ? th : 0f;
				//ANDL[x, y] = (rL[x, y] >= th && gL[x, y] >= th && bL[x, y] >= th) ? th : 0f;
				//SUML[x, y] = rL[x, y] + gL[x, y] + bL[x, y];
				//AVGL[x, y] = SUML[x, y] / 3f;
			}
		}
		//create texture from gradient values
		//TextureFromGradientRef(rL, th, ref roImg);
		//TextureFromGradientRef(gL, th, ref goImg);
		//TextureFromGradientRef(bL, th, ref boImg);
		//TextureFromGradientRef(kL, th, ref koImg);
		//TextureFromGradientRef(ORL, th, ref ORimg);
		//TextureFromGradientRef(ANDL, th, ref ANDimg);
		//TextureFromGradientRef(SUML, th, ref SUMimg);
		//TextureFromGradientRef(AVGL, th, ref AVGimg);
	}
	IEnumerator Wait()
	{
		resultado.text = "Correcto";
		timer.SetActive(true);
		//Debug.Log("Cambio");
		cambio = true;

		basura[residuo].SetActive(false);

		//yield return new WaitForSeconds(0.3f);

		
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

        // GUILayout.Label("original");
        //GUILayout.Label(camTex);
        GUILayout.Label("color");

        GUILayout.Label(rImg);
        GUILayout.Label("blue");
        GUILayout.Label(bImg);

        GUILayout.EndVertical();

        //GUILayout.BeginVertical();
        //GUILayout.Label("green");
        //GUILayout.Label(gImg);
        //GUILayout.Label("grey");
        //GUILayout.Label(kImg);
        //GUILayout.Label("grey detection");
        //GUILayout.Label(koImg);
        //GUILayout.EndVertical();

        //GUILayout.BeginVertical();
        //GUILayout.Label("red detection");
        //GUILayout.Label(roImg);
        //GUILayout.Label("green detection");
        //GUILayout.Label(goImg);
        //GUILayout.Label("blue detection");
        //GUILayout.Label(boImg);
        //GUILayout.EndVertical();

        //GUILayout.BeginVertical();
        //GUILayout.Label("OR detection");
        //GUILayout.Label(ORimg);
        //GUILayout.Label("AND detection");
        //GUILayout.Label(ANDimg);
        //GUILayout.Label("SUM detection");
        //GUILayout.Label(SUMimg);
        //GUILayout.EndVertical();

        //GUILayout.BeginVertical();
        //GUILayout.Label("Average detection");
        //GUILayout.Label(AVGimg);
        //GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    //float gradientValue(int ex, int why, int colorVal, Texture2D image)
    //{
    //	float lx = 0f;
    //	float ly = 0f;
    //	if (ex > 0 && ex < image.width)
    //		lx = 0.5f * (image.GetPixel(ex + 1, why)[colorVal] - image.GetPixel(ex - 1, why)[colorVal]);
    //	if (why > 0 && why < image.height)
    //		ly = 0.5f * (image.GetPixel(ex, why + 1)[colorVal] - image.GetPixel(ex, why - 1)[colorVal]);
    //	//Debug.Log("pos" + lx * lx + ly * ly);
    //	return Mathf.Sqrt(lx * lx + ly * ly);

    //}

    //Texture2D TextureFromGradient(float[,] g, float thres)
    //{
    //	Texture2D output = new Texture2D(g.GetLength(0), g.GetLength(1));
    //	for (int x = 0; x < output.width; x++)
    //	{
    //		for (int y = 0; y < output.height; y++)
    //		{
    //			if (g[x, y] >= thres)
    //				output.SetPixel(x, y, Color.black);
    //			else
    //				output.SetPixel(x, y, Color.white);
    //		}
    //	}
    //	output.Apply();
    //	return output;
    //}

    //void TextureFromGradientRef(float[,] g, float thres, ref Texture2D output)
    //{
    //	for (int x = 0; x < output.width; x++)
    //	{
    //		for (int y = 0; y < output.height; y++)
    //		{
    //			if (g[x, y] >= thres)
    //				output.SetPixel(x, y, Color.black);
    //			else
    //				output.SetPixel(x, y, Color.white);
    //		}
    //	}
    //	output.Apply();
    //}
}