using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRayCastable
{
    //CursorType GetCursorType();
    bool HandleRayCast(PlayerController callingController);

}
