using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

//sydney Dacks for COMP 521 
public class Tower : MonoBehaviour
{
    //components, gameObjects, etc to be set in editor 
    [SerializeField] public GameObject tower;
    [SerializeField] public GameObject stonePrefab;

    //private stuff to be used internally to the script
    private SteppingStone[,] stones = new SteppingStone[8, 10];
    private int HP = 3;

    //this detects when we are hit by a projectile and counts down HP til death. Wanted to make an animation too but ran out of time :(
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            HP--;
        }

        if (HP <= 0)
        {
            print("tower died");
            GeneratePath();
            Destroy(tower);
        }
        print("Hp is " + HP);
    }

    //see README.txt in assignment submission 
    public void GeneratePath()
    {
        print("generating path");
        for (int i = 0; i < 10; i++)
        {
            for (int c = 0; c < 8; c++)
            {
                print("generating potential stones");
                stones[c, i] = new SteppingStone(c, i);
            }
        }
        System.Random random = new System.Random();
        List<SteppingStone> frontier = new List<SteppingStone>();

        int currentColumn = random.Next(0, 10);
        int currentRow = 0;

        SteppingStone potentialStone = stones[currentRow, currentColumn];
        potentialStone.SetPredecessor(new SteppingStone(-1, -1)); //dummy stone
        potentialStone.explored = true;

        while (currentRow < 7)
        {
            print("constructing maze ");
            if (currentColumn > 0)
            {
                potentialStone = stones[currentRow, currentColumn - 1];
                if (!potentialStone.explored)
                {
                    frontier.Add(potentialStone);
                    potentialStone.SetPredecessor(stones[currentRow, currentColumn]);
                }
            }
            if (currentColumn < 9)
            {
                potentialStone = stones[currentRow, currentColumn + 1];
                if (!potentialStone.explored)
                {
                    frontier.Add(potentialStone);
                    potentialStone.SetPredecessor(stones[currentRow, currentColumn]);
                }
            }
            if (currentRow > 0)
            {
                potentialStone = stones[currentRow - 1, currentColumn];
                if (!potentialStone.explored)
                {
                    frontier.Add(potentialStone);
                    potentialStone.SetPredecessor(stones[currentRow, currentColumn]);
                }
            }

            potentialStone = stones[currentRow + 1, currentColumn];
            if (!potentialStone.explored)
            {
                frontier.Add(potentialStone);
                potentialStone.SetPredecessor(stones[currentRow, currentColumn]);
            }

            int randomIndex = random.Next(0, frontier.Count);
            SteppingStone newStone = frontier[randomIndex];
            
            currentRow = newStone.GetRow();
            currentColumn = newStone.GetColumn();

            stones[currentRow, currentColumn].explored = true;
        }

        BackTrack(stones[currentRow, currentColumn]);
    }

    public void BackTrack(SteppingStone endStone)
    {
        SteppingStone currentStone = endStone;
        while (currentStone.GetRow() >= 0)
        {
            Vector3 stonePos = new Vector3(72 - currentStone.GetColumn() * 5, 1, 17 - currentStone.GetRow() * 5);
            print("row: " + currentStone.GetRow() + " column: " + currentStone.GetColumn()+" x: "+stonePos.x + " z: "+stonePos.z);
            Instantiate(stonePrefab, stonePos, Quaternion.identity);
            currentStone = currentStone.GetPredecessor();
        }
    }

}
