using UnityEngine;
using System.Collections;

public class ParticleSortingLayerFIx : MonoBehaviour 
{
    void Start () 
    {
	    GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Player";
	    GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = -1;
    }
}
