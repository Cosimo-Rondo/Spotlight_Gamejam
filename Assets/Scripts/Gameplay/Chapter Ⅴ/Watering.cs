using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watering : MonoBehaviour
{
    // Start is called before the first frame update
    public Chapter5Controller gameController;
    public void MissionComplete()
    {
        if (gameController != null)
            gameController.SetTodoList(2);
    }
}
