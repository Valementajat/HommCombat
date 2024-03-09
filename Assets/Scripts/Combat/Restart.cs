using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;
     public InitializeCombat combatInitializer;
    void Start()
    {

        btn.onClick.AddListener(RestartGame);
    }

    // Update is called once per frame
    void RestartGame(){
		 combatInitializer.RestartGame();
	}
}
