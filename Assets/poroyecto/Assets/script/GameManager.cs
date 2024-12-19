using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverImage; // Imagen para Game Over
    public GameObject winImage;      // Imagen para Victoria
    private int score = 0;
    private int missedNotes = 0;
    private int maxMissedNotes = 10; // Límite de fallos antes del Game Over
    private int maxScore = 20;       // Puntaje para ganar el juego

    // Singleton para acceder desde otros scripts
    public static GameManager instance;

    void Awake()
    {
        // Asegurarnos de que el GameManager sea único
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);  // Si ya existe una instancia, destruimos este objeto
        }
    }

    void Start()
    {
        // Desactivar ambas imágenes al inicio
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }
        if (winImage != null)
        {
            winImage.SetActive(false);
        }
    }

    void Update()
    {
        // Verificar si el juego ha terminado en derrota
        if (missedNotes >= maxMissedNotes)
        {
            GameOver();
        }

        // Verificar si el jugador ha ganado
        if (score >= maxScore)
        {
            Win();
        }
    }

    // Método para aumentar el puntaje cuando una nota es presionada correctamente
    public void IncreaseScore()
    {
        score++;
    }

    // Método para aumentar el número de fallos cuando una nota no es presionada
    public void MissNote()
    {
        missedNotes++;
    }

    // Método que maneja el estado de Game Over
    private void GameOver()
    {
        if (winImage != null)
        {
            winImage.SetActive(false);  // Asegurar que la imagen de victoria esté desactivada
        }

        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);  // Activar la imagen de Game Over
        }

        Time.timeScale = 0;  // Detener el juego
    }

    // Método que maneja el estado de Victoria
    private void Win()
    {
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);  // Asegurar que la imagen de Game Over esté desactivada
        }

        if (winImage != null)
        {
            winImage.SetActive(true);  // Activar la imagen de Victoria
        }

        Time.timeScale = 0;  // Detener el juego
    }

    // Método para reiniciar el juego
    public void RestartGame()
    {
        Time.timeScale = 1; // Restablecer el tiempo
        missedNotes = 0;    // Reiniciar los fallos
        score = 0;          // Reiniciar el puntaje

        // Desactivar ambas imágenes al reiniciar
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }
        if (winImage != null)
        {
            winImage.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Volver a cargar la escena
    }

    // Método para salir del juego
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
