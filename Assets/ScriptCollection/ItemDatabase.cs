using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    private List<Item> item = new List<Item>();

    void Start()
    {
        /*0*/
        item.Add(new Item(0, "Dirt", "Dirt is a block found abundantly in the Overworld.", "Dirt", 0, 0));
        /*1*/
        item.Add(new Item(1, "Cobblestone", "Cobblestone is a common block, obtained from mining stone. Its texture resembles an uneven, roughly paved surface. Cobblestone is mainly used for crafting or as a common building block.", "Cobblestone", 0, 0));
        /*2*/
        item.Add(new Item(2, "Wood", "Wood, also known as logs, is a naturally occurring block found in trees, primarily used to create wood planks. It comes in six varieties: oak, birch, spruce, jungle, dark oak, and acacia", "Wood", 0, 0));
        /*3*/
        item.Add(new Item(3, "Wooden Plank", "Wood Planks is a common block used in many crafting recipes. Its texture resembles a network of planks, coming in six different shades obtained from the six different tree varieties.", "Wooden Plank", 0, 0)); //woodenplank return 4
        /*4*/
        item.Add(new Item(4, "Crafting Table", "The Crafting Table (originally called Workbench) is one of the most essential blocks in Minecraft", "Crafting Table", 0, 0)); //craftingtable return 1

        /*5*/
        item.Add(new Item(5, "Furnace", "Furnace is a block used to smelt blocks and items and convert them into other blocks or items.", "Furnace", 0, 0));
        /*6*/
        item.Add(new Item(6, "Diamond Gem", "Diamonds are one of the rarest materials in Minecraft.", "Diamond Gem", 0, 0));
        /*7*/
        item.Add(new Item(7, "Gold Ingot", "Gold ingots are a metal which is used to craft the second tier of armor, and the first of tools.", "Gold Ingot", 0, 0)); // goldingot return 9
        /*8*/
        item.Add(new Item(8, "Gold Nugget", "A Gold Nugget is an item obtained by killing zombie pigmen.", "Gold Nugget", 0, 0)); // goldnugget return 1

        /*9*/
        item.Add(new Item(9, "Stick", "A stick is an item used for crafting many tools and items.", "Stick", 0, 0)); // stick return 4
        /*10*/
        item.Add(new Item(10, "Gold Sword", "ini deskripsi bukan nama Sword Description", "Gold Sword", 150, 150));
        /*11*/
        item.Add(new Item(11, "Gold Hoe", "ini deskripsi bukan nama Hoe Description", "Gold Hoe", 150, 150));

        /*12*/
        item.Add(new Item(12, "Axe", "Wooden Axes are tools used to ease the process of collecting wood based items.", "Axe", 50, 50));
        /*13*/
        item.Add(new Item(13, "Stone Axe", "Stone Axes are tools used to ease the process of collecting wood based items.", "Stone Axe", 100, 100));
        /*14*/
        item.Add(new Item(14, "Gold Axe", "Gold Axes are tools used to ease the process of collecting wood based items.", "Gold Axe", 150, 150));
        /*15*/
        item.Add(new Item(15, "Diamond Axe", "Diamond Axes are tools used to ease the process of collecting wood based items.", "Diamond Axe", 200, 200));

        /*16*/
        item.Add(new Item(16, "Pickaxe", "Wooden Pickaxes are one of the most commonly used tools in the game, being required to mine all ores and many other types of blocks.", "Pickaxe", 50, 50));
        /*17*/
        item.Add(new Item(17, "Stone Pickaxe", "Stone Pickaxes are one of the most commonly used tools in the game, being required to mine all ores and many other types of blocks.", "Stone Pickaxe", 100, 100));
        /*18*/
        item.Add(new Item(18, "Gold Pickaxe", "Golden Pickaxes are one of the most commonly used tools in the game, being required to mine all ores and many other types of blocks.", "Gold Pickaxe", 150, 150));
        /*19*/
        item.Add(new Item(19, "Diamond Pickaxe", "Diamond Pickaxes are one of the most commonly used tools in the game, being required to mine all ores and many other types of blocks.", "Diamond Pickaxe", 200, 200));

        /*20*/
        item.Add(new Item(20, "Shovel", "Wooden Shovels are tools used to ease the process of collecting dirt and other blocks.", "Shovel", 50, 50));
        /*21*/
        item.Add(new Item(21, "Stone Shovel", "Stone Shovels are tools used to ease the process of collecting dirt and other blocks.", "Stone Shovel", 100, 100));
        /*22*/
        item.Add(new Item(22, "Gold Shovel", "Gold Shovels are tools used to ease the process of collecting dirt and other blocks.", "Gold Shovel", 150, 150));
        /*23*/
        item.Add(new Item(23, "Diamond Shovel", "Diamond Shovels are tools used to ease the process of collecting dirt and other blocks.", "Diamond Shovel", 200, 200));

    }

    public Item getItem(int id)
    {
        return item[id];
    }
}
