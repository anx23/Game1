using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour {






	private GameObject player;
	private playerlockon lockobj;
	public bool iscounter;



	public Transform target;        // ターゲットの位置情報
	//public Transform aimtarget;        // ターゲットの位置情報
	public int life=100;
	public int lifeMax;             // ライフ
	int _currentlife;

	Rigidbody rb;
	Animator anim;
	UnityEngine.AI.NavMeshAgent nav;

	public float visibleDistance;   // 可視距離
	float targetDistance;           // ターゲットとの距離

	public float sightAngle;        // 視野角

	public Transform lineOfSight1;         // 目の位置
	Ray gazeRay1;                   // 目とターゲットを結ぶRay

	public LayerMask visibleLayer;  // 見る対象のLayer（ターゲットと障害物が含まれる）

	public float walkSpeed;         // さまよっているときの速度
	public float runSpeed;          // おいかけているときの速度

	public float targetLostLimitTime;   // ターゲットを見失うまでの時間
	public float targetFindDistance;    // ターゲットを見つける距離（至近距離に近寄ると視野に関係なく見つける）
	float _lostTime = 0f;

	public float idleMaxTime;       // たちどまっている時間
	float _idleTime = 0f;

	public float wanderMaxTime;     // さまよっている時間
	float _wanderTime = 0f;
	bool canmove;
	public bool isTarget;
	float combatLength=3.0f;  

	public enum eState                     // 状態
	{
		Idle,       // 立ち止まっている
		Wander,     // さまよっている
		Chase,      // 追っている
		Attack,     // 攻撃している
		Dead,       // 死んでいる
		Combat,
	}


	public enum CombatType
	{
		offensive,
		deffensive,
		normal,

	}

	public CombatType c_statte=CombatType.normal;

	public eState _state = eState.Idle;


	//戦闘状態
	public enum Combat_state
	{

		near,
		far,
	}








	// --- 初期化 ----------------------------------------------------------
	private void Start ()
	{
		//rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		_currentlife = lifeMax;
		//lineOfSight1 = GameObject.Find ("LineOfSight").transform;

		player = GameObject.FindWithTag ("Player");
		lockobj = player.GetComponent<playerlockon> ();
		//nav.updateRotation = false;

		target=player.transform;


	}

	// --- 更新処理 ----------------------------------------------------------
	private void FixedUpdate ()
	{
		switch (_state)
		{
		case eState.Idle:
			Idle ();
			break;

		case eState.Wander:
			Wander ();
			break;

		case eState.Chase:
			Chase ();
			//enemyavoid ();
			break;
		case eState.Combat:
			Combat ();
			//enemyavoid ();
			break;

		case eState.Dead:
			break;
		}



		//アニメーション
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("move")) {

			nav.isStopped = false;
		} else {

			nav.isStopped = true;
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Damage")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.9) {

			anim.ResetTrigger ("damage");
		}


		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("attck")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime==8.0f) {

			anim.SetTrigger ("combo");


		}

	}


	// --- 立ち止まっているときの処理 ----------------------------------------------------------
	void Idle ()
	{
		Search (_state);

		_idleTime += Time.deltaTime;
		if (_idleTime > idleMaxTime)                        // 一定時間立ち止まったら、さまよう
		{
		//	Debug.Log ("Wandering");
			//anim.SetTrigger ("Walk");
			anim.SetFloat("move",0.5f);
			nav.isStopped=false;
			nav.SetDestination (new Vector3 (transform.position.x+ Random.Range (-5f, 5f), 0f,transform.position.z+ Random.Range (-5f, 5f))); // ランダムな場所へ向かう
			_state = eState.Wander;
			nav.speed = walkSpeed;
			_idleTime = 0f;
		}
	}

	// --- さまよっているときの処理 ----------------------------------------------------------
	void Wander ()
	{
		Search (_state);

		_wanderTime += Time.deltaTime;
		if (_wanderTime > wanderMaxTime ) // 一定時間さまようか行先に着いたら、立ち止まる
		{
			//Debug.Log ("Idling");
			//anim.SetTrigger ("Idle");
			anim.SetFloat("move",0);
			nav.isStopped=true;
			_state = eState.Idle;
			_wanderTime = 0f;
			return;
		}
	}



	// --- ターゲットを探す処理 ----------------------------------------------------------
	void Search (eState state)
	{
		float _angle = Vector3.Angle (target.position - transform.position, lineOfSight1.forward);

		if (_angle <= sightAngle)
		{
			Debug.Log ("Target In SightAngle");

			gazeRay1.origin = lineOfSight1.position;
			gazeRay1.direction = target.position - lineOfSight1.position;
			RaycastHit hit;

			if (Physics.Raycast (gazeRay1, out hit, visibleDistance, visibleLayer))
			{
				Debug.DrawRay (gazeRay1.origin, gazeRay1.direction * hit.distance, Color.red);

				if (hit.collider.gameObject.tag != "Obstacle")    // ターゲットとの間に障害物がない
				{
					if (state == eState.Idle || state == eState.Wander)
					{
						TargetFound ();
					}
					else if (state == eState.Chase)
					{
						TargetInSight ();
					}
					return;
				}
			}

			Debug.DrawRay (gazeRay1.origin, gazeRay1.direction * visibleDistance, Color.gray);
		}

		targetDistance = ( transform.position - target.position ).magnitude;

		if (targetDistance < targetFindDistance)            // 距離でターゲット発見
		{
			if (state == eState.Idle || state == eState.Wander)
			{
				TargetFound ();
			}
			else if (state == eState.Chase)
			{
				TargetInSight ();
			}
			return;
		}
	}



	// --- ターゲットを発見したときの処理 ----------------------------------------------------------
	void TargetFound ()
	{
		//Debug.Log ("Target Found");
		//anim.SetTrigger ("Run");
		anim.SetFloat("move",1.0f);
		nav.isStopped=false;
		nav.SetDestination (target.position);
		_state = eState.Chase;

		_idleTime = 0f;
		_wanderTime = 0f;
	}

	// --- ターゲットを追っているときの処理 ----------------------------------------------------------
	void Chase ()                                // 
	{
		nav.isStopped = false;
		nav.SetDestination (target.position);
		Search (_state);
		anim.SetTrigger ("idle");
		anim.SetFloat("move",1.0f);
		_lostTime += Time.deltaTime;
		// Debug.Log ("LostTime: " + _lostTime);

		if (_lostTime > targetLostLimitTime)                 // 一定時間視界の外なら、見失う
		{

		
		//	Debug.Log ("Target Lost");                       // ターゲットロスト
			_state = eState.Idle;
			nav.isStopped=true;
			anim.SetTrigger ("idle");
			anim.SetFloat("move",0);
			nav.speed = 0f;
			_lostTime = 0f;
		}

		if (Vector3.Distance (transform.position, target.transform.position) >10.5f) {
			lockobj.nearenemys.Remove (this.gameObject);
		}




		if (Vector3.Distance (transform.position, target.transform.position) < 3.5f) {
			nav.speed = 0.2f;
			nav.stoppingDistance = 3.3f;
			//nav.SetDestination (target.position);
			anim.SetFloat ("move",0);
			//anim.SetTrigger()
			nav.velocity=Vector3.zero;
			nav.isStopped = true;
			//nav.updatePosition = false;
			_state = eState.Combat;
			//nav.updatePosition = false;
			//nav.SetDestination (target.position);

		}




	}

	// --- ターゲットが視野に入っているときの処理 ----------------------------------------------------------
	void TargetInSight ()
	{
		//Debug.Log ("Target In Sight");
		_lostTime = 0f;
	}


	bool isRunning=false;

	void Combat(){
		

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("move")) {
			transform.LookAt (target.transform.position);
		}



		//nav.speed = 1;
		/*
		if (isTarget)
			c_statte = CombatType.offensive;
		else
			c_statte = CombatType.deffensive;

		switch (c_statte) {
		case CombatType.offensive:
			combatLength = 2.5f;
			break;
		case CombatType.deffensive:
			combatLength = 4.0f;
			break;
		}


		*/

		if (Vector3.Distance (transform.position, target.transform.position) < 0.8f) {
		//	nav.velocity = Vector3.zero;
		//	nav.isStopped = true;
			nav.velocity=Vector3.zero;
			nav.isStopped = true;
			//nav.updatePosition = false;
			StopCoroutine (Combat_far());
		}




		if (Vector3.Distance (transform.position, target.transform.position) < 1.2f) {
			anim.applyRootMotion = false;
			//nav.isStopped = true;
			StopCoroutine(Combat_far());
			StartCoroutine (Combat_near ());

		}



		if (Vector3.Distance (transform.position, target.transform.position) < 3.5f) {
			
			StartCoroutine (Combat_far());
		
			nav.speed = walkSpeed;
		} 

		else{
			///back to chase
			if (!anim.GetCurrentAnimatorStateInfo (0).IsTag ("attack")) {
				nav.isStopped=false;
				//nav.updatePosition = true;
				StopCoroutine (Combat_near ());
				StopCoroutine (Combat_far ());
				nav.updatePosition = true;
				//anim.SetInteger ("AttackType",0);
				_state = eState.Chase;
				nav.stoppingDistance = 1.5f;
				//nav.SetDestination (target.position);
				nav.speed = runSpeed;

		
			}

		}

	}


	void Animhandlemove(){

		Vector3 localnavdirection = transform.InverseTransformDirection (nav.desiredVelocity);

		anim.SetFloat("move",localnavdirection.z,0.1f,Time.deltaTime);
		//anim.SetFloat ("movez",localnavdirection.z,0.1f,Time.deltaTime);
	}


	void enemyavoid (){


		RaycastHit hit;
		//Debug.Log("test");
		if (Physics.Raycast (transform.position+new Vector3(0,0.5f,0),transform.forward, out hit, 2f, visibleLayer))
		{
			//Debug.DrawRay (gazeRay1.origin, gazeRay1.direction * hit.distance, Color.red);
			//Debug.Log("hit");
			if (hit.collider.gameObject.tag == "Enemy")    // ターゲットとの間に障害物がない
			{
				Debug.Log("avoid");
				nav.updatePosition = false;
				nav.isStopped = true;
			}
		}

		//	Debug.DrawRay (gazeRay1.origin, gazeRay1.direction * visibleDistance, Color.gray);



	}

	//遠距離
	IEnumerator Combat_far(){

		//nav.isStopped=true;
		if (isRunning)
			yield break;
		isRunning = true;

		if (Vector3.Distance (transform.position, target.transform.position) < 0.8f) {
			nav.isStopped = true;
			StopCoroutine (Combat_far());
		}
		c_statte = CombatType.deffensive;
		yield return new WaitForSeconds (3.0f);
		//anim.applyRootMotion = false;
		anim.SetFloat ("move",0.5f);
		int rand = Random.Range (0,100);

		if (rand < 50) {
			
			anim.SetTrigger ("scream");
			Debug.Log ("wait");
		}
		else if(rand<100){
			//anim.SetFloat ("move",0.8f);

			nav.stoppingDistance = 1.2f;
			nav.isStopped = false;
			nav.updateRotation = false;
			nav.SetDestination (target.position);



		}
		if (Vector3.Distance (transform.position, target.transform.position) < 0.8f) {
			nav.isStopped = true;
			StopCoroutine (Combat_far());
		}

		isRunning = false;
	}
	//近距離
	IEnumerator Combat_near(){
		//nav.isStopped=true;
	
	
		if (isRunning)
			yield break;
		isRunning = true;
		c_statte=CombatType.offensive;
		//yield return new WaitForSeconds (8.0f);

		int rand = Random.Range (0,100);
		int attacktpe = Random.Range (0,3);
		yield return new WaitForSeconds (2.0f);
		nav.isStopped = true;
		if (rand < 80) {
			//LerpAttack ();
			//yield return new WaitForSeconds (2.0f);
			anim.SetTrigger ("attack");
			anim.applyRootMotion = true;
			//int attacktpe = Random.Range (0,3);
			//anim.SetInteger ("AttackType_near",attacktpe);

			//anim.ResetTrigger ("Attack");
			//yield break;
		}
		else if(rand<100){
			nav.isStopped = false;
			nav.updateRotation = false;

			anim.SetFloat ("move",1.5f);
			nav.SetDestination (transform.position+transform.forward*-3.4f);
			//anim.SetBool ("guard", true);
			//Animhandlemove();
			nav.speed = walkSpeed;
			nav.isStopped=true;
			Debug.Log ("back");
			yield return new WaitForSeconds (3.0f);
			nav.SetDestination (target.transform.position);
			//yield break;
		}




		isRunning = false;



	}



	void LerpAttack(){
		transform.position=Vector3.Lerp (transform.position,target.transform.position,Time.deltaTime*2.0f);

	}





	//ダメージ処理
	public void Damage ( int damage ) {
		nav.isStopped = true;
		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Guard")) {

			anim.SetTrigger ("reflect");
		} else {
			if (_state != eState.Dead) {
				life -= damage; //体力から差し引く
				if (!anim.GetCurrentAnimatorStateInfo (0).IsName("damage")) 
				anim.SetTrigger ("damage");
				if (life <= 0) {
					//体力が0以下になった時
					life = 0;
					Dead (); //死亡処理
				}
			}
			
		}
	}

	public bool isdead=false;

	//死亡処理
	public void Dead () {

		_state = eState.Dead;
		isdead = true;
		nav.isStopped=true;
		lockobj.obj.Remove (this.gameObject);
		lockobj.nearenemys.Remove (this.gameObject);
		lockobj.target = null;
		lockobj.lockon = false;
		anim.ResetTrigger ("death");
		anim.ResetTrigger ("damage");
		anim.SetTrigger("death");
		GetComponent<DestroyTime> ().enabled = true;
		GetComponent<WeaponCollider> ().boxcoll.enabled = false;
		Destroy (this);
	}








}
