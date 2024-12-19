using UnityEngine;

public class LaneController : MonoBehaviour
{
    public NoteSyncGame2 gameManager; // Referencia al script NoteSyncGame2


    void Start()
    {
        // Si no se asigna en el Inspector, buscar el script NoteSyncGame2 en la escena
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<NoteSyncGame2>();
            if (gameManager == null)
            {
                Debug.LogError("NoteSyncGame2 no encontrado en la escena. Asegúrate de que existe un GameObject con este script.");
            }
        }
    }

    void Update()
    {
        // Comprobar si la tecla correspondiente ha sido presionada
        if (Input.GetKeyDown(KeyCode.D)) gameManager.CheckNote(0); // Lane 0 -> D
        if (Input.GetKeyDown(KeyCode.F)) gameManager.CheckNote(1); // Lane 1 -> F
        if (Input.GetKeyDown(KeyCode.J)) gameManager.CheckNote(2); // Lane 2 -> J
        if (Input.GetKeyDown(KeyCode.K)) gameManager.CheckNote(3); // Lane 3 -> K
    }
}
