using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipObject : MonoBehaviour
{
    public float flipInterval = 5f; // Tiempo en segundos para hacer el flip
    private float timer = 0f;       // Temporizador para controlar el intervalo
    private bool isFlipped = false; // Estado de si el objeto está volteado o no

    void Update()
    {
        // Actualizamos el temporizador
        timer += Time.deltaTime;

        // Comprobamos si han pasado 5 segundos
        if (timer >= flipInterval)
        {
            // Realizamos el flip (cambiamos el estado)
            Flip();

            // Reiniciamos el temporizador
            timer = 0f;
        }
    }

    void Flip()
    {
        // Cambiar el valor de isFlipped
        isFlipped = !isFlipped;

        // Flip horizontal (cambiar el valor de la escala en X)
        Vector3 scale = transform.localScale;
        scale.x = isFlipped ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // Asegurarse de que el valor en X sea positivo
        transform.localScale = scale;
    }
}
