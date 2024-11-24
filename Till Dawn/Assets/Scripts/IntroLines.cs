using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class IntroLines : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    [SerializeField] private int index;
 
    // Start is called before the first frame update
    void Start()
    {
        dialogText.text = string.Empty;
        StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogText.text == lines[index])
        {
            NextLine();
        }


        if (Input.GetMouseButtonDown(0))
        {
            if(dialogText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogText.text = lines[index];
            }
        }

    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeDialogs());
    }
    IEnumerator TypeDialogs()
    {
        foreach(char item in lines[index].ToCharArray())
        {
            dialogText.text += item;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogText.text = string.Empty;
            StartCoroutine(TypeDialogs());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
