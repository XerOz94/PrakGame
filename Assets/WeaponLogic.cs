using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] Camera ShootCamera;
    [SerializeField] float range = 1000f;
    public ParticleSystem MuzzleFlash;
    public GameObject HitEffect;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            MuzzleFlash.Play();
            Shoot();
        }
    }
     private void Shoot(){
        RaycastHit hit;
        if(Physics.Raycast(ShootCamera.transform.position, ShootCamera.transform.forward, out hit, range)){
            CreateHitImpact(hit);
            Debug.Log("I hit this thing: "+hit.transform.name);
            if (hit.transform.tag.Equals("Enemy")){
                EnemyLogic target = hit.transform.GetComponent<EnemyLogic>();
                target.TakeDamage(50);
            }
        } else{
            return;
        }
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Vector3 direction = ShootCamera.transform.TransformDirection(Vector3.forward)*range;
        Gizmos.DrawRay(ShootCamera.transform.position, direction);
    }
    private void CreateHitImpact(RaycastHit hit){
        GameObject impact = Instantiate(HitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, .1f);
    }
   
}
