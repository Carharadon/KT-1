using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilitiesManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Abilities Buttons")]
    [SerializeField] private Button ability1Button;
    [SerializeField] private Button ability2Button;
    [SerializeField] private Button ability3Button;

    [Header("Cooldown Times")]
    [SerializeField] private float cooldown1 = 2f;
    [SerializeField] private float cooldown2 = 4f;
    [SerializeField] private float cooldown3 = 6f;

    private Vector3 originalScale;

    void Start()
    {
        if (playerSprite == null)
            playerSprite = GetComponent<SpriteRenderer>();

        originalScale = playerSprite.transform.localScale;

        ability1Button.onClick.AddListener(UseAbility1);
        ability2Button.onClick.AddListener(UseAbility2);
        ability3Button.onClick.AddListener(UseAbility3);
    }

    public void UseAbility1()
    {
        playerSprite.color = Color.red;
        StartCoroutine(CooldownRoutine(ability1Button, cooldown1, ResetColor));
    }

    public void UseAbility2()
    {
        playerSprite.color = Color.blue;
        StartCoroutine(CooldownRoutine(ability2Button, cooldown2, ResetColor));
    }

    public void UseAbility3()
    {
        playerSprite.transform.localScale = originalScale * 1.5f;
        StartCoroutine(CooldownRoutine(ability3Button, cooldown3, ResetScale));
    }

    private IEnumerator CooldownRoutine(Button button, float cooldownTime, System.Action resetAction)
    {
        button.interactable = false; 

        yield return new WaitForSeconds(cooldownTime); 

        resetAction.Invoke(); 
        button.interactable = true; 
    }

    private void ResetColor()
    {
        playerSprite.color = Color.white;
    }

    private void ResetScale()
    {
        playerSprite.transform.localScale = originalScale;
    }
}
