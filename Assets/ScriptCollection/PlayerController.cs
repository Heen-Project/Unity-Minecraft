using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public GameObject partikel;
    public AudioClip audioHit;
    public AudioClip audioObtain;
    public bool attack = true;
    private bool coolDown = false, lockcursor = true, showcursor = false;
    float timerCahaya = 1.2f;
    float cahayaIntensity = -0.0084f;
    private Inventory invent;

    // Use this for initialization
    void Start()
    {
        Screen.lockCursor = lockcursor;
        Screen.showCursor = showcursor;
        invent = GameObject.FindGameObjectWithTag("inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
			if (this.gameObject.GetComponent<Transform>().position.y<-30){
				this.gameObject.GetComponent<Transform>().position = new Vector3(this.gameObject.GetComponent<Transform>().position.x-5, 70, this.gameObject.GetComponent<Transform>().position.z-5);
			}
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 4))
            {
                if (Input.GetButton("Fire1") && !coolDown)
                {
                    coolDown = true;
                    StartCoroutine(coolDownFn());
                    plankInfo plank = hit.collider.gameObject.GetComponent<plankInfo>();
                    craftingtableInfo crafttabel = hit.collider.gameObject.GetComponent<craftingtableInfo>();
                    grassInfo grass = hit.collider.gameObject.GetComponent<grassInfo>();
                    cobblestoneInfo cobblestone = hit.collider.gameObject.GetComponent<cobblestoneInfo>();
                    woodInfo wood = hit.collider.gameObject.GetComponent<woodInfo>();


                    AudioSource.PlayClipAtPoint(audioHit, hit.collider.gameObject.transform.position);
                    if (grass)
                    {
                        Destroy(Instantiate(partikel, hit.collider.transform.position, Quaternion.identity), 0.3f);
                        if (grass.grassHitFn() < 1)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniGrass"), hit.collider.gameObject.transform.position, Quaternion.identity);
                        }
                        if (invent.instantDestroy() == 20 ||
                            invent.instantDestroy() == 21 ||
                            invent.instantDestroy() == 22 ||
                            invent.instantDestroy() == 23)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniGrass"), hit.collider.gameObject.transform.position, Quaternion.identity);
                            invent.healthminus();
                        }
                    }
                    if (cobblestone)
                    {
                        Destroy(Instantiate(partikel, hit.collider.transform.position, Quaternion.identity), 0.3f);
                        if (cobblestone.cobblestoneHitFn() < 1)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniCobblestone"), hit.collider.gameObject.transform.position, Quaternion.identity);
                        }
                        if (invent.instantDestroy() == 16 ||
                            invent.instantDestroy() == 17 ||
                            invent.instantDestroy() == 18 ||
                            invent.instantDestroy() == 19)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniCobblestone"), hit.collider.gameObject.transform.position, Quaternion.identity);
                            invent.healthminus();
                        }
                    }
                    if (wood)
                    {
                        Destroy(Instantiate(partikel, hit.collider.transform.position, Quaternion.identity), 0.3f);
                        if (wood.woodHitFn() < 1)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniWood"), hit.collider.gameObject.transform.position, Quaternion.identity);
                        }
                        if (invent.instantDestroy() == 12 ||
                            invent.instantDestroy() == 13 ||
                            invent.instantDestroy() == 14 ||
                            invent.instantDestroy() == 15)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniWood"), hit.collider.gameObject.transform.position, Quaternion.identity);
                            invent.healthminus();
                        }
                    }


                    if (plank)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        Destroy(Instantiate(partikel, hit.collider.transform.position, Quaternion.identity), 0.3f);
                        if (plank.plankHitFn() < 1)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniPlank"), hit.collider.gameObject.transform.position, Quaternion.identity);
                        }
                        if (invent.instantDestroy() == 12 ||
                            invent.instantDestroy() == 13 ||
                            invent.instantDestroy() == 14 ||
                            invent.instantDestroy() == 15)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniPlank"), hit.collider.gameObject.transform.position, Quaternion.identity);
                            invent.healthminus();
                        }
                    }
                    if (crafttabel)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        Destroy(Instantiate(partikel, hit.collider.transform.position, Quaternion.identity), 0.3f);
                        if (crafttabel.craftingtableHitFn() < 1)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniCraftingTable"), hit.collider.gameObject.transform.position, Quaternion.identity);
                        }
                        if (invent.instantDestroy() == 12 ||
                            invent.instantDestroy() == 13 ||
                            invent.instantDestroy() == 14 ||
                            invent.instantDestroy() == 15)
                        {
                            Destroy(hit.collider.gameObject);
							GameObject.Instantiate((GameObject)Resources.Load("Prefab/miniCraftingTable"), hit.collider.gameObject.transform.position, Quaternion.identity);
                            invent.healthminus();
                        }
                    }
                }
            }
        }
        if (Input.GetButtonDown("showInvent"))
        {
            toggle();
        }
        cahayaFn();
    }

    void OnControllerColliderHit(ControllerColliderHit collider)
    {
        if (collider.gameObject.tag == "mini")
        {
            Destroy(collider.gameObject);
            Inventory inventory = GameObject.FindGameObjectWithTag("inventory").GetComponent<Inventory>();
            inventory.getItem(collider.gameObject.name.Replace("(Clone)", ""));
            AudioSource.PlayClipAtPoint(audioObtain, collider.gameObject.transform.position);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 20, 20), "+");
    }

    IEnumerator coolDownFn()
    {
        yield return new WaitForSeconds(0.5f);
        coolDown = false;
    }

    void cahayaFn()
    {
        timerCahaya -= Time.deltaTime;
        if (timerCahaya <= 0)
        {
            GameObject.Find("cahayamatahari").GetComponent<Light>().intensity += cahayaIntensity;
            timerCahaya = 1.2f;
            if (GameObject.Find("cahayamatahari").GetComponent<Light>().intensity == 0 || GameObject.Find("cahayamatahari").GetComponent<Light>().intensity > 1)
            {
                cahayaIntensity *= -1;
            }
        }
    }

    void toggle()
    {
        attack = !attack;
        Screen.lockCursor = !lockcursor;
        Screen.showCursor = !showcursor;
    }

}