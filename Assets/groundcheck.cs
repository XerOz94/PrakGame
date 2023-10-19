using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundcheck : MonoBehaviour
{
    PlayerLogic logicmovement;
    private void Start(){
        logicmovement = this.GetComponentInParent<PlayerLogic>();
    }
    private void OnTriggerEnter(Collider other){
       logicmovement.groundchanger();
    }
}
