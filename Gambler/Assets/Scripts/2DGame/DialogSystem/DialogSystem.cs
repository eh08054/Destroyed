using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private Speaker[] speakers;
    [SerializeField] private DialogData[] dialogs;
    [SerializeField] private bool isAutoStart = true;
    private bool isFirst = true;
    private int currentDialogIndex = -1;
    private int currentSpeakerIndex = 0;
    private float typingSpeed = 0.1f;
    private bool isTypingEffect = false;
    private Coroutine typingTextCoroutine;

    private void SetUp()
    {
        for(int i = 0; i < speakers.Length; i++)
        {
            SetActiveObjects(speakers[i], false);
            speakers[i].spriteRenderer.gameObject.SetActive(true);
        }
    }
    public bool UpdateDialog()
    {
        if (isFirst == true)
        {
            SetUp();
            if (isAutoStart) { SetNextDialog(); }
            isFirst = false;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(isTypingEffect == true)
            {
                isTypingEffect = false;
                if (typingTextCoroutine != null)
                {
                    StopCoroutine(typingTextCoroutine);
                }
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                speakers[currentSpeakerIndex].objectArrow.SetActive(true);
                return false;
            }
            if(dialogs.Length > currentDialogIndex + 1)
            {
                SetNextDialog();
            }
            else
            {
                GameManager.Instance.StopDialogue();
                InitDialogue();
                return true;
            }
        }
        return false;
    }
    private void SetNextDialog()
    {
        SetActiveObjects(speakers[currentSpeakerIndex], false);
        currentDialogIndex++;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        SetActiveObjects(speakers[currentSpeakerIndex], true);
        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
        typingTextCoroutine = StartCoroutine(OnTypingText());
    }
    private void SetActiveObjects(Speaker speaker, bool isVisible)
    {
        speaker.textName.gameObject.SetActive(isVisible);
        speaker.textDialogue.gameObject.SetActive(isVisible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.spriteRenderer.color;
        color.a = isVisible == true ? 1 : 0.2f;
        speaker.spriteRenderer.color = color;
    }
    private void InitDialogue()
    {
        for (int i = 0; i < speakers.Length; i++)
        {
            SetActiveObjects(speakers[i], false);
            speakers[i].spriteRenderer.gameObject.SetActive(false);
        }
        isFirst = true;
        currentDialogIndex = -1;
        currentSpeakerIndex = 0;
        typingTextCoroutine = null;
    }
    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        while(index < dialogs[currentDialogIndex].dialogue.Length)
        {
            speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTypingEffect = false;
        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }
}

[System.Serializable]
public class Speaker
{
    public Image spriteRenderer;
    public TMP_Text textName;
    public TMP_Text textDialogue;
    public GameObject objectArrow;
}

[System.Serializable]
public class DialogData
{
    public int speakerIndex;
    public string name;
    [TextArea(3, 5)]
    public string dialogue;
}
