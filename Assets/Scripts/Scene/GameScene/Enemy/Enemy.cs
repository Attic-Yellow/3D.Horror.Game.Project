using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Player player;

    public AudioClip overClip;
    public Transform gameoverCamPos;
    public Transform gameoverLookAt;
    public float walkSpeed;

    protected void Awake()
    {
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
