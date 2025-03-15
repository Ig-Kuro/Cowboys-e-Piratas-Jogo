using System;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class SabrePirata : Arma
{
    public LayerMask enemyLayer;
    public GameObject player;
    public Vector3 boxHalfSize;
    public override void Action()
    {
        //os inimigos estão separados na outra branch, então se eu tentasse dar dano neles aqui daria erro, mas eles tem uma função de tomar dano.
        if(Physics.BoxCast(transform.position,boxHalfSize,transform.forward,Quaternion.identity,enemyLayer)&& player.GetComponent<Pirata>().canAttack==true)
        {
            Debug.Log("EnemyHit");
        }
    }
}
