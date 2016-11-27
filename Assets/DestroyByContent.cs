using UnityEngine;
using System.Collections;

public class DestroyByContent : MonoBehaviour {

    // Use this for initialization
    void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        if (other.tag == "Boundary") {
            return;        }
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
