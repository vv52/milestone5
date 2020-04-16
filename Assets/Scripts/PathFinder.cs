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
        	// Pop the top item off of the priority queue
        	TilePath current = new TilePath(pathQueue.Dequeue());
        	var tempTile = map.GetTile(start);
            var currentPosition = current.GetMostRecentTile().Position;

        	if (currentPosition == end)
        	{
        		//If the item contains the final tile in the path, you are done.
        		found = true;
        		discoveredPath = current;
        	}
        	else
        	{	
        		// If not, for each of the tile's neighbors (there should be 4 since we're using square tiles)
        		Vector3Int up = new Vector3Int(currentPosition.x, currentPosition.y + 1, currentPosition.z);
            	Vector3Int down = new Vector3Int(currentPosition.x, currentPosition.y - 1, currentPosition.z);
            	Vector3Int left = new Vector3Int(currentPosition.x - 1, currentPosition.y, currentPosition.z);
            	Vector3Int right = new Vector3Int(currentPosition.x + 1, currentPosition.y, currentPosition.z);
            	Vector3Int[] adjacentTiles = new Vector3Int[] { up, down, left, right };

            	// Create a new path with the additional tile.
            	for (int i = 0; i < adjacentTiles.Length; i++)
            	{
            		tempTile = map.GetTile(adjacentTiles[i]);
	            	if (tempTile != null)
	            	{
	            		Tile newTile = new Tile(tileFactory.GetTile(tempTile.name));
		            	newTile.Position = adjacentTiles[i];
		            	TilePath newPath = new TilePath(current);
		            	newPath.AddTileToPath(newTile);
		            	
		            	if (newTile.Position == end)
		            	{
		            		// If that path contains the final tile, you're done.
		            		found = true;
		            		discoveredPath = newPath;
		            	}
		            	else
		            	{
		            		// If not, add that path back into the Priority Queue.
		            		pathQueue.Enqueue(newPath);
		            	}
	            	}
            	}
        	}			
        }
        return discoveredPath;
    }
}
