using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	[Tooltip("Furthest distance bullet will look for target")]
	//가장 먼 거리의 총알이 목표물을 찾습니다
	public float maxDistance = 1000000;
	RaycastHit hit;
	[Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
	//벽 총알 타격 프리팹. 오브젝트에 데칼을 만들려면 '레벨파트' 태그가 필요합니다.
	public GameObject decalHitWall;
	[Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
	//데칼은 렌딩 문제를 일으키지 않도록 벽 앞에 약간 있어야 하므로 가장 좋은 느낌은 0.01-0.1입니다.
	public float floatInfrontOfWall;
	[Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
	//총알이 적을 맞혔을 경우 생성되는 혈흔 이펙트 프리팹
	public GameObject bloodEffect;
	[Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
	//총알 레이캐스트를 무시하려면 무기층과 플레이어층을 배치합니다.
	public LayerMask ignoreLayer;

	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	* 
	* 이 스크립트가 첨부된 Uppon 총알 생성, 총알은 해당 태그를 검색하는 레이캐스트를 생성합니다.
	* 레이캐스트가 높은 태그를 찾으면 해당 태그의 데칼을 생성합니다.
	* 
	*/
	void Update () {

		if(Physics.Raycast(transform.position, transform.forward,out hit, maxDistance, ~ignoreLayer)){
			if(decalHitWall){
				if(hit.transform.tag == "LevelPart"){
					Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
				if(hit.transform.tag == "Dummie"){
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(gameObject);
				}
			}		
			Destroy(gameObject);
		}
		Destroy(gameObject, 0.1f);
	}

}
