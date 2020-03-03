using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item
{

    public int idItem;
    public string name;
    public string description;
    public Texture2D icon;
    public int health;
    public int maxhealth;

    public Item(int id, string nama, string deskripsi, string ikon, int health, int maxhealth)
    {
        this.idItem = id;
        this.name = nama;
        this.description = deskripsi;
        this.icon = (Texture2D)Resources.Load("Image/Icons/" + ikon);
        this.health = health;
        this.maxhealth = maxhealth;
    }

    public int healthminus()
    {
        return health -= 10;
    }

}