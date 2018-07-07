using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    public void ChangeLevel(int _newLevel)
    {
        level = _newLevel;
        transform.position = Vector3.zero;
    }
}
