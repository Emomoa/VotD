using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool IsSneaking = false;
    public float rayDistance = 10f; // Avstånd för raycast

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

        // Hämta input från tangentbordet
        float moveX = Input.GetAxis("Horizontal"); // Vänster och höger
        float moveZ = Input.GetAxis("Vertical");   // Framåt och bakåt

        // Skapa en rörelsevektor i världsrummet
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Om vi har någon rörelseinput, uppdatera spelarens rotation och position
        if (move != Vector3.zero)
        {
            // Flytta spelaren
            controller.Move(move.normalized * speed * Time.deltaTime); // Normaliserar rörelsevektorn för jämn hastighet
        }

        // Skjut en raycast från spelaren i blickriktningen
        RaycastHit hit;
        Vector3 rayDirection = transform.forward; // Skjut rayen i framåtriktningen
        if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance))
        {
            Debug.Log("Tittar på: " + hit.collider.name); // Debug för att se vad vi träffar
        }

        // Rita en raycast för att visa riktningen
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
        speed = 5f; // Återställer hastigheten till normal
    }
}
