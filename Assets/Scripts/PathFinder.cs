using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder
{
    public static TilePath DiscoverPath(Tilemap map, Vector3Int start, Vector3Int end)
    {
        //you will return this path to the user.  It should be the shortest path between
        //the start and end vertices 
        TilePath discoveredPath = new TilePath();

        //TileFactory is how you get information on tiles that exist at a particular vector's
        //coordinates
        TileFactory tileFactory = TileFactory.GetInstance();

        //This is the priority queue of paths that you will use in your implementation of
        //Dijkstra's algorithm
        PriortyQueue<TilePath> pathQueue = new PriortyQueue<TilePath>();

        //You can slightly speed up your algorithm by remembering previously visited tiles.
        //This isn't strictly necessary.
        Dictionary<Vector3Int, int> discoveredTiles = new Dictionary<Vector3Int, int>();

        //quick sanity check
        if(map == null || start == null || end == null)
        {
            return discoveredPath;
        }

        //This is how you get tile information for a particular map location
        //This gets the Unity tile, which contains a coordinate (.Position)
        var startingMapLocation = map.GetTile(start);

        //And this converts the Unity tile into an object model that tracks the
        //cost to visit the tile.
        var startingTile = tileFactory.GetTile(startingMapLocation.name);
        startingTile.Position = start;

        //Any discovered path must start at the origin!
        discoveredPath.AddTileToPath(startingTile);

        //This adds the starting tile to the PQ and we start off from there...
        pathQueue.Enqueue(discoveredPath);
        bool found = false;
        while(found == false && pathQueue.IsEmpty() == false)
        {
            //TODO: After some experimentation, I have found that
            //      my pathfinding is not working because of something
            //      specifically happening at line 124 and the other
            //      three directions with similar lines.
            //
            //      I think that I am pulling and checking tiles
            //      improperly, I am going to go back through the
            //      TileFactory and see where my process is incorrect

        	//Pop path off priority queue
            TilePath current = pathQueue.GetFirst();
            pathQueue.Dequeue();

            //Check if popped path contains end tile
            if (current.GetMostRecentTile().Position == end)
            {
            	found = true;
            }
            else
            {
            	//Create temp variables
            	Vector3Int temp = new Vector3Int();
            	var newTile = map.GetTile(start);


            	/* start new attempt
            	Tile shortest = new Tile();
            	shortest.Weight = 9999;

            	temp = current.GetMostRecentTile().Position;
            	temp.y++;
            	newTile = map.GetTile(temp);
            	Tile newTileUp = tileFactory.GetTile(newTile.name);
            	newTileUp.Position = temp;
            	temp = current.GetMostRecentTile().Position;
            	temp.y--;
            	newTile = map.GetTile(temp);
            	Tile newTileDown = tileFactory.GetTile(newTile.name);
            	newTileDown.Position = temp;
            	temp = current.GetMostRecentTile().Position;
            	temp.x--;
            	newTile = map.GetTile(temp);
            	Tile newTileLeft = tileFactory.GetTile(newTile.name);
            	newTileLeft.Position = temp;
            	temp = current.GetMostRecentTile().Position;
            	temp.x++;
            	newTile = map.GetTile(temp);
            	Tile newTileRight = tileFactory.GetTile(newTile.name);
            	newTileRight.Position = temp;

            	Tile[] Tiles = { newTileUp, newTileDown, newTileLeft, newTileRight };
            	for (int i = 0; i < 4; i++)
            	{
            		if (Tiles[i].Weight < shortest.Weight)
            		{
            			shortest = Tiles[i];
            		}
            	}

            	TilePath newPathShortest = new TilePath(current);
            	newPathShortest.AddTileToPath(newTileUp);

            	if (newPathShortest.GetMostRecentTile().Position == end)
            	{
            		found = true;
            		discoveredPath = newPathShortest;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathShortest);
            	}
           		// end new attempt */

           		
            	//Check y+1 adjacent tile and add to queue if not at end
            	temp = current.GetMostRecentTile().Position;
            	temp.y++;
            	newTile = map.GetTile(temp);
            	Tile newTileUp = tileFactory.GetTile(newTile.name);
            	newTileUp.Position = temp;
            	TilePath newPathUp = new TilePath(current);
            	newPathUp.AddTileToPath(newTileUp);
            	if (newPathUp.GetMostRecentTile().Position == end)
            	{
            		found = true;
            		discoveredPath = newPathUp;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathUp);
            	}

            	//Check y-1 adjacent tile and add to queue if not at end
            	temp = current.GetMostRecentTile().Position;
            	temp.y--;
            	newTile = map.GetTile(temp);
            	Tile newTileDown = tileFactory.GetTile(newTile.name);
            	newTileDown.Position = temp;
            	TilePath newPathDown = new TilePath(current);
            	newPathDown.AddTileToPath(newTileDown);
            	if (newPathDown.GetMostRecentTile().Position == end)
            	{
            		found = true;
            		discoveredPath = newPathDown;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathDown);
            	}

            	//Check x-1 adjacent tile and add to queue if not at end
            	temp = current.GetMostRecentTile().Position;
            	temp.x--;
            	newTile = map.GetTile(temp);
            	Tile newTileLeft = tileFactory.GetTile(newTile.name);
            	newTileLeft.Position = temp;
            	TilePath newPathLeft = new TilePath(current);
            	newPathLeft.AddTileToPath(newTileLeft);
            	if (newPathLeft.GetMostRecentTile().Position == end)
            	{
            		found = true;
            		discoveredPath = newPathLeft;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathLeft);
            	}

            	//Check x+1 adjacent tile and add to queue if not at end
            	temp = current.GetMostRecentTile().Position;
            	temp.x++;
            	newTile = map.GetTile(temp);
            	Tile newTileRight = tileFactory.GetTile(newTile.name);
            	newTileRight.Position = temp;
            	TilePath newPathRight = new TilePath(current);
            	newPathRight.AddTileToPath(newTileRight);
            	if (newPathRight.GetMostRecentTile().Position == end)
            	{
            		found = true;
            		discoveredPath = newPathRight;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathRight);
            	}
            	

				/*
            	Tile shortest = new Tile();
            	shortest.Weight = 9999;
            	Tile[] Tiles = { newTileUp, newTileDown, newTileLeft, newTileRight };
            	for (int i = 0; i < 4; i++)
            	{
            		if (Tiles[i].Weight < shortest.Weight)
            		{
            			shortest = Tiles[i];
            		}
            	}
           		start = shortest.Position;
           		*/
            }

            //NOTE: when I comment the following line out
            //      I get better results, however, I should
            //      be able to make it happen with this line
            discoveredPath = pathQueue.GetFirst();
        }
        return discoveredPath;
    }
}
