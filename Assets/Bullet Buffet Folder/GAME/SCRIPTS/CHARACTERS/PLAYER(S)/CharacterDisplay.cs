using UnityEngine;

public class CharacterDisplay : MonoBehaviour
{
    [SerializeField] private CharacterData character;

    private GameObject skin;
    private GameObject handgunWeapon;
    private GameObject meeleWeapon;

    public int healthPlayer;

    private void Start()
    {
        //character = GetComponent<CharacterData>();
        healthPlayer = character.health;
        skin = character._skin;
        handgunWeapon = character._handgunWeapon;
        meeleWeapon = character._meeleWeapon;

        Instantiate(skin);
    }

    private void Update()
    {
        if(healthPlayer == 0)
        {
            Destroy(gameObject);

            
        }
    }
}
