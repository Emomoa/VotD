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
        //n�r QTE triggas ska spelaren sl� i riktningen sp�ket kommer ifr�n
    }

    private void LightWaypoint() {
        //n�r spelaren g�r f�rbi ot�nd fackla p� v�gg t�nds den och ger ljudclue
    }
}
