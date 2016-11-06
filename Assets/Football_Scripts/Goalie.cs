using UnityEngine;
using System.Collections;

public class Goalie : MonoBehaviour {
public float timer;
public int newtarget;
public float speed;
public NavMeshAgent nav;
public Vector3 Target;

	// Use this for initialization
	void Start () {
	nav = gameObject.GetComponent<NavMeshAgent>();
	
	}
	
	// Update is called once per frame
	void Update () {
 

	timer += Time.deltaTime;
	if(timer >= newtarget){
		newTarget();
		timer = 0;
		nav.speed = 1;
		GetComponent<Animation>().Play("StandSave");
	}
	}

	void newTarget(){
		
		
		

		float xPos = Random.Range(-1.5f,2)
		;
		
		Target = new Vector3(xPos,gameObject.transform.position.y, gameObject.transform.position.z);
		nav.SetDestination(Target);
		nav.updateRotation = false;
		
	}
}
