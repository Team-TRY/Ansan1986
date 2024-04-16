using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Chair : MonoBehaviour
{
    public Transform sittingPos;
    public TMP_Text interactionText;
    public GameObject player;
    
    private Vector3 playerScale;
    private DynamicMoveProvider dynamicMoveProvider;
    private bool isPlayerInRange = false;
    private bool isPlayerSitting = false;

    private void Awake()
    {
        dynamicMoveProvider = player.GetComponent<DynamicMoveProvider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = "Press [E] to Sit";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPlayerSitting)
            {
                SitOnChair();
                isPlayerSitting = true;
                interactionText.text = "Press [E] to Stand Up";
            }
            else
            {
                Standing();
                isPlayerSitting = false;
                interactionText.text = "Press [E] to Sit";
            }
        }
    }
    
    private void SitOnChair()
    {
        player.transform.position = sittingPos.position;
        player.transform.localScale *= 0.5f;
        
        if (dynamicMoveProvider != null)
        {
            dynamicMoveProvider.moveSpeed = 0f;
        }
        
        Debug.Log("Player is now sitting on the chair.");
    }
    
    private void Standing()
    {
        player.transform.localScale = playerScale;
        
        if (dynamicMoveProvider != null)
        {
            dynamicMoveProvider.moveSpeed = 1f;
        }
        
        Debug.Log("Player is now standing.");
    }
}
