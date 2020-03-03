using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScreen : MonoBehaviour {
	private int check = 1, resolution;
	public GameObject menu1;
	public GameObject menu2;
	public GameObject menu3;
	public GameObject menu4;
	public GameObject menu5;
	public GameObject partikel;
	private int  posx, posy, posz; //posx =-65,
	private int qualityidx=1, resolutionidx=0;
	private string[] qualityScreen;
	private Resolution[] resolutionScreen;
	private bool fullscreenBool;

	// Use this for initialization
	void Start () {
		posx = (int)menu1.GetComponent<Transform> ().position.x - 40;
		posy = (int)menu1.GetComponent<Transform>().position.y-25;
		posz = (int)menu1.GetComponent<Transform>().position.z;
		qualityScreen = QualitySettings.names;
		resolutionScreen = Screen.resolutions;
		if (Screen.fullScreen) 
		{
			Debug.Log("ini fullscreen");
			fullscreenBool = true;
		}
		else{
			Debug.Log("ini windowed");
			fullscreenBool = false;
		}

		QualitySettings.SetQualityLevel (qualityidx, true);
		Debug.Log ("resolutionlength "+resolutionScreen.Length.ToString());
		Debug.Log ("res l w "+resolutionScreen[resolutionScreen.Length-1].width.ToString());
		Debug.Log ("res l h "+resolutionScreen[resolutionScreen.Length-1].height.ToString());
		Debug.Log ("res w "+Screen.currentResolution.width.ToString() );
		Debug.Log ("res h "+Screen.currentResolution.height.ToString() );
		//Screen.SetResolution(resolutionScreen[resolutionidx].width, resolutionScreen[resolutionidx].height, false);
		menu3.GetComponent<TextMesh>().text = "Resolution " + resolutionScreen[resolutionidx].width.ToString() + " x " + resolutionScreen[resolutionidx].height.ToString();
		menu4.GetComponent<TextMesh>().text = "Quality " + qualityScreen[qualityidx];
	}
	
	// Update is called once per frame
	void Update () {
		refresh();
		if (Input.GetKeyUp(KeyCode.W))
        {
            if (check == 1) { }
            else
            {
                check--;
            }

        }
		else if (Input.GetKeyUp(KeyCode.S))
        {
            if (check == 5) { }
            else
            {
                check++;
            }
        }

		if (Input.GetKeyUp(KeyCode.Return))
        {
            Debug.Log(check);
            if (check == 1)
            {
                Application.LoadLevel("state2");
            }
            else if (check == 2)
            {
				Application.LoadLevel("state3");
            }
            else if (check == 5)
            {
                Application.Quit();
            }
			refresh();
        }

		if (Input.GetKeyUp(KeyCode.A))
        {
            if (check == 3)
            {
                if (resolutionidx > 0)
                {
                    resolutionidx--;
                }
				Screen.SetResolution(resolutionScreen[resolutionidx].width, resolutionScreen[resolutionidx].height, fullscreenBool);
            }
            else if (check == 4)
            {
                if (qualityidx > 0)
                {
                    qualityidx--;
                }
				QualitySettings.SetQualityLevel(qualityidx, true);
            }

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (check == 3)
            {
				if (resolutionidx < resolutionScreen.Length-1)
                {
                    resolutionidx++;
                }
				Screen.SetResolution(resolutionScreen[resolutionidx].width, resolutionScreen[resolutionidx].height, fullscreenBool);
            }
            else if (check == 4)
            {
                if (qualityidx < qualityScreen.Length - 1)
                {
                    qualityidx++;
                }
				QualitySettings.SetQualityLevel(qualityidx, true);
            }

        }       	
	}

	void changePosition(int flag) {
		menu1.GetComponent<TextMesh>().color = Color.white;
		menu2.GetComponent<TextMesh>().color = Color.white;
		menu3.GetComponent<TextMesh>().color = Color.white;
		menu4.GetComponent<TextMesh>().color = Color.white;
		menu5.GetComponent<TextMesh>().color = Color.white;
		if (flag == 1) {
			menu1.GetComponent<TextMesh>().color = Color.red;
			posx = (int)menu1.GetComponent<Transform>().position.x-40;
			posy = (int)menu1.GetComponent<Transform>().position.y-25;
			posz = (int)menu1.GetComponent<Transform>().position.z;
		} else if (flag == 2) {
			menu2.GetComponent<TextMesh>().color = Color.red;
			posx = (int)menu2.GetComponent<Transform>().position.x-40;
			posy = (int)menu2.GetComponent<Transform>().position.y-25;
			posz = (int)menu2.GetComponent<Transform>().position.z;
		} else if (flag == 3) {
			menu3.GetComponent<TextMesh>().color = Color.red;
			posx = (int)menu3.GetComponent<Transform>().position.x-40;
			posy = (int)menu3.GetComponent<Transform>().position.y-25;
			posz = (int)menu3.GetComponent<Transform>().position.z;
		} else if (flag == 4) {
			menu4.GetComponent<TextMesh>().color = Color.red;
			posx = (int)menu4.GetComponent<Transform>().position.x-40;
			posy = (int)menu4.GetComponent<Transform>().position.y-25;
			posz = (int)menu4.GetComponent<Transform>().position.z;
		} else if (flag == 5) {
			menu5.GetComponent<TextMesh>().color = Color.red;
			posx = (int)menu5.GetComponent<Transform>().position.x-40;
			posy = (int)menu5.GetComponent<Transform>().position.y-25;
			posz = (int)menu5.GetComponent<Transform>().position.z;
		}
	}

    void refresh(){
		changePosition(check);
        partikel.GetComponent<Transform>().position = new Vector3(posx, posy, posz); 
		menu3.GetComponent<TextMesh>().text = "Resolution " + resolutionScreen[resolutionidx].width.ToString() + " x " + resolutionScreen[resolutionidx].height.ToString();
		menu4.GetComponent<TextMesh>().text = "Quality " + qualityScreen[qualityidx];
    }
}