using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaitingBoard : Board
{
    
    public bool PlaceChess(BaseChess data)
    {
        foreach(var x in Grid.GridArray)
        {
            if (x.Chess == null)
            {
                data.ChangeChessPosition(this,x.Position);
                return true;
            }
        }
        Destroy(data.gameObject);
		return false;
	}
}
