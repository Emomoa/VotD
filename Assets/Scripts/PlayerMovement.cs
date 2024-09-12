using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool IsSneaking = false;
    public float rayDistance = 10f; // Avst�nd f�r raycast

    [SerializeField]
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Hantera smygning
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sneak();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopSneaking();
        }

        // H�mta input fr�n tangentbordet
        float moveX = Input.GetAxis("Horizontal"); // V�nster och h�ger
        float moveZ = Input.GetAxis("Vertical");   // Fram�t och bak�t

        // Skapa en r�relsevektor i v�rldsrummet
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Om vi har n�gon r�relseinput, uppdatera spelarens rotation och position
        if (move != Vector3.zero)
        {
            // Flytta spelaren
            controller.Move(move.normalized * speed * Time.deltaTime); // Normaliserar r�relsevektorn f�r j�mn hastighet
        }

        // Skjut en raycast fr�n spelaren i blickriktningen
        RaycastHit hit;
        Vector3 rayDirection = transform.forward; // Skjut rayen i fram�triktningen
        if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance))
        {
            Debug.Log("Tittar p�: " + hit.collider.name); // Debug f�r att se vad vi tr�ffar
        }

        // Rita en raycast f�r att visa riktningen
        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);
    }

    void Sneak()
    {
        IsSneaking = true;
        speed = 2.5f;
    }

    void StopSneaking()
    {
        IsSneaking = false;
        speed = 5f; // �terst�ller hastigheten till normal
    }
}
