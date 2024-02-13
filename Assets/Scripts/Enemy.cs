using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    private Animator anim;
    private AudioSource audioSource;

    bool death = false;
    public GameObject Player;

    [Range(0.0f, 1.0f)]
    public float HitAccuracy = 0.5f;
    public int heal = 100;

    public AudioClip shootSoundEffect;
    public ParticleSystem muzzleEffect;

    private Transform enemyTransform;
    public Transform raycastStartingPos;
    private IEnumerator Start()
    {
        enemyTransform = transform.GetChild(0);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        while (true)
        {
            yield return null;
            Vector3 directionToPlayer = Player.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(raycastStartingPos.position, directionToPlayer, out hit))
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                    Sight();
                else
                    NoSight();
            }
        }
    }

    public void Sight()
    {
        anim.SetBool("shoot", true);
        enemyTransform.LookAt(Player.transform, Vector3.up);
    }
    public void NoSight()
    {
        anim.SetBool("shoot", false);
    }

    public void ShootEvent()
    {
        muzzleEffect.Play();
        audioSource.PlayOneShot(shootSoundEffect);
        float random = Random.Range(0.0f, 1.0f);

        bool isHit = random > 1.0f - HitAccuracy;

        if (isHit)
        {
            Player.GetComponent<Player>().Damage(5);
        }
    }
    public void Damage(int damage)
    {
        heal -= damage;
        if(heal <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        if(!death)
        {
            GameManager.instance.EnemyDestroyed();
            death = true;
            anim.SetTrigger("death");
            StopAllCoroutines();
        }
    }
}
