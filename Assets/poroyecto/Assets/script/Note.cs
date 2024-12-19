using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 5f;
    public int laneIndex; // Guardar el índice de la lane a la que esta nota pertenece

    // Método para mover la nota
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Si la nota llega al final sin haber sido presionada, solo contar el fallo
        if (transform.position.y < -6f)
        {
            GameManager.instance.MissNote(); // Llamar al método MissNote desde el controlador principal (GameManager)
            Destroy(gameObject); // Aquí destruimos la nota al final, sin afectar a las demás
        }

        // Verificar si la nota fue presionada correctamente
        CheckIfPressed();
    }

    // Método para verificar si la nota es correctamente presionada
    public void CheckIfPressed()
    {
        // Lógica para verificar si la tecla correcta fue presionada (teclas D, F, J, K)
        if (Input.GetKeyDown(KeyCode.D) && laneIndex == 0) // Si es la lane 0 y presionas D
        {
            Destroy(gameObject); // Nota destruida correctamente
            GameManager.instance.IncreaseScore(); // Aumentar puntaje
        }
        else if (Input.GetKeyDown(KeyCode.F) && laneIndex == 1) // Si es la lane 1 y presionas F
        {
            Destroy(gameObject);
            GameManager.instance.IncreaseScore();
        }
        else if (Input.GetKeyDown(KeyCode.J) && laneIndex == 2) // Si es la lane 2 y presionas J
        {
            Destroy(gameObject);
            GameManager.instance.IncreaseScore();
        }
        else if (Input.GetKeyDown(KeyCode.K) && laneIndex == 3) // Si es la lane 3 y presionas K
        {
            Destroy(gameObject);
            GameManager.instance.IncreaseScore();
        }
    }
}
