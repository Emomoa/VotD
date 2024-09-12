using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] bool isLit;
    [SerializeField] AudioSource torchSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Hit() {
        //när QTE triggas ska spelaren slå i riktningen spöket kommer ifrån
    }

    private void LightWaypoint() {
        //när spelaren går förbi otänd fackla på vägg tänds den och ger ljudclue
    }
}
