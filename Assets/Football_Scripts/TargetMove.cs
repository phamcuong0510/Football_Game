using UnityEngine;
using System.Collections;

public class TargetMove : MonoBehaviour {
public float timer;
public int newtarget;
public float speed;
public NavMeshAgent nav;
public Vector3 a;
public Vector3 Scale;
public float x,y;

//public int sc = GameControl.scorePlayer;
	// Use this for initialization
	void Start () {
	nav = gameObject.GetComponent<NavMeshAgent>();
   Scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
		x =    Scale.x - (GameControl.scorePlayer*0.01f);
		y =   Scale.y - GameControl.scorePlayer*0.01f;
		transform.localScale = new Vector3(x,y,Scale.z);


		if(GameControl.scorePlayer >7){
		timer += Time.deltaTime;
		
	if(timer >= newtarget){
		
		newTarget();
		timer = 0;
		nav.speed = 1;
		
	}
		}
	
	}

	

	void newTarget(){
		
	
		

		float xPos = Random.Range(-2,2)
		;
		
		a = new Vector3(xPos,gameObject.transform.position.y, gameObject.transform.position.z);
		nav.SetDestination(a);
		nav.updateRotation = false;
	
		
	}


}
