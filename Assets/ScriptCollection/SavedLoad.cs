using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

public class SavedLoad
{



    public static bool save(Item[,] inventoryLocation, int[,] inventoryCount,
                            Item[] quickslotLocation, int[] quickslotCount,
                            Item[,] basicCraftLocation, int[,] basicCraftCount,
                            Item[,] boxCraftLocation, int[,] boxCraftCount,
                            Item craftResultLocation, int craftResultCount)
    {


        XmlWriterSettings setting = new XmlWriterSettings();
        setting.Indent = true;
        setting.OmitXmlDeclaration = true;
        setting.NewLineOnAttributes = true;
        XmlWriter writeFile = XmlWriter.Create(Application.dataPath + @"/SaveFile.xml", setting);

        writeFile.WriteStartDocument();
        writeFile.WriteStartElement("SavedData");
        writeFile.WriteStartElement("Inventory");
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (inventoryLocation[i, j] == null)
                {
                    inventoryLocation[i, j] = new Item(25, "", "", "", 0, 0);
                    inventoryCount[i, j] = 0;
                }
                writeFile.WriteStartElement("InventoryInfo");
                writeFile.WriteElementString("InventoryItemID", inventoryLocation[i, j].idItem.ToString());
                writeFile.WriteElementString("InventoryHealth", inventoryLocation[i, j].health.ToString());
                writeFile.WriteElementString("InventoryQuantity", inventoryCount[i, j].ToString());
                writeFile.WriteEndElement();//inventory info
            }
        }
        writeFile.WriteEndElement();//inventory

        writeFile.WriteStartElement("QuickSlot");
        for (int i = 0; i < 9; i++)
        {
            if (quickslotLocation[i] == null)
            {
                quickslotLocation[i] = new Item(25, "", "", "", 0, 0);
                quickslotCount[i] = 0;
            }
            writeFile.WriteStartElement("QuickSlotInfo");
            writeFile.WriteElementString("QuickSlotID", quickslotLocation[i].idItem.ToString());
            writeFile.WriteElementString("QuickSlotHealth", quickslotLocation[i].health.ToString());
            writeFile.WriteElementString("QuickSlotQuantity", quickslotCount[i].ToString());
            writeFile.WriteEndElement();//quickslot info
        }
        writeFile.WriteEndElement();//quickslot

        writeFile.WriteStartElement("basicCraft");
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (basicCraftLocation[i, j] == null)
                {
                    basicCraftLocation[i, j] = new Item(25, "", "", "", 0, 0);
                    basicCraftCount[i, j] = 0;
                }
                writeFile.WriteStartElement("basicCraftInfo");
                writeFile.WriteElementString("basicCraftItemID", basicCraftLocation[i, j].idItem.ToString());
				writeFile.WriteElementString("basicCraftHealth", basicCraftLocation[i, j].health.ToString());
                writeFile.WriteElementString("basicCraftQuantity", basicCraftCount[i, j].ToString());
                writeFile.WriteEndElement();//basiccrafft info

            }
        }
        writeFile.WriteEndElement();//basiccraft 2x2

        writeFile.WriteStartElement("boxCraft");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (boxCraftLocation[i, j] == null)
                {
                    boxCraftLocation[i, j] = new Item(25, "", "", "", 0, 0);
                    boxCraftCount[i, j] = 0;
                }
                writeFile.WriteStartElement("boxCraftInfo");
                writeFile.WriteElementString("boxCraftItemID", boxCraftLocation[i, j].idItem.ToString());
                writeFile.WriteElementString("boxCraftHealth", boxCraftLocation[i, j].health.ToString());
                writeFile.WriteElementString("boxCraftQuantity", boxCraftCount[i, j].ToString());
                writeFile.WriteEndElement();//boxcraft info

            }
        }
        writeFile.WriteEndElement();//boxcraft 3x3

        writeFile.WriteStartElement("CraftResult");
        writeFile.WriteStartElement("ResultItem");
        if (craftResultLocation == null)
        {
            writeFile.WriteElementString("ResultID", "25");
            writeFile.WriteElementString("ResultHealth", "0");
            writeFile.WriteElementString("ResultQuantity", "0");
        }
        else
        {
            writeFile.WriteElementString("ResultID", craftResultLocation.idItem.ToString());
            writeFile.WriteElementString("ResultHealth", craftResultLocation.health.ToString());
            writeFile.WriteElementString("ResultQuantity", craftResultCount.ToString());
        }
        writeFile.WriteEndElement();//result info
        writeFile.WriteEndElement();//craftresult


        writeFile.WriteStartElement("Player");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        writeFile.WriteStartElement("PlayerLocation");
        writeFile.WriteElementString("PlayerX", player.transform.position.x.ToString());
        writeFile.WriteElementString("PlayerY", player.transform.position.y.ToString());
        writeFile.WriteElementString("PlayerZ", player.transform.position.z.ToString());
        writeFile.WriteEndElement();//Player info
        writeFile.WriteEndElement();//Player

        writeFile.WriteStartElement("Cube");//start cube
        foreach (GameObject cube in GameObject.FindGameObjectsWithTag("norm"))
        {
            writeFile.WriteStartElement("CubeLocation");// start cubeinfo
            writeFile.WriteElementString("CubeName", cube.name);
            writeFile.WriteElementString("CubeX", cube.transform.position.x.ToString());
            writeFile.WriteElementString("CubeY", cube.transform.position.y.ToString());
            writeFile.WriteElementString("CubeZ", cube.transform.position.z.ToString());
            writeFile.WriteEndElement();//Cube Info
        }
        writeFile.WriteEndElement();//Cube
        writeFile.WriteEndElement();//saveddata
        writeFile.WriteEndDocument();
		writeFile.Close ();
		Debug.Log("sampai sini");
        return true;
    }

    public static Item[,] LoadInventoryLocation()
    {
        ItemDatabase database;
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
        Item[,] inventoryLocation = new Item[5, 8];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, idInventoryItem, healthInventoryItem;
        while (readFile.Read())
        {
            if (j == 8 && i < 5) { i++; j = 0; }
            else if (i == 4 && j == 7) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "InventoryItemID": if (int.TryParse(readFile.ReadString().ToString(), out idInventoryItem))
                        {
                            if (idInventoryItem < 24) { inventoryLocation[i, j] = database.getItem(idInventoryItem); }
                            else { inventoryLocation[i, j] = null; }
                        } break;
                    case "InventoryHealth": if (inventoryLocation[i, j] != null && int.TryParse(readFile.ReadString().ToString(), out healthInventoryItem))
                        {
                            inventoryLocation[i, j].health = healthInventoryItem; j++;
                        } break;
                }
            }
        }
        readFile.Close();
        return inventoryLocation;
    }
    public static int[,] LoadInventoryCount()
    {
        int[,] inventoryCount = new int[5, 8];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, countInventoryItem;
        while (readFile.Read())
        {
            if (j == 8 && i < 5) { i++; j = 0; }
            else if (i == 4 && j == 8) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "InventoryQuantity": if (int.TryParse(readFile.ReadString().ToString(), out countInventoryItem))
                        {
                            inventoryCount[i, j] = countInventoryItem; j++;
                        }; break;
                }
            }
        }
        readFile.Close();
        return inventoryCount;
    }

    public static Item[] LoadQuickslotLocation()
    {
        ItemDatabase database;
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
        Item[] quickslotLocation = new Item[9];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, idQuickslotItem, healthQuickslotItem;
        while (readFile.Read())
        {
            if (i > 8) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "QuickSlotID": if (int.TryParse(readFile.ReadString().ToString(), out idQuickslotItem))
                        {
                            if (idQuickslotItem < 24) { quickslotLocation[i] = database.getItem(idQuickslotItem); }
                            else { quickslotLocation[i] = null; }
                        } break;
                    case "QuickSlotHealth": if (quickslotLocation[i] != null && int.TryParse(readFile.ReadString().ToString(), out healthQuickslotItem))
                        {
                            quickslotLocation[i].health = healthQuickslotItem; i++;
                        } break;
                }
            }
        }
        readFile.Close();
        return quickslotLocation;
    }

    public static int[] LoadQuickslotCount()
    {
        int[] quickslotCount = new int[9];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, countQuickslotItem;
        while (readFile.Read())
        {
            if (i > 8) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "QuickSlotQuantity": if (int.TryParse(readFile.ReadString().ToString(), out countQuickslotItem))
                        {
                            quickslotCount[i] = countQuickslotItem; i++;
                        }; break;
                }
            }
        }
        readFile.Close();
        return quickslotCount;
    }

    public static Item[,] LoadBasicCraftLocation()
    {
        ItemDatabase database;
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
        Item[,] basicCraftLocation = new Item[2, 2];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, idBasicCraftItem, healthBasicCraftItem;
        while (readFile.Read())
        {
            if (j == 2 && i < 2) { i++; j = 0; }
            else if (i == 1 && j == 2) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "basicCraftItemID": if (int.TryParse(readFile.ReadString().ToString(), out idBasicCraftItem))
                        {
                            if (idBasicCraftItem < 24) { basicCraftLocation[i, j] = database.getItem(idBasicCraftItem); }
                            else { basicCraftLocation[i, j] = null; }
                        } break;
                    case "basicCraftHealth": if (basicCraftLocation[i, j] != null && int.TryParse(readFile.ReadString().ToString(), out healthBasicCraftItem))
                        {
                            basicCraftLocation[i, j].health = healthBasicCraftItem; j++;
                        } break;
                }
            }
        }
        readFile.Close();
        return basicCraftLocation;
    }
    public static int[,] LoadBasicCraftCount()
    {
        int[,] basicCraftCount = new int[2, 2];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, countBasicCraftItem;
        while (readFile.Read())
        {
            if (j == 2 && i < 2) { i++; j = 0; }
            else if (i == 1 && j == 2) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "basicCraftQuantity": if (int.TryParse(readFile.ReadString().ToString(), out countBasicCraftItem))
                        {
                            basicCraftCount[i, j] = countBasicCraftItem; j++;
                        }; break;
                }
            }
        }
        readFile.Close();
        return basicCraftCount;
    }

    public static Item[,] LoadBoxCraftLocation()
    {
        ItemDatabase database;
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
        Item[,] boxCraftLocation = new Item[3, 3];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, idBoxCraftItem, healthBoxCraftItem;
        while (readFile.Read())
        {
            if (j == 3 && i < 3) { i++; j = 0; }
            else if (i == 2 && j == 3) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "boxCraftItemID": if (int.TryParse(readFile.ReadString().ToString(), out idBoxCraftItem))
                        {
                            if (idBoxCraftItem < 24) { boxCraftLocation[i, j] = database.getItem(idBoxCraftItem); }
                            else { boxCraftLocation[i, j] = null; }
                        } break;
                    case "boxCraftHealth": if (boxCraftLocation[i, j] != null && int.TryParse(readFile.ReadString().ToString(), out healthBoxCraftItem))
                        {
                            boxCraftLocation[i, j].health = healthBoxCraftItem; j++;
                        } break;
                }
            }
        }
        readFile.Close();
        return boxCraftLocation;
    }

    public static int[,] LoadBoxCraftCount()
    {
        int[,] boxCraftCount = new int[3, 3];
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, j = 0, countBoxCraftItem;
        while (readFile.Read())
        {
            if (j == 3 && i < 3) { i++; j = 0; }
            else if (i == 2 && j == 3) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "boxCraftQuantity": if (int.TryParse(readFile.ReadString().ToString(), out countBoxCraftItem))
                        {
                            boxCraftCount[i, j] = countBoxCraftItem; j++;
                        }; break;
                }
            }
        }
        readFile.Close();
        return boxCraftCount;
    }

    public static Item LoadResultLocation()
    {
        ItemDatabase database;
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
        Item craftResultLocation = null;
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, idResultItem, healthResultItem;
        while (readFile.Read())
        {
            if (i > 0) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "ResultID": if (int.TryParse(readFile.ReadString().ToString(), out idResultItem))
                        {
                            if (idResultItem < 24) { craftResultLocation = database.getItem(idResultItem); }
                            else { craftResultLocation = null; }
                        } break;
                    case "ResultHealth": if (craftResultLocation != null && int.TryParse(readFile.ReadString().ToString(), out healthResultItem))
                        {
                            craftResultLocation.health = healthResultItem; i++;
                        } break;
                }
            }
        }
        readFile.Close();
        return craftResultLocation;
    }

    public static int LoadResultCount()
    {
        int craftResultCount = 0;
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0, countResultItem;
        while (readFile.Read())
        {
            if (i > 0) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name.ToString())
                {
                    case "ResultQuantity": if (int.TryParse(readFile.ReadString().ToString(), out countResultItem))
                        {
                            craftResultCount = countResultItem; i++;
                        }; break;
                }
            }
        }
        readFile.Close();
        return craftResultCount;
    }

    public static Vector3 LoadPlayerLocation()
    {
        float x = 0, y = 0, z = 0;
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        int i = 0;
        while (readFile.Read())
        {
            if (i > 0) { break; }
            if (readFile.IsStartElement())
            {
                switch (readFile.Name)
                {
                    case "PlayerX": float.TryParse(readFile.ReadString().ToString(), out x); break;
                    case "PlayerY": float.TryParse(readFile.ReadString().ToString(), out y); break;
                    case "PlayerZ": float.TryParse(readFile.ReadString().ToString(), out z); i++; break;
                }
            }
        }
        readFile.Close();
        return new Vector3(x, y, z);
    }

    public static void loadMap()
    {
        string cube = "";
        float x = 0, y = 0, z = 0;
        XmlReader readFile = XmlReader.Create(Application.dataPath + @"/SaveFile.xml");
        while (readFile.Read())
        {
            if (readFile.IsStartElement())
            {
                switch (readFile.Name)
                {
                    case "CubeName": cube = readFile.ReadString().ToString().Replace("(Clone)", ""); break;
                    case "CubeX": float.TryParse(readFile.ReadString().ToString(), out x); break;
                    case "CubeY": float.TryParse(readFile.ReadString().ToString(), out y); break;
                    case "CubeZ": if (float.TryParse(readFile.ReadString().ToString(), out z))
                        {
                            if (cube.Equals("normCobbleStone"))
                            {
                                GameObject.Instantiate((GameObject)Resources.Load("Prefab/normCobbleStone"), new Vector3(x, y, z), Quaternion.identity);
                            }
                            else if (cube.Equals("normGrass"))
                            {
                                GameObject.Instantiate((GameObject)Resources.Load("Prefab/normGrass"), new Vector3(x, y, z), Quaternion.identity);
                            }
                            else if (cube.Equals("normWood"))
                            {
                                GameObject.Instantiate((GameObject)Resources.Load("Prefab/normWood"), new Vector3(x, y, z), Quaternion.identity);
                            }
                            else if (cube.Equals("normPlank"))
                            {
                                GameObject.Instantiate((GameObject)Resources.Load("Prefab/normPlank"), new Vector3(x, y, z), Quaternion.identity);
                            }
                            else if (cube.Equals("normCraftingTable"))
                            {
                                GameObject.Instantiate((GameObject)Resources.Load("Prefab/normCraftingTable"), new Vector3(x, y, z), Quaternion.identity);
                            }
                        } break;
                }
            }
        }
        readFile.Close();
    }
}
