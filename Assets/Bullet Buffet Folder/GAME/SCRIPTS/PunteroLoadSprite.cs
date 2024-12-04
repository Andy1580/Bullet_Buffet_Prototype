using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunteroLoadSprite : MonoBehaviour
{
    private string previousSpriteName;
    private Image currentImage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger detected with {other.gameObject.name}");

        string objectName = other.gameObject.name;

        if (objectName == "CRIM" || objectName == "SKYIE" || objectName == "NOVA" || objectName == "KAI")
        {
            currentImage = other.GetComponent<Image>();

            if (currentImage != null)
            {
                Debug.Log($"Current sprite: {currentImage.sprite.name}");

                // Guardar el sprite actual
                previousSpriteName = currentImage.sprite.name;

                // Cargar y asignar el nuevo sprite desde Resources
                Sprite newSprite = Resources.Load<Sprite>($"out{objectName}");
                if (newSprite != null)
                {
                    Debug.Log($"Loaded sprite: {newSprite.name}");
                    currentImage.sprite = newSprite;
                }
                else
                {
                    Debug.LogWarning($"Sprite not found in Resources: out{objectName}");
                }
            }
            else
            {
                Debug.LogWarning($"No Image component found on {other.gameObject.name}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentImage == null || other.gameObject.name != currentImage.gameObject.name) return;

        // Restaurar el sprite anterior
        Sprite originalSprite = Resources.Load<Sprite>(previousSpriteName);
        if (originalSprite != null)
        {
            currentImage.sprite = originalSprite;
        }
        else
        {
            Debug.LogWarning($"No se pudo restaurar el sprite original: {previousSpriteName}");
        }

        currentImage = null;
    }
}
