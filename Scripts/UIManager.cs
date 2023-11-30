using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text currentTurnText;
    public GameObject foul;
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTurn(int turn)
    {
        currentTurnText.text = $"Player {turn}'s turn";
    }

    public void Foul()
    {
        foul.SetActive(true);
        StartCoroutine(FoulCoroutine());
    }

    IEnumerator FoulCoroutine()
    {
        yield return new WaitForSeconds(1);
        foul.SetActive(false);
    }
}
