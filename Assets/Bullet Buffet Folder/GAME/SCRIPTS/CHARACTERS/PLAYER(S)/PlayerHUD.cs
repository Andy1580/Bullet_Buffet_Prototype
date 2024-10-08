using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image healthBar;
    public Image abilityBar;
    public Image dashIcon;
    public Image shieldIcon;
    public Image invulnerabilityIcon;
    public Image speedIcon;
    public Image coneShotIcon;
    public Image explosiveShotIcon;
    public Image characterIcon;
    public TMP_Text dashCounter;
    public TMP_Text shieldCounter;
    public string name;

    //public void SetupHUD(InfoLobby.PlayerInfo playerInfo)
    //{
    //    // Configurar la información del HUD según el playerInfo
    //    healthBar.fillAmount = 1;
    //    abilityBar.fillAmount = 1;
    //    dashIcon.enabled = true;
    //    shieldIcon.enabled = true;
    //    invulnerabilityIcon.enabled = false;
    //    speedIcon.enabled = false;
    //    coneShotIcon.enabled = true;
    //    explosiveShotIcon.enabled = true;
    //    characterIcon.sprite = GetCharacterSprite(playerInfo.personaje);

    //    dashCounter.text = "5";  // Inicializar el contador de dash
    //    shieldCounter.text = "5";  // Inicializar el contador de escudo
    //}



    private void FixedUpdate()
    {
        characterIcon.sprite = GetCharacterSprite(name);
    }

    public float BarraDeVida
    {
        set
        {
            healthBar.fillAmount = value;
        }
    }

    public float BarraDeHabilidad
    {
        set => abilityBar.fillAmount = value;

    }

    public string Name
    {
        set => name = value;
    }

    public bool DashIcon
    {
        set => dashIcon.enabled = value;
    }
    //public void UpdateDashStatus(bool isActive, float count)
    //{
    //    dashIcon.enabled = isActive;
    //    dashCounter.text = count.ToString();
    //}

    //public void UpdateDashCounter(int dashCount)
    //{
    //    dashCounter.text = dashCount.ToString();
    //}

    //public void UpdateShieldStatus(bool isActive, int count)
    //{
    //    shieldIcon.enabled = isActive;
    //    shieldCounter.text = count.ToString();
    //}

    //public void UpdateShieldCounter(int shieldCount)
    //{
    //    shieldCounter.text = shieldCount.ToString();
    //}

    //public void EnableSuperShootIcon()
    //{
    //    // Lógica para habilitar el icono de Super Shoot
    //    coneShotIcon.enabled = true;
    //}

    //public void DisableSuperShootIcon()
    //{
    //    // Lógica para habilitar el icono de Super Shoot
    //    coneShotIcon.enabled = false;
    //}

    //public void EnableExplosiveBulletIcon()
    //{
    //    // Lógica para habilitar el icono de Explosive Bullet
    //    explosiveShotIcon.enabled = true;
    //}

    //public void DisableExplosiveBulletIcon()
    //{
    //    // Lógica para habilitar el icono de Super Shoot
    //    explosiveShotIcon.enabled = false;
    //}

    public void EnablePowerUpIcon(string powerUp)
    {
        switch (powerUp)
        {
            case "Invulnerability":
                invulnerabilityIcon.enabled = true;
                break;
            case "Super Speed":
                speedIcon.enabled = true;
                break;
            default:
                break;
        }
    }

    public void DisablePowerUpIcons()
    {
        speedIcon.enabled = false;
        invulnerabilityIcon.enabled = false;
    }

    private Sprite GetCharacterSprite(string characterName)
    {
        // Aquí puedes cargar el sprite correspondiente según el nombre del personaje
        // Por ejemplo, usando un Resource.Load o un diccionario preconfigurado
        switch (characterName)
        {
            case "CRIM":
                return Resources.Load<Sprite>("CRIM");
            case "KAI":
                return Resources.Load<Sprite>("KAI");
            case "NOVA":
                return Resources.Load<Sprite>("NOVA");
            case "SKYIE":
                return Resources.Load<Sprite>("SKYIE");
            default:
                return null;
        }
    }
}
