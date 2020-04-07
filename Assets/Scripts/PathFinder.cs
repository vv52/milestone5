﻿using System.Collections;
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
            //TODO: Implement Dijkstra's algorithm!

        	//Pop path off priority queue
            TilePath current = pathQueue.Dequeue();

            //Check if popped path contains end tile
            if (current.GetMostRecentTile().Position == end)
            {
            	found = true;
            }
            else
            {
            	//Create temp Tile for readability
            	Tile lastTile = current.GetMostRecentTile();

            	//Check y+1 adjacent tile and add to queue if not at end
            	TilePath newPathUp = new TilePath(current);
            	newPathUp.AddTileToPath(lastTile.Position.up);
            	if (newPathUp.GetMostRecentTile().Position == end)
            	{
            		found = true;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathUp);
            	}

            	//Check y-1 adjacent tile and add to queue if not at end
            	TilePath newPathDown = new TilePath(current);
            	newPathDown.AddTileToPath(lastTile.Position.down);
            	if (newPathDown.GetMostRecentTile().Position == end)
            	{
            		found = true;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathDown);
            	}

            	//Check x-1 adjacent tile and add to queue if not at end
            	TilePath newPathLeft = new TilePath(current);
            	newPathLeft.AddTileToPath(lastTile.Position.left);
            	if (newPathLeft.GetMostRecentTile().Position == end)
            	{
            		found = true;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathLeft);
            	}

            	//Check x+1 adjacent tile and add to queue if not at end
            	TilePath newPathRight = new TilePath(current);
            	newPathRight.AddTileToPath(lastTile.Position.right);
            	if (newPathRight.GetMostRecentTile().Position == end)
            	{
            		found = true;
            	}
            	else
            	{
            		pathQueue.Enqueue(newPathRight);
            	}
            }

            discoveredPath = pathQueue.GetFirst();

            //This line ensures that we don't get an infinite loop in Unity.
            //You will need to remove it in order for your pathfinding algorithm to work.
            //found = true;
        }
        return discoveredPath;
    }
}