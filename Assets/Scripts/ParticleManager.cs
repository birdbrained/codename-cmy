using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;
    public static ParticleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ParticleManager>();
            }
            return instance;
        }
    }

    public GameObject CritParticle;
    public GameObject ResistParticle;
    public GameObject WhiffParticle;
    public GameObject DownParticle;
    public GameObject UpParticle;
    public GameObject GameOverParticle;
    public GameObject DyingParticles;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
