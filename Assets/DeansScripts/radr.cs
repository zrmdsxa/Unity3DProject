using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class radr : MonoBehaviour {
	public float insideRadarDistance = 20;
	public float blimpSizePercentage = 5;
	public GameObject rawImageBlipSphere;
	public GameObject rawImageBlipCube;
	private RawImage rawImageradarBackground;
	private Transform playerTransform;
	private float radarWidth;
	private float radarHeight;
	private float blipHeight;
	private float blipWidth;
	void Start(){
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		rawImageradarBackground = GetComponent<RawImage>();
		radarWidth = rawImageradarBackground.rectTransform.rect.width;
		radarHeight = rawImageradarBackground.rectTransform.rect.height;
		blipHeight = radarHeight*blimpSizePercentage/100;
		blipWidth = radarWidth * blimpSizePercentage/100;

	}
void Update(){

	RemoveAllBlips();
	FindAndDisplayBlipForTag("Cube", rawImageBlipCube);
	FindAndDisplayBlipForTag("Sphere", rawImageBlipSphere);


	}
	private void FindAndDisplayBlipForTag(string tag, GameObject prefabBlip){
		Vector3 playerPos = playerTransform.position;
		GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject target in targets){
			Vector3 targetPos = target.transform.position;
			float distanceToTargetb = Vector3.Distance(targetPos,playerPos);
			if((distanceToTargetb <= insideRadarDistance)){
				Vector3 normalizedTargetPosition = NormalisedPosition(playerPos,targetPos);
				Vector2 blipPosition = CalaculateBlipPosition(normalizedTargetPosition);
				DrawBlip(blipPosition, prefabBlip);
			}
		}

	}
	private void RemoveAllBlips(){
		GameObject[] blips = GameObject.FindGameObjectsWithTag("Blip");
		foreach(GameObject blip in blips )
		Destroy(blip);
	}
	private Vector3 NormalisedPosition(Vector3 playerPos, Vector3 targetPos ){

		float normalizedTargetX = (targetPos.x - playerPos.x) / insideRadarDistance;
		float normalizedTargetZ = (targetPos.z - playerPos.z) / insideRadarDistance;
		return new Vector3 (normalizedTargetX, 0, normalizedTargetZ);


	}
	private Vector2 CalaculateBlipPosition(Vector3 targetPos ){
		//find angle from the player to target
		float angleToTarget = Mathf.Atan2(targetPos.x, targetPos.z) *Mathf.Rad2Deg;
		//directionplayer is facing
		float anglePlayer = playerTransform.eulerAngles.y;
		//subtract player angle to get relative angle to object
		//subtarct 90
		//so 0 degrees same direction as player is up 
		float angleRadarDegrees = angleToTarget - anglePlayer - 90;
		//calaculate x,y position given angle and distance 
		float normalizedDistanceToTarget = targetPos.magnitude;
		float angleRadians = angleRadarDegrees * Mathf.Deg2Rad;
		float blipX = normalizedDistanceToTarget * Mathf.Cos(angleRadians);
		float blipY = normalizedDistanceToTarget * Mathf.Sin(angleRadians);
		//scale blip position according to radar size
		blipX *= radarWidth/2;
		blipY *= radarHeight/1;
		//offseyt blip position relative to radar size
		blipX += radarWidth/2;
		blipY += radarHeight/2;
		return new Vector2(blipX,blipY);

	}
	private void DrawBlip(Vector2 pos, GameObject blipPrefab){
		GameObject blipGO = (GameObject) Instantiate(blipPrefab);
		blipGO.transform.SetParent(transform.parent);
		RectTransform rt = blipGO.GetComponent<RectTransform>();
		rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,pos.x, blipWidth);
		rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,pos.y, blipHeight);

	}

}
