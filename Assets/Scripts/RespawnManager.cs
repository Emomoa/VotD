using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public Transform respawnPoint;
    public GameObject player;
    public float respawnDelay = 0.5f;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerDeath += HandleRespawnPlayer;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= HandleRespawnPlayer;
    }

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        playerMovement = player.GetComponent<PlayerMovement>();
        characterController = player.GetComponent<CharacterController>();
    }

    private void Start()
    {
        if (respawnPoint == null)
        {
            Debug.LogError("RespawnPoint �r inte tilldelad i Inspektorn.");
        }
    }

    private void HandleRespawnPlayer()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private IEnumerator RespawnPlayer()
    {
        
        yield return new WaitForSeconds(respawnDelay);
        
        player.gameObject.SetActive(true);
        
        player.transform.position = respawnPoint.position;
        player.transform.rotation = respawnPoint.rotation;


        playerMovement.isDead = false;
        playerMovement.LoadParameters();
        
        Debug.Log("Spelaren har respawnat.");
        
    }
}
