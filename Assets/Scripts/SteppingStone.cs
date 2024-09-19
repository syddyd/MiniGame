using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

//Sydney Dacks for COMP 521
//this is a regular c# script (not monobehaviour) which helps us store info about potential stepping stones in a fairly lightweight way while generating path 
public class SteppingStone 
{
    private int mRow; 
    private int mColumn; 
    private SteppingStone pred;
    public bool explored;

    public SteppingStone(int row, int column){
        mRow = row;
        mColumn = column;
        explored = false;
    }   

    public int GetRow(){
        return mRow;
    }

    public int GetColumn(){
        return mColumn;
    }

    public SteppingStone GetPredecessor(){
        return pred;
    }

    public void SetPredecessor( SteppingStone predecessor){
        pred = predecessor;
    }
}
