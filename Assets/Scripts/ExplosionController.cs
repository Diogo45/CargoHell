using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
      public int explosionTime; // set it in inspector
     
     private void Start() {
        Invoke("DestroyMe", explosionTime); // shedules derived call 
     }
     
     private void DestroyMe() {
        Destroy(gameObject);
     }

}
