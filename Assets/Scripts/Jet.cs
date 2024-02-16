using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jet : MonoBehaviour
{
    private bool rocketActivate = true;
    [SerializeField] private float rocketDelay;
    [SerializeField] private ParticleSystem muzzleEffect, explosionEffect, lineEffect;
    [SerializeField] private GameObject rocket;
    [SerializeField] private Transform rocket1Pos, rocket2Pos;
    private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound, rocketSound;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && rocketActivate)
        {
            StartCoroutine(Rocket());
        }
    }
    private IEnumerator Rocket()
    {
        audioSource.PlayOneShot(rocketSound);
        muzzleEffect.Play();
        Transform rocket1 = Instantiate(rocket, rocket1Pos.position, Quaternion.identity).transform;
        Transform rocket2 = Instantiate(rocket, rocket2Pos.position, Quaternion.identity).transform;
        rocket1.eulerAngles = transform.eulerAngles; 
        rocket2.eulerAngles = transform.eulerAngles;
        Destroy(rocket1.gameObject, 6f);
        Destroy(rocket2.gameObject, 6f);
        rocketActivate = false;
        yield return new WaitForSeconds(rocketDelay);
        rocketActivate = true;
    }
    public void Crash()
    {
        GameManager.instance.Lose();
        audioSource.PlayOneShot(explosionSound);
        lineEffect.Stop();
        explosionEffect.Play();
        rocketActivate = false;
        StopAllCoroutines();
    }
}
