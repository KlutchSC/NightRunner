using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public bool isNight;

    public static GameController control;

	// Use this for initialization
	void Start () {
        isNight = true;
        PassDayTime();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Time.time);
	}

    public void PassDayTime()
    {
        //Invoke("ChangeToNight", 20.0f);

    }

    public void ChangeToNight()
    {
        isNight = true;
        Debug.Log("Now it's night");
    }
}
