using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour
{
	//application.screencapturescreen -> screenshoot
	public AudioClip audioInstantiate;
	public Texture2D inventoryIcon, healthBar;
	private Texture2D iconDragged;
	private ItemDatabase database;
	private craftingBook craftrecipe;
	private MouseLook mainCameraControl;
	private Item[,] inventoryLocation = new Item[5, 8];
	private Item[] quickslotLocation = new Item[9];
	private Item[,] basicCraftLocation = new Item[2, 2];
	private Item[,] boxCraftLocation = new Item[3, 3];
	private Item[,] magicBoxLocation = new Item[5, 10];
	private Item tempLocation, craftResultLocation;
	private int[,] inventoryCount = new int[5, 8];
	private int[] quickslotCount = new int[9];
	private int[,] basicCraftCount = new int[2, 2];
	private int[,] boxCraftCount = new int[3, 3];
	private int tempCount, craftResultCount, quickslotCurrent = 0;
	private bool isDragging = false, showItem = false, craftBox = false, resultCondition = false, magicboxCondition = false, 
	escapeCondition = false, screenShootCondition= true;
	private int itemSource = 0;
	private int col, row, moverow, movecol;
	private PlayerController playercontrol;
	
	void Start()
	{
		Time.timeScale = 1.0f;
		database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
		craftrecipe = GameObject.FindGameObjectWithTag("craft").GetComponent<craftingBook>();
		playercontrol = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		mainCameraControl = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MouseLook> ();
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Cancel")) {
			Debug.Log("esc button");
			if (escapeCondition){
				playercontrol.GetComponent<MouseLook>().sensitivityX=15;
				mainCameraControl.sensitivityY=10;
				playercontrol.attack=!playercontrol.attack;
				escapeCondition=false;
				Time.timeScale = 1.0f;
			}
			else if (!escapeCondition){
				playercontrol.GetComponent<MouseLook>().sensitivityX=0;
				mainCameraControl.sensitivityY=0;
				playercontrol.attack = false;
				escapeCondition=true;
				isDragging = false;
				showItem = false;
				craftBox = false;
				magicboxCondition = false;
				Time.timeScale = 0.0f;
			}
			
		}
		//Debug.Log("result condition >> " + resultCondition);
		if (Input.GetButtonDown("showInvent") && !escapeCondition)
		{
			if (showItem)
			{
				playercontrol.GetComponent<MouseLook>().sensitivityX=15;
				mainCameraControl.sensitivityY=10;
				playercontrol.attack = false;
				showItem = false;
				Screen.lockCursor = true;
				Screen.showCursor = false;
				isDragging=false;
				
			}
			else if (!showItem)
			{
				playercontrol.GetComponent<MouseLook>().sensitivityX=0;
				mainCameraControl.sensitivityY=0;
				playercontrol.attack = true;
				showItem = true;
				Screen.lockCursor = false;
				Screen.showCursor = true;
			}
			Time.timeScale = 1.0f;
			magicboxCondition = false;
			craftBox = false;
		}
		//lagi ga drag item
		if (Input.GetButtonUp("Fire1") && !isDragging && showItem && !magicboxCondition)
		{
			//validasi inventory
			if (((int)Input.mousePosition.x / 40) < 8 && ((int)(Screen.height - Input.mousePosition.y) / 40) < 5)
			{
				col = (int)Input.mousePosition.x / 40;
				row = (int)(Screen.height - Input.mousePosition.y) / 40;
				Debug.Log(col);
				Debug.Log(row);
				if (inventoryLocation[row, col] != null)
				{
					isDragging = true;
					//Debug.Log (isDragging);
					iconDragged = inventoryLocation[row, col].icon;
					tempLocation = inventoryLocation[row, col];
					tempCount = inventoryCount[row, col];
					itemSource = 1;
				}
			}
			//validasi quickslot
			else if (((int)Input.mousePosition.x / 40) < 9 && ((int)Input.mousePosition.y / 40) < 1)
			{
				col = (int)Input.mousePosition.x / 40;
				Debug.Log(col);
				if (quickslotLocation[col] != null)
				{
					isDragging = true;
					iconDragged = quickslotLocation[col].icon;
					tempLocation = quickslotLocation[col];
					tempCount = quickslotCount[col];
					itemSource = 2;
				}
			}
			else
			{
				//validasi crafting 2x2
				if (!craftBox)
				{
					//craft 2x2
					if ((((int)(Screen.width - Input.mousePosition.x) / 40) < 2) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 2))
					{
						col = (int)((Input.mousePosition.x - Screen.width) / 40) + 1;
						row = (int)(Screen.height - Input.mousePosition.y) / 40;
						Debug.Log(col);
						Debug.Log(row);
						if (basicCraftLocation[row, col] != null)
						{
							isDragging = true;
							iconDragged = basicCraftLocation[row, col].icon;
							tempLocation = basicCraftLocation[row, col];
							tempCount = basicCraftCount[row, col];
							itemSource = 3;
						}
					}
					else if ((((int)(Screen.width - Input.mousePosition.x) / 40) < 2) && (((int)(Screen.height - Input.mousePosition.y) / 40) > 2) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 4))
					{
						// craft result 2x2
						Debug.Log("craft result di hit 2x2");
						Debug.Log("result condition " + resultCondition);
						if (!resultCondition)
						{
							int[,] tempitemcraft = new int[2, 2];
							for (int i = 0; i < 2; i++)
							{
								for (int j = 0; j < 2; j++)
								{
									if (basicCraftLocation[i, j] != null)
									{
										tempitemcraft[i, j] = basicCraftLocation[i, j].idItem;
										// Debug.Log("row " + i + " col " + j + " craft");
									}
									else
									{
										tempitemcraft[i, j] = 25;
										//Debug.Log("row " + i + " col " + j + " null");
									}
								}
							}
							craftResultLocation = craftrecipe.recipe2x2(
								tempitemcraft[0, 0], tempitemcraft[0, 1],
								tempitemcraft[1, 0], tempitemcraft[1, 1]);
							
							if (craftResultLocation != null)
							{
								for (int i = 0; i < 2; i++)
								{
									for (int j = 0; j < 2; j++)
									{
										basicCraftLocation[i, j] = null;
										basicCraftCount[i, j] = 0;
									}
								}
								if (craftResultLocation.idItem == 3)
								{
									craftResultCount = 4;
								}
								else if (craftResultLocation.idItem == 8)
								{
									craftResultCount = 9;
								}
								else if (craftResultLocation.idItem == 9)
								{
									craftResultCount = 4;
								}
								else
								{
									craftResultCount = 1;
								}
								resultCondition = true;
							}
							Debug.Log("result condition " + resultCondition);
						}
						else if (resultCondition)
						{
							isDragging = true;
							iconDragged = craftResultLocation.icon;
							tempLocation = craftResultLocation;
							tempCount = craftResultCount;
							itemSource = 4;
						}
						
						
					}
				}
				//validasi crafting 3x3
				else
				{
					//craft 3x3
					if ((((int)(Screen.width - Input.mousePosition.x) / 40) < 3) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 3))
					{
						
						col = (int)((Input.mousePosition.x - Screen.width) / 40) + 2;
						row = (int)(Screen.height - Input.mousePosition.y) / 40;
						Debug.Log(col);
						Debug.Log(row);
						if (boxCraftLocation[row, col] != null)
						{
							isDragging = true;
							iconDragged = boxCraftLocation[row, col].icon;
							tempLocation = boxCraftLocation[row, col];
							tempCount = boxCraftCount[row, col];
							itemSource = 5;
						}
					}
					//craft result 3x3
					else if (((int)(Screen.width - Input.mousePosition.x) / 40 < 3) && (((int)(Screen.height - Input.mousePosition.y) / 40) > 3) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 6))
					{
						Debug.Log("result condition " + resultCondition);
						Debug.Log("craft result di hit 3x3");
						if (!resultCondition)
						{
							int[,] tempitemcraft = new int[3, 3];
							for (int i = 0; i < 3; i++)
							{
								for (int j = 0; j < 3; j++)
								{
									if (boxCraftLocation[i, j] != null)
									{
										tempitemcraft[i, j] = boxCraftLocation[i, j].idItem;
									}
									else
									{
										tempitemcraft[i, j] = 25;
									}
								}
							}
							
							craftResultLocation = craftrecipe.recipe3x3(
								tempitemcraft[0, 0], tempitemcraft[0, 1], tempitemcraft[0, 2],
								tempitemcraft[1, 0], tempitemcraft[1, 1], tempitemcraft[1, 2],
								tempitemcraft[2, 0], tempitemcraft[2, 1], tempitemcraft[2, 2]
								);
							
							
							if (craftResultLocation != null)
							{
								for (int i = 0; i < 3; i++)
								{
									for (int j = 0; j < 3; j++)
									{
										boxCraftLocation[i, j] = null;
										boxCraftCount[i, j] = 0;
									}
								}
								if (craftResultLocation.idItem == 3)
								{
									craftResultCount = 4;
								}
								else if (craftResultLocation.idItem == 8)
								{
									craftResultCount = 9;
								}
								else if (craftResultLocation.idItem == 9)
								{
									craftResultCount = 4;
								}
								else
								{
									craftResultCount = 1;
								}
								resultCondition = true;
							}
							Debug.Log("result condition " + resultCondition);
							
						}
						else if (resultCondition)
						{
							isDragging = true;
							iconDragged = craftResultLocation.icon;
							tempLocation = craftResultLocation;
							tempCount = craftResultCount;
							itemSource = 6;
						}
						
					}
				}
			}
		}
		
		//lagi ngedrag item kiri
		else if (Input.GetButtonUp("Fire1") && isDragging && showItem && !magicboxCondition)
		{
			//validasi inventory
			if (((int)Input.mousePosition.x / 40) < 8 && ((int)(Screen.height - Input.mousePosition.y) / 40) < 5)
			{
				movecol = (int)Input.mousePosition.x / 40;
				moverow = (int)(Screen.height - Input.mousePosition.y) / 40;
				swap(tempLocation, tempCount, inventoryLocation[moverow, movecol], inventoryCount[moverow, movecol], itemSource, 1);
				isDragging = !isDragging;
			}
			//validasi quickslot
			else if (((int)Input.mousePosition.x / 40) < 9 && ((int)Input.mousePosition.y / 40) < 1)
			{
				movecol = (int)Input.mousePosition.x / 40;
				Debug.Log(col);
				swap(tempLocation, tempCount, quickslotLocation[movecol], quickslotCount[movecol], itemSource, 2);
				isDragging = !isDragging;
			}
			else
			{
				//validasi crafting 2x2
				if (!craftBox)
				{
					if ((((int)(Screen.width - Input.mousePosition.x) / 40) < 2) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 2))
					{
						movecol = (int)((Input.mousePosition.x - Screen.width) / 40) + 1;
						moverow = (int)(Screen.height - Input.mousePosition.y) / 40;
						swap(tempLocation, tempCount, basicCraftLocation[moverow, movecol], basicCraftCount[moverow, movecol], itemSource, 3);
						isDragging = !isDragging;
					}
				}
				//validasi crafting 3x3
				else
				{
					if ((((int)(Screen.width - Input.mousePosition.x) / 40) < 3) && (((int)(Screen.height - Input.mousePosition.y) / 40) < 3))
					{
						movecol = (int)((Input.mousePosition.x - Screen.width) / 40) + 2;
						moverow = (int)(Screen.height - Input.mousePosition.y) / 40;
						swap(tempLocation, tempCount, boxCraftLocation[moverow, movecol], boxCraftCount[moverow, movecol], itemSource, 4);
						isDragging = !isDragging;
					}
				}
			}
		}
		//lagi ngedrag item kanan
		else if (Input.GetButtonUp("Fire2") && isDragging && showItem && !magicboxCondition)
		{
			Debug.Log("kanan");
			//validasi inventory
			if (itemSource == 1)
			{
				inventoryLocation[row, col] = null;
				inventoryCount[row, col] = 0;
				iconDragged = null;
				isDragging = !isDragging;
			}
			else if (itemSource == 2)
			{
				quickslotLocation[col] = null;
				quickslotCount[col] = 0;
				iconDragged = null;
				isDragging = !isDragging;
			}
			else if (itemSource == 3)
			{
				basicCraftLocation[row, col] = null;
				basicCraftCount[row, col] = 0;
				iconDragged = null;
				isDragging = !isDragging;
			}
			else if (itemSource == 4 || itemSource == 6)
			{
				craftResultLocation = null;
				craftResultCount = 0;
				iconDragged = null;
				isDragging = !isDragging;
			}
			else if (itemSource == 5)
			{
				boxCraftLocation[row, col] = null;
				boxCraftCount[row, col] = 0;
				iconDragged = null;
				isDragging = !isDragging;
			}
		}
		//get item from magic box
		else if (Input.GetButtonUp("Fire1") && !isDragging && !showItem && magicboxCondition)
		{
			if (((int)Input.mousePosition.x / 40) < 10 && ((int)(Screen.height - Input.mousePosition.y) / 40) < 5)
			{
				col = (int)Input.mousePosition.x / 40;
				row = (int)(Screen.height - Input.mousePosition.y) / 40;
				Debug.Log(col);
				Debug.Log(row);
				int idtempgetfrommagicbox = ((row * 10) + col);
				Debug.Log("item dari index " + idtempgetfrommagicbox);
				Item tempgetfrommagicbox = database.getItem(idtempgetfrommagicbox);
				if (tempgetfrommagicbox.idItem < 24)
				{
					for (int i = 0; i < 5; i++)
					{
						for (int j = 0; j < 8; j++)
						{
							if (i == 4 && j == 7)
							{
								return;
							}
							else if (inventoryLocation[i, j] == null)
							{
								inventoryLocation[i, j] = tempgetfrommagicbox;
								if (tempgetfrommagicbox.idItem >= 0 && tempgetfrommagicbox.idItem < 10)
								{
									inventoryCount[i, j] = 64;
								}
								else if (tempgetfrommagicbox.idItem >= 10 && tempgetfrommagicbox.idItem < 24)
								{
									inventoryCount[i, j]++;
								}
								return;
							}
						}
					}
				}
			}
		}
		//instantiate object & magibox & crafting table
		else if (Input.GetButtonUp("Fire2") && !isDragging && !showItem)
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 4))
			{
				if (Input.GetButtonUp("Fire2") && hit.collider.gameObject.name == "normMagixBox")
				{
					Debug.Log("hit magicbox");
					Debug.Log("magixbox condition >> "+craftBox);
					showMagicboxFn();
					
				}
				else if (Input.GetButtonUp("Fire2") && hit.collider.gameObject.name == "normCraftingTable(Clone)" && !showItem && !magicboxCondition)
				{
					craftboxFn();
					
				}
				else if ((Input.GetButtonUp("Fire2") && quickslotLocation[quickslotCurrent] != null))
				{
					AudioSource.PlayClipAtPoint(audioInstantiate, hit.collider.gameObject.transform.position);
					instantiateObject(quickslotCurrent, hit.collider.transform.position, hit.normal);
				}
			}
		}
		
		
		
		
		//input quickslot
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			quickslotCurrent = 0;
			Debug.Log("quick key 1");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			quickslotCurrent = 1;
			Debug.Log("quick key 2");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			quickslotCurrent = 2;
			Debug.Log("quick key 3");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			quickslotCurrent = 3;
			Debug.Log("quick key 4");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha5))
		{
			quickslotCurrent = 4;
			Debug.Log("quick key 5");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha6))
		{
			quickslotCurrent = 5;
			Debug.Log("quick key 6");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha7))
		{
			quickslotCurrent = 6;
			Debug.Log("quick key 7");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha8))
		{
			quickslotCurrent = 7;
			Debug.Log("quick key 8");
		}
		else if (Input.GetKeyUp(KeyCode.Alpha9))
		{
			quickslotCurrent = 8;
			Debug.Log("quick key 9");
		}
		
		
		
		
		
		
		
	}
	
	public void getItem(string ItemName)
	{
		int id = 25;
		
		if (ItemName == "miniCobbleStone")
		{
			id = 1;
		}
		else if (ItemName == "miniGrass")
		{
			id = 0;
		}
		else if (ItemName == "miniWood")
		{
			id = 2;
		}
		else if (ItemName == "miniPlank")
		{
			id = 3;
		}
		else if (ItemName == "miniCraftingTable")
		{
			id = 4;
		}
		
		Item temp = database.getItem(id);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (inventoryLocation[i, j] == temp && inventoryCount[i, j] < 64)
				{
					Debug.Log(inventoryLocation[i, j].name);
					inventoryCount[i, j]++;
					return;
				}
			}
		}
		
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				if (i == 4 && j == 7)
				{
					return;
				}
				if (inventoryLocation[i, j] == null)
				{
					inventoryLocation[i, j] = temp;
					inventoryCount[i, j]++;
					return;
				}
			}
		}
	}
	
	void OnGUI()
	{
		if (showItem)
		{
			//inventory
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					GUI.Label(new Rect(j * 40, i * 40, 40, 40), inventoryIcon);
					if (inventoryLocation[i, j] != null)
					{
						GUI.Label(new Rect(j * 40, i * 40, 40, 40), inventoryLocation[i, j].icon);
						//item
						if (inventoryLocation[i, j].idItem < 10 && inventoryLocation[i, j].idItem >= 0)
						{
							GUI.Label(new Rect(j * 40, i * 40, 40, 40), inventoryCount[i, j].ToString());
						}
						//tools
						else if (inventoryLocation[i, j].idItem < 24 && inventoryLocation[i, j].idItem >= 10)
						{
							if (inventoryLocation[i, j].health <= 0)
							{
								inventoryLocation[i, j] = null;
								inventoryCount[i, j] = 0;
							}
							else
							{
								GUI.DrawTexture(new Rect(j * 40, i * 40 + 30, 35 * ((float)inventoryLocation[i, j].health / inventoryLocation[i, j].maxhealth), 7), healthBar);
							}
						}
					}
				}
			}
			// crafting
			if (!craftBox)
			{
				//craftbox 2x2
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						GUI.Label(new Rect((Screen.width - 80) + j * 40, i * 40, 40, 40), inventoryIcon);
						if (basicCraftLocation[i, j] != null)
						{
							GUI.Label(new Rect((Screen.width - 80) + j * 40, i * 40, 40, 40), basicCraftLocation[i, j].icon);
						}
					}
				}
				//craftresult 2x2
				GUI.Label(new Rect((Screen.width - 80), 80, 80, 80), inventoryIcon);
				if (resultCondition && craftResultLocation != null)
				{
					GUI.DrawTexture(new Rect((Screen.width - 80), 85, 70, 70), craftResultLocation.icon);
				}
			}
			else
			{
				//craftbox 3x3
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						GUI.Label(new Rect((Screen.width - 120) + j * 40, i * 40, 40, 40), inventoryIcon);
						if (boxCraftLocation[i, j] != null)
						{
							GUI.Label(new Rect((Screen.width - 120) + j * 40, i * 40, 40, 40), boxCraftLocation[i, j].icon);
						}
					}
				}
				//craftresult 3x3
				GUI.Label(new Rect((Screen.width - 100), 120, 80, 80), inventoryIcon);
				if (resultCondition && craftResultLocation != null)
				{
					GUI.DrawTexture(new Rect((Screen.width - 100), 125, 70, 70), craftResultLocation.icon);
				}
			}
			
		}
		
		//quickslot gui
		for (int i = 0; i < 9; i++)
		{
			GUI.Label(new Rect(40 * i, (Screen.height) - 40, 40, 40), inventoryIcon);
			if (i == quickslotCurrent)
			{
				GUI.Box(new Rect((40 * i), (Screen.height) - 38, 34, 35), "");
			}
			if (quickslotLocation[i] != null)
			{
				/*if (i == quickslotCurrent)
                {
					GUI.Box(new Rect((40 * i), (Screen.height) - 38, 34, 35), "");
                }*/
				GUI.Label(new Rect(40 * i, (Screen.height) - 40, 40, 40), quickslotLocation[i].icon);
				if (quickslotLocation[i].idItem >= 0 && quickslotLocation[i].idItem < 10)
				{
					if (quickslotCount[i] <= 0)
					{
						quickslotLocation[i] = null;
						quickslotCount[i] = 0;
					}
					else
					{
						GUI.Label(new Rect(40 * i, (Screen.height) - 40, 40, 40), quickslotCount[i].ToString());
					}
				}
				else if (quickslotLocation[i].idItem >= 10 && quickslotLocation[i].idItem < 24)
				{
					if (quickslotLocation[i].health <= 0)
					{
						quickslotLocation[i] = null;
						quickslotCount[i] = 0;
					}
					else
					{
						
						GUI.DrawTexture(new Rect(i * 40, (Screen.height) - 10, 35 * ((float)quickslotLocation[i].health / quickslotLocation[i].maxhealth), 7), healthBar);
					}
				}
				
			}
		}
		
		//icon == mouse
		if (isDragging && showItem)
		{
			GUI.Label(new Rect(Input.mousePosition.x, (Screen.height - Input.mousePosition.y), 40, 40), iconDragged);
		}
		
		//item hover description inventory
		if (!isDragging && showItem)
		{
			if (Input.mousePosition.x < Screen.width && Input.mousePosition.y < Screen.height)
			{
				if (((int)Input.mousePosition.x / 40) < 8 && ((int)(Screen.height - Input.mousePosition.y) / 40) < 5)
				{
					col = (int)Input.mousePosition.x / 40;
					row = (int)(Screen.height - Input.mousePosition.y) / 40;
					if (inventoryLocation[row, col] != null)
					{
						GUI.skin.box.wordWrap = true;
						GUI.Box(
							new Rect(Input.mousePosition.x + 10, (Screen.height - Input.mousePosition.y), 150, 100),
							inventoryLocation[row, col].description);
					}
				}
			}
		}
		//show magic box
		if (magicboxCondition && !isDragging && !showItem)
		{
			//int k = 0;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					GUI.Label(new Rect(j * 40, i * 40, 40, 40), inventoryIcon);

					//if (k < 24)
					if (((i*10)+j)<24)
					{
						//magicBoxLocation[i,j] = database.getItem(k);
						magicBoxLocation[i,j] = database.getItem(((i*10)+j));

						GUI.Label(new Rect(j * 40, i * 40, 40, 40), magicBoxLocation[i,j].icon);
						//if (k < 10 && k >= 0)
						if(((i*10)+j) < 10 && ((i*10)+j) >= 0)
						{
							GUI.Label(new Rect(j * 40, i * 40, 40, 40), "64");
						}
						//else if (k < 24 && k >= 10)
						else if (((i*10)+j) < 24 && ((i*10)+j) >= 10)
						{
							GUI.DrawTexture(new Rect(j * 40, i * 40 + 30, 35, 7), healthBar);
							
						}
					}
					//k++;
				}
			}
		}

		//item hover description magicbox
		if (magicboxCondition && !isDragging && showItem)
		{
			if (Input.mousePosition.x < Screen.width && Input.mousePosition.y < Screen.height)
			{
				if (((int)Input.mousePosition.x / 40) < 10 && ((int)(Screen.height - Input.mousePosition.y) / 40) < 5)
				{
					col = (int)Input.mousePosition.x / 40;
					row = (int)(Screen.height - Input.mousePosition.y) / 40;
					if (magicBoxLocation[row, col] != null)
					{
						GUI.skin.box.wordWrap = true;
						GUI.Box(
							new Rect(Input.mousePosition.x + 10, (Screen.height - Input.mousePosition.y), 150, 100),
							magicBoxLocation[row, col].description);
					}
				}
			}
		}
		//escape button
		if (escapeCondition && screenShootCondition) {
			GUI.Box(new Rect((Screen.width/2)- 100, (Screen.height/2)-50, 200, 100), "Do You Want To Exit ?");
			if(GUI.Button(new Rect((Screen.width/2)+25, (Screen.height/2)+15, 50, 20), "Cancel")){
				escapeCondition= false;
				playercontrol.attack=!playercontrol.attack;
				Time.timeScale = 1.0f;
				playercontrol.GetComponent<MouseLook>().sensitivityX=15;
				mainCameraControl.sensitivityY=10;
			}
			if (GUI.Button(new Rect((Screen.width/2)-75, (Screen.height/2)+15, 50, 20), "Exit")){
				if(SavedLoad.save(inventoryLocation, inventoryCount, quickslotLocation, quickslotCount, basicCraftLocation,basicCraftCount,
				               boxCraftLocation, boxCraftCount,craftResultLocation, craftResultCount)==true){
					screenShootCondition=false;
				Application.CaptureScreenshot(Application.dataPath + @"/SaveFile.png");
			
				Application.LoadLevel("state1");
				}
			}
		}
	}
	
	void swap(Item lamaLocation, int lamaCount, Item baruLocation, int baruCount, int itemSource, int destination)
	{
		/*s1*/
		if (itemSource == 1)
		{
			if (destination == 3)
			{
				if (basicCraftLocation[moverow, movecol] == null)
				{
					inventoryCount[row, col] -= 1;
				}
			}
			else if (destination == 4)
			{
				if (boxCraftLocation[moverow, movecol] == null)
				{
					inventoryCount[row, col] -= 1;
				}
			}
			else
			{
				inventoryLocation[row, col] = baruLocation;
				inventoryCount[row, col] = baruCount;
			}
		}
		/*s2*/		else if (itemSource == 2)
		{
			if (destination == 3)
			{
				Debug.Log("is2 dest3");
				if (basicCraftLocation[moverow, movecol] == null)
				{
					quickslotCount[col] -= 1;
				}
			}
			else if (destination == 4)
			{
				Debug.Log("is2 dest4");
				if (boxCraftLocation[moverow, movecol] == null)
				{
					quickslotCount[col] -= 1;
				}
			}
			else
			{
				Debug.Log("is2 dest");
				quickslotLocation[col] = baruLocation;
				quickslotCount[col] = baruCount;
			}
			
		}
		/*s3*/		else if (itemSource == 3)
		{
			basicCraftLocation[row, col] = baruLocation;
			basicCraftCount[row, col] = baruCount;
		}
		/*s4,6*/	else if (itemSource == 4 || itemSource == 6)
		{
			craftResultLocation = baruLocation;
			craftResultCount = baruCount;
		}
		/*s5*/		else if (itemSource == 5)
		{
			boxCraftLocation[row, col] = baruLocation;
			boxCraftCount[row, col] = baruCount;
		}
		
		/*d1*/
		if (destination == 1)
		{
			if (inventoryLocation[moverow, movecol] != null)
			{
				if (itemSource == 1)
				{
					if (movecol == col && moverow == row)
					{
						inventoryLocation[row, col] = lamaLocation;
						inventoryCount[row, col] = lamaCount;
					}
					else if (lamaLocation == baruLocation && inventoryLocation[row, col].idItem >= 0 && inventoryLocation[row, col].idItem < 10)
					{
						inventoryCount[moverow, movecol] = lamaCount + baruCount;
						inventoryCount[row, col] = 0;
						inventoryLocation[row, col] = null;
					}
					else
					{
						inventoryLocation[moverow, movecol] = lamaLocation;
						inventoryCount[moverow, movecol] = lamaCount;
					}
				}
				if (itemSource == 3)
				{
					if (lamaLocation == baruLocation && basicCraftLocation[row, col].idItem >= 0 && basicCraftLocation[row, col].idItem < 10)
					{
						inventoryCount[moverow, movecol] += 1;
						basicCraftLocation[row, col] = null;
						basicCraftCount[row, col] = 0;
					}
					else
					{
						basicCraftLocation[row, col] = lamaLocation;
						basicCraftCount[row, col] = lamaCount;
						inventoryLocation[moverow, movecol] = baruLocation;
						inventoryCount[moverow, movecol] = baruCount;
					}
				}
				else if (itemSource == 5)
				{
					if (lamaLocation == baruLocation && boxCraftLocation[row, col].idItem >= 0 && boxCraftLocation[row, col].idItem < 10)
					{
						inventoryCount[moverow, movecol] += 1;
						boxCraftLocation[row, col] = null;
						boxCraftCount[row, col] = 0;
					}
					else
					{
						boxCraftLocation[row, col] = lamaLocation;
						boxCraftCount[row, col] = lamaCount;
						inventoryLocation[moverow, movecol] = baruLocation;
						inventoryCount[moverow, movecol] = baruCount;
					}
				}
				
				else if (itemSource == 4 || itemSource == 6)
				{
					if (lamaLocation == baruLocation && craftResultLocation.idItem >= 0 && craftResultLocation.idItem < 10)
					{
						Debug.Log("result ke inventory success ++");
						inventoryCount[moverow, movecol] += 1;
						craftResultLocation = null;
						craftResultCount = 0;
						resultCondition = false;
					}
					else if (inventoryLocation[moverow, movecol] != null)
					{
						Debug.Log("result ke inventory failed");
						craftResultLocation = lamaLocation;
						craftResultCount = lamaCount;
						inventoryLocation[moverow, movecol] = baruLocation;
						inventoryCount[moverow, movecol] = baruCount;
					}
				}
				else if (itemSource == 2)
				{
					if (lamaLocation == baruLocation && quickslotLocation[col].idItem >= 0 && quickslotLocation[col].idItem < 10)
					{
						inventoryCount[moverow, movecol] += 1;
						quickslotLocation[col] = null;
						quickslotCount[col] = 0;
					}
					else
					{
						quickslotLocation[col] = lamaLocation;
						quickslotCount[col] = lamaCount;
						inventoryLocation[moverow, movecol] = baruLocation;
						inventoryCount[moverow, movecol] = baruCount;
					}
				}
				
			}
			else
			{
				inventoryLocation[moverow, movecol] = lamaLocation;
				inventoryCount[moverow, movecol] = lamaCount;
				craftResultLocation = null;
				craftResultCount = 0;
				resultCondition = false;
			}
			
		}
		
		/*d2*/		else if (destination == 2)
		{
			if (quickslotLocation[movecol] != null)
			{
				if (itemSource == 2)
				{
					if (movecol == col && moverow == row)
					{
						Debug.Log("masuk 1");
						quickslotLocation[col] = lamaLocation;
						quickslotCount[col] = lamaCount;
					}
					else if (lamaLocation == baruLocation && quickslotLocation[col].idItem >= 0 && quickslotLocation[col].idItem <= 10)
					{
						Debug.Log("masuk 2");
						quickslotCount[movecol] = lamaCount + baruCount;
						quickslotCount[col] = 0;
						quickslotLocation[col] = null;
					}
					else
					{
						Debug.Log("masuk 3");
						quickslotLocation[movecol] = lamaLocation;
						quickslotCount[movecol] = lamaCount;
					}
				}
				else if (itemSource == 3)
				{
					if (lamaLocation == baruLocation && basicCraftLocation[row, col].idItem >= 0 && basicCraftLocation[row, col].idItem < 10)
					{
						quickslotCount[movecol] += 1;
						basicCraftLocation[row, col] = null;
						basicCraftCount[row, col] = 0;
					}
					else
					{
						basicCraftLocation[row, col] = lamaLocation;
						basicCraftCount[row, col] = lamaCount;
						quickslotLocation[movecol] = baruLocation;
						quickslotCount[movecol] = baruCount;
					}
				}
				else if (itemSource == 5)
				{
					if (lamaLocation == baruLocation && boxCraftLocation[row, col].idItem >= 0 && boxCraftLocation[row, col].idItem < 10)
					{
						quickslotCount[movecol] += 1;
						boxCraftLocation[row, col] = null;
						boxCraftCount[row, col] = 0;
					}
					else
					{
						boxCraftLocation[row, col] = lamaLocation;
						boxCraftCount[row, col] = lamaCount;
						quickslotLocation[movecol] = baruLocation;
						quickslotCount[movecol] = baruCount;
					}
				}
				else if (itemSource == 4 || itemSource == 6)
				{
					if (lamaLocation == baruLocation && craftResultLocation.idItem >= 0 && craftResultLocation.idItem < 10)
					{
						quickslotCount[movecol] += 1;
						craftResultLocation = null;
						craftResultCount = 0;
						resultCondition = false;
					}
					else
					{
						craftResultLocation = lamaLocation;
						craftResultCount = lamaCount;
						quickslotLocation[movecol] = baruLocation;
						quickslotCount[movecol] = baruCount;
					}
				}
				else if (itemSource == 1)
				{
					if (lamaLocation == baruLocation && inventoryLocation[row, col].idItem >= 0 && inventoryLocation[row, col].idItem < 10)
					{
						quickslotCount[movecol] += 1;
						inventoryLocation[row, col] = null;
						inventoryCount[row, col] = 0;
					}
					else
					{
						inventoryLocation[row, col] = lamaLocation;
						inventoryCount[row, col] = lamaCount;
						quickslotLocation[movecol] = baruLocation;
						quickslotCount[movecol] = baruCount;
					}
				}
				else
				{
					quickslotLocation[movecol] = lamaLocation;
					quickslotCount[movecol] = lamaCount;
				}
			}
			else
			{
				quickslotLocation[movecol] = lamaLocation;
				quickslotCount[movecol] = lamaCount;
				craftResultLocation = null;
				craftResultCount = 0;
				resultCondition = false;
			}
			
		}
		/*d3*/		else if (destination == 3)
		{
			if (basicCraftLocation[moverow, movecol] != null)
			{
				Debug.Log("masuk sini");
				if (itemSource == 3)
				{
					if (movecol == col && moverow == row)
					{
						basicCraftLocation[row, col] = lamaLocation;
						basicCraftCount[row, col] = lamaCount;
					}
					else
					{
						Debug.Log("masuk ke barter");
						basicCraftLocation[moverow, movecol] = lamaLocation;
						basicCraftCount[moverow, movecol] = lamaCount;
					}
				}
				else if (itemSource == 4 || itemSource == 6)
				{
					craftResultLocation = lamaLocation;
					craftResultCount = lamaCount;
					basicCraftLocation[moverow, movecol] = baruLocation;
					basicCraftCount[moverow, movecol] = baruCount;
					
				}
				else if (itemSource == 2)
				{
					quickslotLocation[col] = lamaLocation;
					quickslotCount[col] = lamaCount;
					basicCraftLocation[moverow, movecol] = baruLocation;
					basicCraftCount[moverow, movecol] = baruCount;
					
				}
			}
			else
			{
				if (itemSource == 2)
				{
					quickslotLocation[col] = lamaLocation;
					quickslotCount[col] = lamaCount;
					basicCraftLocation[moverow, movecol] = baruLocation;
					basicCraftCount[moverow, movecol] = baruCount;
					
				}
				else if (itemSource == 1)
				{
					basicCraftLocation[moverow, movecol] = lamaLocation;
					basicCraftCount[moverow, movecol] = 1;
				}
				else if (itemSource == 3)
				{
					Debug.Log("masuk ke barter");
					basicCraftLocation[moverow, movecol] = lamaLocation;
					basicCraftCount[moverow, movecol] = lamaCount;
				}
			}
			
		}
		/*d4*/		else if (destination == 4)
		{
			if (boxCraftLocation[moverow, movecol] != null)
			{
				if (itemSource == 5)
				{
					if (movecol == col && moverow == row)
					{
						boxCraftLocation[row, col] = lamaLocation;
						boxCraftCount[row, col] = lamaCount;
					}
					else
					{
						boxCraftLocation[moverow, movecol] = lamaLocation;
						boxCraftCount[moverow, movecol] = lamaCount;
					}
				}
				else if (itemSource == 4 || itemSource == 6)
				{
					craftResultLocation = lamaLocation;
					craftResultCount = lamaCount;
					boxCraftLocation[moverow, movecol] = baruLocation;
					boxCraftCount[moverow, movecol] = baruCount;
					
				}
				else if (itemSource == 2)
				{
					quickslotLocation[col] = lamaLocation;
					quickslotCount[col] = lamaCount;
					boxCraftLocation[moverow, movecol] = baruLocation;
					boxCraftCount[moverow, movecol] = baruCount;
					
				}
			}
			else
			{
				if (itemSource == 2)
				{
					quickslotLocation[col] = lamaLocation;
					quickslotCount[col] = lamaCount;
					boxCraftLocation[moverow, movecol] = baruLocation;
					boxCraftCount[moverow, movecol] = baruCount;
					
				}
				else if (itemSource == 1)
				{
					boxCraftLocation[moverow, movecol] = lamaLocation;
					boxCraftCount[moverow, movecol] = 1;
				}
				else if (itemSource == 5)
				{
					boxCraftLocation[moverow, movecol] = lamaLocation;
					boxCraftCount[moverow, movecol] = lamaCount;
				}
			}
			
		}
		if (itemSource == 1)
		{
			if (inventoryCount[row, col] == 0)
			{
				inventoryLocation[row, col] = null;
			}
		}
		else if (itemSource == 2)
		{
			if (quickslotCount[col] == 0)
			{
				quickslotLocation[col] = null;
			}
		}
		
		iconDragged = null;
		itemSource = 0;
		lamaLocation = null;
		lamaCount = 0;
		baruLocation = null;
		baruCount = 0;
		
	}
	
	void instantiateObject(int quickslotCurrent, Vector3 posvect3, Vector3 hit)
	{
		Vector3 newpos = new Vector3(posvect3.x + hit.x, posvect3.y + hit.y, posvect3.z + hit.z);
		Debug.Log(posvect3);
		Debug.Log(hit);
		Debug.Log(newpos);
		if (quickslotLocation[quickslotCurrent] != null)
		{
			if (quickslotLocation[quickslotCurrent].idItem == 0)
			{
				Instantiate((GameObject)Resources.Load("Prefab/normGrass"), newpos, Quaternion.identity);
				quickslotCount[quickslotCurrent] -= 1;
			}
			else if (quickslotLocation[quickslotCurrent].idItem == 1)
			{
				Instantiate((GameObject)Resources.Load("Prefab/normCobblestone"), newpos, Quaternion.identity);
				quickslotCount[quickslotCurrent] -= 1;
			}
			else if (quickslotLocation[quickslotCurrent].idItem == 2)
			{
				Instantiate((GameObject)Resources.Load("Prefab/normWood"), newpos, Quaternion.identity);
				quickslotCount[quickslotCurrent] -= 1;
			}
			else if (quickslotLocation[quickslotCurrent].idItem == 3)
			{
				Instantiate((GameObject)Resources.Load("Prefab/normPlank"), newpos, Quaternion.identity);
				quickslotCount[quickslotCurrent] -= 1;
			}
			else if (quickslotLocation[quickslotCurrent].idItem == 4)
			{
				Instantiate((GameObject)Resources.Load("Prefab/normCraftingTable"), newpos, Quaternion.identity);
				quickslotCount[quickslotCurrent] -= 1;
			}
		}
	}
	
	public int instantDestroy()
	{
		
		if (quickslotLocation[quickslotCurrent] != null)
		{
			if (quickslotLocation[quickslotCurrent].idItem >= 10 && quickslotLocation[quickslotCurrent].idItem < 24)
			{
				if (quickslotLocation[quickslotCurrent].health > 0)
				{
					return quickslotLocation[quickslotCurrent].idItem;
				}
				else
				{
					return 25;
				}
			}
			else
			{
				return 25;
			}
		}
		else
		{
			return 25;
		}
	}
	public void healthminus()
	{
		if (quickslotLocation[quickslotCurrent] != null)
		{
			if (quickslotLocation[quickslotCurrent].idItem >= 10 && quickslotLocation[quickslotCurrent].idItem < 24)
			{
				if (quickslotLocation[quickslotCurrent].health - 10 <= 0)
				{
					quickslotLocation[quickslotCurrent] = null;
					quickslotCount[quickslotCurrent] = 0;
				}
				else
				{
					quickslotLocation[quickslotCurrent].healthminus();
				}
			}
		}
	}
	
	
	void craftboxFn()
	{
		if (quickslotLocation[quickslotCurrent] != null)
		{
			if (quickslotLocation[quickslotCurrent].idItem >= 0 && quickslotLocation[quickslotCurrent].idItem < 10)
			{
				Debug.Log("gagal buka craftbox");
				return;
			}
			else
			{
				Debug.Log("hit crafting table !null");
				playercontrol.attack = !playercontrol.attack;
				craftBox = true;
				showCraftBoxFn();
				Debug.Log("berhasil buka craftbox !null");
			}
		}
		else
		{
			Debug.Log("hit crafting table null");
			playercontrol.attack = !playercontrol.attack;
			craftBox = true;
			showCraftBoxFn();
			Debug.Log("berhasil buka craftbox null");
		}
	}
	
	void showCraftBoxFn()
	{
		playercontrol.attack = false;
		showItem = true;
		Screen.lockCursor = false;
		Screen.showCursor = true;
		Time.timeScale = 0.0f;
		playercontrol.GetComponent<MouseLook>().sensitivityX=0;
		mainCameraControl.sensitivityY=0;
	}
	
	void showMagicboxFn()
	{
		Time.timeScale = 0.0f;
		playercontrol.attack = false;
		magicboxCondition = true;
		Screen.lockCursor = false;
		Screen.showCursor = true;
		playercontrol.GetComponent<MouseLook>().sensitivityX=0;
		mainCameraControl.sensitivityY=0;
	}
}