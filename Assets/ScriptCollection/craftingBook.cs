using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class craftingBook : MonoBehaviour
{
    private ItemDatabase database;
	string tempa, tempb, tempc, tempd;
	string tempaa, tempab, tempac, tempba, tempbb, tempbc, tempca, tempcb, tempcc;
    void Start()
    {
        database = GameObject.FindGameObjectWithTag("database").GetComponent<ItemDatabase>();
    }

    public Item recipe2x2(int a, int b, int c, int d)
    {

        Debug.Log("masuk recipe2 2x2");
        string recipe;
		int itemCount = count2x2(a,b,c,d);
		recipe = tempa+tempb+tempc+tempd;
        
		Debug.Log ("ItemCount >> " + itemCount);
		Debug.Log ("string Recipe >> " + recipe);
        if (itemCount == 1)
        {
            if (recipe.Contains("2"))
            {
                //return woodplank
                return database.getItem(3);
            }
			else{
				return null;
			}

        }
        else if (itemCount == 2)
        {
            if (recipe.Contains("33") || recipe.Contains("3B3"))
            {
                //return stick
                return database.getItem(9);
            }
			else{
				return null;
			}

        }
        /*else if (itemCount == 3)
        {


        }*/
        else if (itemCount == 4)
        {
            if (recipe.Contains("3333"))
            {
                //return crafting table
                return database.getItem(4);
			}			else{
				return null;
			}

		}		
		else{
			return null;
		}



    }

	public Item recipe3x3(int aa, int ab, int ac, int ba, int bb, int bc, int ca, int cb, int cc)
    {
        Debug.Log("masuk recipe2 3x3");
        string recipe;
		int itemCount = count3x3(aa,ab,ac,ba,bb,bc,ca,cb,cc);
		recipe = tempaa + tempab + tempac + tempba + tempbb + tempbc + tempca + tempcb + tempcc;
        
		Debug.Log ("ItemCount >> " + itemCount);
		Debug.Log ("string Recipe >> " + recipe);
        if (itemCount == 1)
        {
			 if (recipe.Contains("7"))
			{
				//return gold nugget
				return database.getItem(8);
			}
			else if (recipe.Contains("2"))
            {
                //return woodplank
                return database.getItem(3);
            }
			else{
				return null;
			}

        }
        else if (itemCount == 2)
        {
            if (recipe.Contains("33") || recipe.Contains("3BB3"))
            {
                //return stick
                return database.getItem(9);
            }
			else{
				return null;
			}

        }
        else if (itemCount == 3)
        {
			if (recipe.Contains("3BB9BB9"))
			{
				//return Wooden Shovel
				return database.getItem(20);
			}
			else if (recipe.Contains("1BB9BB9"))
			{
				//return Stone Shovel
				return database.getItem(21);
			}
			else if (recipe.Contains("7BB9BB9"))
			{
				//return Gold Shovel
				return database.getItem(22);
			}
			else if (recipe.Contains("6BB9BB9"))
			{
				//return Diamond Shovel
				return database.getItem(23);
			}
			else{
				return null;
			}		
        }
        else if (itemCount == 4)
        {
            if (recipe.Contains("33B33"))
            {
                //return crafting table
                return database.getItem(4);
            }
			else{
				return null;
			}

        }
        else if (itemCount == 5)
        {
            if (recipe.Contains("33B39BB9"))
            {
                //return Wooden axe
                return database.getItem(12);
            }
            else if (recipe.Contains("11B19BB9"))
            {
                //return Stone axe
                return database.getItem(13);
            }
            else if (recipe.Contains("77B79BB9"))
            {
                //return Gold axe
                return database.getItem(14);
            }
            else if (recipe.Contains("66B69BB9"))
            {
                //return Diamond axe
                return database.getItem(15);
            }


            if (recipe.Contains("333B9BB9"))
            {
                //return Wooden pickaxe
                return database.getItem(16);
            }
            else if (recipe.Contains("111B9BB9"))
            {
                //return Stone pickaxe
                return database.getItem(17);
            }
            else if (recipe.Contains("777B9BB9"))
            {
                //return Gold pickaxe
                return database.getItem(18);
            }
            else if (recipe.Contains("666B9BB9"))
            {
                //return Diamond pickaxe
                return database.getItem(19);
            }
			else{
				return null;
			}


           
        }
        /*else if (itemCount == 6)
        {

        }
        else if (itemCount == 7)
        {

        }
        else if (itemCount == 8)
        {

        }*/
        else if (itemCount == 9)
        {
            //return gold ingot
            if (recipe.Contains("888888888"))
            {
                return database.getItem(7);
            }
			else{
				return null;
			}
        }
		else{
			return null;
		}

    }

	int count3x3(int aa, int ab, int ac, int ba, int bb, int bc, int ca, int cb, int cc)
    {
        int i = 0;
		if (aa != 25) { i++;tempaa=aa.ToString(); }else { tempaa = "B";	}
		if (ab != 25) { i++;tempab=ab.ToString(); }else { tempab = "B";	}
		if (ac != 25) { i++;tempac=ac.ToString(); }else { tempac = "B";	}
		if (ba != 25) { i++;tempba=ba.ToString(); }else { tempba = "B";	}
		if (bb != 25) { i++;tempbb=bb.ToString(); }else { tempbb = "B";	}
		if (bc != 25) { i++;tempbc=bc.ToString(); }else { tempbc = "B";	}
		if (ca != 25) { i++;tempca=ca.ToString(); }else { tempca = "B";	}
		if (cb != 25) { i++;tempcb=cb.ToString(); }else { tempcb = "B";	}
		if (cc != 25) { i++;tempcc=cc.ToString(); }else { tempcc = "B";	}
        return i;
    }
	int count2x2(int a, int b, int c, int d)
    {
        int i = 0;
		if (a != 25) {	i++; tempa = a.ToString();} else { tempa = "B";}
		if (b != 25) { 	i++; tempb = b.ToString();} else { tempb = "B";}
		if (c != 25) { 	i++; tempc = c.ToString();} else { tempc = "B";}
		if (d != 25) { 	i++; tempd = d.ToString();} else { tempd = "B";}
        return i;
    }

}
