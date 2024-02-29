using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Player player;

 
    public Transform gameoverCamPos;
    public Transform enemySpine;

   
    protected void Awake()
    {
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

  
  
   
    
 

}
