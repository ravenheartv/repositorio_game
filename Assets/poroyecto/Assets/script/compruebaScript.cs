using System.Collections.Generic;
using UnityEngine;

public class compruebaScript : MonoBehaviour
{
    public Transform[] lanes;  // Las posiciones de las teclas (lanes) en el juego
    public float detectionRadius = 1f;  // El radio del área de detección
    public GameObject notePrefab;       // Prefab de la nota
    public Sprite[] noteSprites;        // Array de sprites para las notas
    public float spawnInterval = 0.8f;  // Intervalo entre spawns
    public float defaultNoteSpeed = 5f; // Velocidad normal de las notas
    public float slowNoteSpeed = 2f;    // Velocidad reducida
    public float slowDuration = 10f;    // Duración del efecto lento en segundos

    private float spawnTimer = 0f;      // Controla el tiempo entre spawns
    private List<GameObject> notes = new List<GameObject>(); // Lista de notas activas
    private bool isSlowed = false;      // Indica si las notas están ralentizadas
    private float slowTimer = 0f;       // Temporizador para el efecto lento

    void Update()
    {
        // Controlar el efecto lento
        if (isSlowed)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer >= slowDuration)
            {
                isSlowed = false;
                slowTimer = 0f;
                Debug.Log("Velocidad de las notas restaurada.");
            }
        }

        // Verificar si se pulsa la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space) && !isSlowed)
        {
            isSlowed = true;
            slowTimer = 0f;
            Debug.Log("Velocidad de las notas reducida.");
        }

        // Verificar la entrada del jugador para cada tecla
        if (Input.GetKeyDown(KeyCode.D)) DetectNoteInLane(0, true);  // Lane 0 -> D
        if (Input.GetKeyDown(KeyCode.F)) DetectNoteInLane(1, true);  // Lane 1 -> F
        if (Input.GetKeyDown(KeyCode.J)) DetectNoteInLane(2, true);  // Lane 2 -> J
        if (Input.GetKeyDown(KeyCode.K)) DetectNoteInLane(3, true);  // Lane 3 -> K

        // Detectar las notas en cada lane sin presionar teclas
        DetectNoteInLane(0, false);  // Lane 0
        DetectNoteInLane(1, false);  // Lane 1
        DetectNoteInLane(2, false);  // Lane 2
        DetectNoteInLane(3, false);  // Lane 3

        // Spawning de notas
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnNote();
            spawnTimer = 0f;
        }

        // Mover las notas
        foreach (var note in notes)
        {
            if (note != null)
            {
                float speed = isSlowed ? slowNoteSpeed : defaultNoteSpeed;
                note.transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
        }
    }

    void SpawnNote()
    {
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

        // Asegurarse de que la nota está en la capa "Note"
        note.layer = LayerMask.NameToLayer("Note");

        notes.Add(note); // Agregar a la lista
        Debug.Log("Nota instanciada: " + note.name);
    }

    void DetectNoteInLane(int laneIndex, bool playerPressed)
    {
        if (laneIndex < 0 || laneIndex >= lanes.Length)
        {
            Debug.LogError("Índice de lane fuera de rango: " + laneIndex);
            return;
        }

        Vector2 keyPosition = lanes[laneIndex].position;
        Debug.DrawLine(keyPosition, keyPosition + Vector2.up * detectionRadius, Color.red, 0.1f);
        Collider2D[] hitNotes = Physics2D.OverlapCircleAll(keyPosition, detectionRadius, LayerMask.GetMask("Note"));

        if (hitNotes.Length > 0)
        {
            foreach (var hit in hitNotes)
            {
                if (hit != null && hit.CompareTag("Note"))
                {
                    if (playerPressed)
                    {
                        Debug.Log("¡Nota acertada en lane " + laneIndex + ": " + hit.gameObject.name);
                        Destroy(hit.gameObject);
                    }
                    else
                    {
                        Debug.Log("Nota en el área de lane " + laneIndex + ": " + hit.gameObject.name);
                    }
                }
            }
        }
        else
        {
            if (playerPressed)
            {
                Debug.Log("No hay notas en el área de lane " + laneIndex + " tras pulsar la tecla.");
            }
        }
    }
}
