using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteSyncGame2 : MonoBehaviour
{
    public GameObject notePrefab;       // Prefab de la nota
    public Transform[] lanes;           // Posiciones de las "lanes"
    public Sprite[] noteSprites;        // Array de sprites para las notas
    public float noteSpeed = 5f;        // Velocidad de caída
    public float spawnInterval = 0.8f;  // Intervalo entre spawns
    public TMP_Text scoreText;
    public TMP_Text missedText;
    public TMP_Text gameOverText;
    public GameObject gameOverImage;    // Imagen para Game Over
    public GameObject winImage;         // Imagen para Victoria

    public Vector2 detectionAreaCenter; // Centro del área de detección
    public float detectionRadius = 1f;  // Radio del área de detección

    private int score = 0;
    private int missedNotes = 0;
    private int maxMissedNotes = 10;
    private int maxScore = 20;          // Puntaje necesario para ganar
    private List<GameObject> notes = new List<GameObject>(); // Lista de notas activas

    private float spawnTimer = 0f;  // Controla el spawn

    void Start()
    {
        // Desactivar las imágenes de Game Over y Victoria al inicio
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }
        if (winImage != null)
        {
            winImage.SetActive(false);
        }

        // Desactivar el texto de Game Over
        gameOverText.gameObject.SetActive(false);

        // Verificar si el prefab está asignado
        if (notePrefab == null)
        {
            Debug.LogError("notePrefab no está asignado en el Inspector.");
        }
    }

    void Update()
    {
        // Spawning de notas
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnNote();
            spawnTimer = 0f;
        }

        // Mover las notas y verificar si se salen de la pantalla
        for (int i = notes.Count - 1; i >= 0; i--) // Iterar desde el final
        {
            GameObject note = notes[i];
            if (note != null)
            {
                note.transform.Translate(Vector3.down * noteSpeed * Time.deltaTime);

                // Si la nota se sale de la pantalla
                if (note.transform.position.y < -6f)
                {
                    Debug.Log("Nota perdida: " + note.name);
                    MissNote(note, i);  // Aquí solo eliminamos la nota sin contar como fallo
                }
            }
        }

        // Verificar input para cada tecla y lanzar el OverlapCircle
        if (Input.GetKeyDown(KeyCode.D)) CheckNote(0); // Lane 0 -> D
        if (Input.GetKeyDown(KeyCode.F)) CheckNote(1); // Lane 1 -> F
        if (Input.GetKeyDown(KeyCode.J)) CheckNote(2); // Lane 2 -> J
        if (Input.GetKeyDown(KeyCode.K)) CheckNote(3); // Lane 3 -> K

        // Actualización de UI
        scoreText.text = $"Score: {score}";
        missedText.text = $"Missed: {missedNotes}/{maxMissedNotes}";

        // Verificar Game Over
        if (missedNotes >= maxMissedNotes)
        {
            GameOver();
        }

        // Verificar Victoria
        if (score >= maxScore)
        {
            Win();
        }
    }

    void SpawnNote()
    {
        // Validación para asegurarse de que notePrefab es válido
        if (notePrefab == null)
        {
            Debug.LogError("notePrefab no está asignado o fue destruido.");
            return;
        }

        int laneIndex = Random.Range(0, lanes.Length); // Selección aleatoria de lane
        GameObject note = Instantiate(notePrefab, lanes[laneIndex].position, Quaternion.identity);
        note.name = "Note_Lane_" + laneIndex; // Asignar nombre para depuración

        // Asignar un sprite aleatorio a la nota
        if (noteSprites.Length > 0)
        {
            Sprite randomSprite = noteSprites[Random.Range(0, noteSprites.Length)];
            note.GetComponent<SpriteRenderer>().sprite = randomSprite;
        }

        // Asegurarse de que la nota tenga un Collider2D
        if (note.GetComponent<Collider2D>() == null)
        {
            note.AddComponent<BoxCollider2D>(); // Añadir Collider2D si no tiene uno
        }

        notes.Add(note); // Agregar a la lista
        Debug.Log("Nota instanciada: " + note.name);
    }

    public void CheckNote(int laneIndex)
    {
        // Obtenemos la posición de la tecla correspondiente (lane)
        Vector2 keyPosition = lanes[laneIndex].position;

        // Lanza el OverlapCircle desde la posición de la tecla para detectar notas
        Collider2D[] hits = Physics2D.OverlapCircleAll(keyPosition, detectionRadius, LayerMask.GetMask("Note"));

        // Verificar si detectamos alguna nota dentro del círculo
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit != null && hit.CompareTag("Note"))
                {
                    GameObject note = hit.gameObject;
                    if (note != null)
                    {
                        // Si la nota está dentro del área de detección, el jugador ha acertado
                        Debug.Log("Nota acertada: " + note.name);

                        // Destruir la nota y removerla de la lista
                        Destroy(note);
                        notes.Remove(note);

                        // Incrementar el puntaje
                        score++;

                        // Actualizamos el puntaje en la UI
                        scoreText.text = $"Score: {score}";
                    }
                }
            }
        }
        else
        {
            // Si no se detecta una nota, es un fallo
            Debug.Log("No hay notas detectadas en la lane: " + laneIndex);
            missedNotes++;
        }
    }

    void MissNote(GameObject note, int index)
    {
        if (note != null)
        {
            Debug.Log("Destruyendo nota perdida: " + note.name);
            Destroy(note);
            notes.RemoveAt(index);
            // Aquí no incrementamos missedNotes
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");

        // Activar imagen de Game Over
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
        }

        // Desactivar imagen de Victoria (por si estaba activa)
        if (winImage != null)
        {
            winImage.SetActive(false);
        }

        // Mostrar el texto de Game Over
        gameOverText.gameObject.SetActive(true);

        Time.timeScale = 0; // Pausar el juego
    }

    void Win()
    {
        Debug.Log("VICTORIA");

        // Activar imagen de Victoria
        if (winImage != null)
        {
            winImage.SetActive(true);
        }

        // Desactivar imagen de Game Over (por si estaba activa)
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }

        Time.timeScale = 0; // Pausar el juego
    }

    // Método para detectar cuando una nota abandona el trigger del área de detección
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            // Buscar y eliminar la nota de la lista
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i] == other.gameObject)
                {
                    // Asegúrate de que solo se destruye una vez
                    if (notes[i] != null)
                    {
                        Debug.Log("Nota abandonó el trigger: " + other.name);

                        // Eliminar la nota de la lista y destruirla
                        Destroy(notes[i]);  // Destruir la nota
                        notes.RemoveAt(i); // Remover de la lista

                        // No contamos esto como fallo
                    }
                    break;  // Salir del bucle una vez que se destruya la nota
                }
            }
        }
    }
}
