using UnityEngine;
using System.Collections;

public class AttackAreaController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroyMe", 0.2f);
	}
	
    void DestroyMe ()
    {
        Destroy(this.gameObject);
    }
}
