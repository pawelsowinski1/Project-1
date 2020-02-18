using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCore : MonoBehaviour
{
    public GameObject myLight; // reference to the object containing light component
    //public GameObject myParticleSystem; // reference to the object containing particle system component <-- is it neccessary?

    public float size; // size of the fire

    IEnumerator FireUpdate()
    {
        for (;;)
        {
            // particles
            
            ParticleSystem ps = GetComponent<ParticleSystem>();

            ps.gravityModifier = -1f - size*0.25f;
            ps.startSize = 0.2f + size*0.1f;
            ps.emissionRate = 5f + size*3f;

            ParticleSystem.ShapeModule pssm = ps.shape;

            pssm.radius = 0.05f + size*0.002f;
            

            // light

            myLight.transform.position = new Vector3(transform.position.x, transform.position.y, -1f *size/2);

            myLight.GetComponent<Light>().intensity = size;
            myLight.GetComponent<Light>().range = 5f + size*1.5f;

            
            // add flickering

            float r = Random.Range(0.8f,1f);

            //myLight.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z*r);
            myLight.GetComponent<Light>().intensity *= r;
            myLight.GetComponent<Light>().range *= r;
           
            yield return new WaitForSeconds(0.05f);
        }
    }

    void Start()
    {
        StartCoroutine("FireUpdate");
    }

    void Destroy()
    {
        
    }

}
