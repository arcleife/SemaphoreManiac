using UnityEngine;
using System.Collections;

public class ScoreOutBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y + 0.01f, 80);
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 2f)
        {
            Destroy(this.gameObject);
        }
	}
}
