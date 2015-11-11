using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TerrainGeneration : MonoBehaviour {


    public int mazeW = 5;
    public int mazeH = 5;
    public GameObject[] tiles;
    public GameObject Nexus;
    public Rigidbody player;
    public Rigidbody enemy;
    public int tileOffset = 20;


    public List<mycell> pathOrder = new List<mycell>();

	// Use this for initialization
	void Start () {
        mycell[,] path = new mycell[mazeH, mazeW];
        int currX = (int)Random.Range(0, mazeH - 1);
        int currY = (int)Random.Range(0, mazeH - 1);

        for (int i = 0; i < mazeH; i++)
        {
            for (int j = 0; j < mazeW; j++)
            {
                path[i, j] = new mycell();
            }
        }

	    try
	    {
            path[currX, currY].tile = "start";
            path[currX, currY].traversed = true;
            path[currX, currY].x = currX;
            path[currX, currY].y = currY;
            //path[currX, currY].position.Rotate(-90, 0, 0);
            pathOrder.Add(path[currX, currY]);

            GeneratePath(path, currX, currY);
            Generate(path);
	    }
	    catch
	    {
	        while (pathOrder.Count > 0)
	        {
	            pathOrder.Remove(pathOrder[0]);
	        }
            SeedPath(path);
            Generate(path);
	    }

        //set player starting position
        var start = pathOrder[0];
        player.transform.position = new Vector3(0, 2, 0);

        //test method to be called after terrain gen and player set
	    Test();
	}

    void Test()
    {
        var pos = player.position;
        pos.y = 200;
        var copy = Instantiate(enemy, player.transform.position, Quaternion.identity);
    }


#region "Path Generation"
    void Generate(mycell[,] path)
    {
        var pathArray = pathOrder.ToArray();

        var rotate = transform;
        rotate.rotation = Quaternion.identity;
        //instantiate level
        Debug.Log("Made it");
        var pathcount = 0;


        for (int i = 0; i < mazeH; i++)
        {
            for (int j = 0; j < mazeW; j++)
            {
                path[i, j].rotate = new Vector3(-90, 0, 0);
            }
        }
        
        int minX = 0, maxX = 0, minY = 0, maxY = 0;

        foreach (var cell in pathOrder)
        {


            if (pathOrder.IndexOf(cell) == pathOrder.Count - 1)
            {
                path[cell.x, cell.y].tile = "circle";
                var nextPath = pathArray[pathcount - 1];
                //rotate towwards this path
                if (nextPath.y < cell.y)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }
                else if (nextPath.x > cell.x)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }
                else if (nextPath.x < cell.x)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }
                else
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }



                var nexusPosition = transform.position;
                nexusPosition.x = cell.x * tileOffset;
                nexusPosition.z = cell.y * tileOffset;
                nexusPosition.y = 0;

                var copy = Instantiate(Nexus, nexusPosition, rotate.rotation) as GameObject;
                copy.transform.Rotate(cell.rotate.x, cell.rotate.y, cell.rotate.z);

            }
            else if (pathcount > 0)
            {
                var nextPath = pathArray[pathcount + 1];
                var prevPath = pathArray[pathcount - 1];
                if (prevPath.x == nextPath.x)
                {
                    //we are on the same x plane
                    if (prevPath.y > nextPath.y)
                    {
                        //go stright up
                        path[cell.x, cell.y].tile = "straight";
                    }
                    else if (prevPath.y < nextPath.y)
                    {
                        //go stright down
                        path[cell.x, cell.y].tile = "straight";
                    }
                    else
                    {
                        path[cell.x, cell.y].tile = "???";
                    }
                    path[cell.x, cell.y].rotate = new Vector3(-90, 90, 0);

                }

                else if (prevPath.y == nextPath.y)
                {
                    //we are on the same 'y' plane
                    if (prevPath.x > nextPath.x)
                    {
                        path[cell.x, cell.y].tile = "straight";
                    }
                    else if (prevPath.x < nextPath.x)
                    {
                        path[cell.x, cell.y].tile = "straight";
                    }
                    else
                    {
                        path[cell.x, cell.y].tile = "???";
                    }
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }
                else
                {
                    path[cell.x, cell.y].tile = "turn";
                    //going from top to bottom
                    if (prevPath.x < cell.x)
                    {
                        if (cell.y < nextPath.y)
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 90, 0);
                            path[cell.x, cell.y].color = Color.magenta;
                        }
                        else
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                            path[cell.x, cell.y].color = Color.red;
                        }

                    }
                    else if (prevPath.x > cell.x)
                    {
                        if (cell.y < nextPath.y)
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 180, 0);
                            path[cell.x, cell.y].color = Color.blue;
                        }
                        else
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                            path[cell.x, cell.y].color = Color.black;
                        }
                    }
                    else if (prevPath.y > cell.y)
                    {
                        if (cell.x < nextPath.x)
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 180, 0);
                            path[cell.x, cell.y].color = Color.yellow;
                        }
                        else
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                            path[cell.x, cell.y].color = Color.cyan;
                        }
                    }
                    else if (prevPath.y < cell.y)
                    {
                        if (cell.x < nextPath.x)
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 270, 0);
                            path[cell.x, cell.y].color = Color.green;
                        }
                        else
                        {
                            path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                            path[cell.x, cell.y].color = Color.gray;
                        }
                    }


                    ////rotate towwards path
                    //if (nextPath.y < cell.y)
                    //{
                    //    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                    //    path[cell.x, cell.y].color = Color.red;
                    //}
                    //else if (nextPath.x > cell.x)
                    //{
                    //    path[cell.x, cell.y].rotate = new Vector3(-90, 90, 0);
                    //    path[cell.x, cell.y].color = Color.blue;
                    //}
                    //else if (nextPath.x < cell.x)
                    //{
                    //    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                    //    path[cell.x, cell.y].color = Color.yellow;
                    //}
                    //else
                    //{
                    //    path[cell.x, cell.y].rotate = new Vector3(-90, 270, 0);
                    //    path[cell.x, cell.y].color = Color.magenta;
                    //}
                }

            }
            else
            {
                minX = cell.x;
                minY = cell.y;
                maxX = cell.y;
                maxY = cell.y;
                var nextPath = pathArray[pathcount + 1];
                path[cell.x, cell.y].tile = "circle";
                //rotate towwards this path
                if (nextPath.y < cell.y)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 90, 0);
                }
                else if (nextPath.x > cell.x)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 180, 0);
                }
                else if (nextPath.x < cell.x)
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 0, 0);
                }
                else
                {
                    path[cell.x, cell.y].rotate = new Vector3(-90, 270, 0);
                }
            }
            minX = Math.Min(cell.x, minX);
            minY = Math.Min(cell.y, minY);
            maxX = Math.Max(cell.x, maxX);
            maxY = Math.Max(cell.y, maxY);

            pathcount++;
        }



        Vector3 position;
        //render the boxes
        for (int i = minY; i <= maxY; i++)
        {
            for (int j = minX; j <= maxX; j++)
            {

                //for (int i = 0; i <= mazeH-1; i++) {
                //for(int j = 0; j <= mazeW-1; j++){
                string neededTile = "";
                position = transform.position;

                position.x = 0;
                position.y = 0;
                position.x = i*tileOffset;
                position.z = j*tileOffset;
                position.y = 0;

                GameObject copy;
                var temp = path[i, j].tile;
                switch (path[i, j].tile)
                {
                    case "circle":
                        copy = Instantiate(tiles[0], position, rotate.rotation) as GameObject;
                        copy.transform.Rotate(path[i, j].rotate.x, path[i, j].rotate.y, path[i, j].rotate.z);
                        break;
                    case "turn":
                        copy = Instantiate(tiles[2], position, rotate.rotation) as GameObject;
                        copy.transform.Rotate(path[i, j].rotate.x, path[i, j].rotate.y, path[i, j].rotate.z);
                        var render = copy.GetComponent<Renderer>();
                        render.material.color = path[i, j].color;
                        break;
                    case "straight":
                        copy = Instantiate(tiles[1], position, rotate.rotation) as GameObject;
                        copy.transform.Rotate(path[i, j].rotate.x, path[i, j].rotate.y, path[i, j].rotate.z);
                        break;
                    default:
                        copy = Instantiate(tiles[3], position, rotate.rotation) as GameObject;
                        copy.transform.Rotate(path[i, j].rotate.x, path[i, j].rotate.y, path[i, j].rotate.z);
                        break;
                }
            }
        }
    }


    void SeedPath(mycell[,] terrain)
    {
        terrain[0, 0].tile = "start";
        terrain[0, 0].x = 0;
        terrain[0, 0].y = 0;
        terrain[0, 0].traversed = true;
        pathOrder.Add(terrain[0, 0]);

        terrain[0, 1].tile = "";
        terrain[0, 1].x = 1;
        terrain[0, 1].y = 0;
        terrain[0, 1].traversed = true;
        pathOrder.Add(terrain[0, 1]);

        terrain[0, 2].tile = "";
        terrain[0, 2].x = 2;
        terrain[0, 2].y = 0;
        terrain[0, 2].traversed = true;
        pathOrder.Add(terrain[0, 2]);

        terrain[1, 2].tile = "";
        terrain[1, 2].x = 2;
        terrain[1, 2].y = 1;
        terrain[1, 2].traversed = true;
        pathOrder.Add(terrain[1, 2]);

        terrain[1, 1].tile = "";
        terrain[1, 1].x = 1;
        terrain[1, 1].y = 1;
        terrain[1, 1].traversed = true;
        pathOrder.Add(terrain[1, 1]);

        terrain[2, 1].tile = "";
        terrain[2, 1].x = 1;
        terrain[2, 1].y = 2;
        terrain[2, 1].traversed = true;
        pathOrder.Add(terrain[2, 1]);

        terrain[3, 1].tile = "";
        terrain[3, 1].x = 1;
        terrain[3, 1].y = 3;
        terrain[3, 1].traversed = true;
        pathOrder.Add(terrain[3, 1]);

        terrain[3, 0].tile = "";
        terrain[3, 0].x = 0;
        terrain[3, 0].y = 3;
        terrain[3, 0].traversed = true;
        pathOrder.Add(terrain[3, 0]);

        terrain[4, 0].tile = "";
        terrain[4, 0].x = 0;
        terrain[4, 0].y = 4;
        terrain[4, 0].traversed = true;
        pathOrder.Add(terrain[4, 0]);

        terrain[4, 1].tile = "";
        terrain[4, 1].x = 1;
        terrain[4, 1].y = 4;
        terrain[4, 1].traversed = true;
        pathOrder.Add(terrain[4, 1]);

        terrain[4, 2].tile = "";
        terrain[4, 2].x = 2;
        terrain[4, 2].y = 4;
        terrain[4, 2].traversed = true;
        pathOrder.Add(terrain[4, 2]);

        terrain[4, 3].tile = "";
        terrain[4, 3].x = 3;
        terrain[4, 3].y = 4;
        terrain[4, 3].traversed = true;
        pathOrder.Add(terrain[4, 3]);

        terrain[3, 3].tile = "";
        terrain[3, 3].x = 3;
        terrain[3, 3].y = 3;
        terrain[3, 3].traversed = true;
        pathOrder.Add(terrain[3, 3]);

        terrain[2, 3].tile = "";
        terrain[2, 3].x = 3;
        terrain[2, 3].y = 2;
        terrain[2, 3].traversed = true;
        pathOrder.Add(terrain[2, 3]);

        terrain[2, 4].tile = "";
        terrain[2, 4].x = 4;
        terrain[2, 4].y = 2;
        terrain[2, 4].traversed = true;
        pathOrder.Add(terrain[2, 4]);
    }

    void GeneratePath(mycell[,] terrain, int currX, int currY)
    {
        //up, right, down, left
        List<int> directions = new List<int>(new[] { 1, 2, 3, 4 });
        bool checking = true;
        while (checking && directions.Count > 0)
        {
            int[] dirArray = directions.ToArray();
            var temp = Random.Range(0, directions.Count - 1);
            switch (dirArray[temp])
            {
                case 1: if (checkPath(terrain, currX, currY, 1))
                    {
                        currY--;
                        terrain[currY, currX].tile = "";
                        terrain[currY, currX].traversed = true;
                        terrain[currY, currX].x = currX;
                        terrain[currY, currX].y = currY;
                        //terrain[currX, currY].position.Rotate(-90, 0, 0);
                        pathOrder.Add(terrain[currY, currX]);
                        checking = false;
                        GeneratePath(terrain, currX, currY);
                    }
                    else
                    {
                        directions.Remove(1);
                    }
                    break;
                case 2: if (checkPath(terrain, currX, currY, 2))
                    {
                        currX++;
                        terrain[currY, currX].tile = "";
                        terrain[currY, currX].traversed = true;
                        terrain[currY, currX].x = currX;
                        terrain[currY, currX].y = currY;
                        //terrain[currX, currY].position.Rotate(-90, 0, 0);
                        pathOrder.Add(terrain[currY, currX]);
                        checking = false;
                        GeneratePath(terrain, currX, currY);
                    }
                    else
                    {
                        directions.Remove(2);
                    }
                    break;
                case 3: if (checkPath(terrain, currX, currY, 3))
                    {
                        currY++;
                        terrain[currY, currX].tile = "";
                        terrain[currY, currX].traversed = true;
                        terrain[currY, currX].x = currX;
                        terrain[currY, currX].y = currY;
                        //terrain[currX, currY].position.Rotate(-90, 0, 0);
                        pathOrder.Add(terrain[currY, currX]);
                        checking = false;
                        GeneratePath(terrain, currX, currY);
                    }
                    else
                    {
                        directions.Remove(3);
                    }
                    break;
                case 4: if (checkPath(terrain, currX, currY, 4))
                    {
                        currX--;
                        terrain[currY, currX].tile = "";
                        terrain[currY, currX].traversed = true;
                        terrain[currY, currX].x = currX;
                        terrain[currY, currX].y = currY;
                        //terrain[currX, currY].position.Rotate(-90, 0, 0);
                        pathOrder.Add(terrain[currY, currX]);
                        checking = false;
                        GeneratePath(terrain, currX, currY);
                    }
                    else
                    {
                        directions.Remove(4);
                    }
                    break;
            }
        }
    }

    private bool checkPath(mycell[,] terrain, int currX, int currY, int direction)
    {
        //up, right, down, left
        switch (direction)
        {
            case 1:
                if (currY - 1 > 0 && currY <= mazeH)
                {
                    if (!terrain[currX, currY - 1].traversed)
                    {
                        return true;
                    }
                }
                return false;
            case 2:
                if (currX > 0 && currX + 1 < mazeW)
                {
                    if (!terrain[currX + 1, currY].traversed)
                    {
                        return true;
                    }
                }
                return false;
            case 3:
                if (currY >= 0 && currY + 1 < mazeH)
                {
                    if (!terrain[currX, currY + 1].traversed)
                    {
                        return true;
                    }
                }
                return false;
            case 4:
                if (currX - 1 > 0 && currX + 1 < mazeW)
                {
                    if (!terrain[currX - 1, currY].traversed)
                    {
                        return true;
                    }
                }
                return false;
            default: return false;
        }

    }

#endregion
	// Update is called once per frame
	void Update () {
	
	}
}

public class mycell
{
    public string tile;
    public bool traversed;
    public int x;
    public int y;
    public Vector3 rotate = new Vector3(0, 0, 0);
    public Color color;
}
