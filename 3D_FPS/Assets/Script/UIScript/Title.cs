using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    public string sceneName = "GameStage";

    public static Title instance;

    private SaveNLoad theSaveNLoad;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("�ε�");
        SceneManager.LoadScene(sceneName);
    }
    public void ClickLoad()
    {
        Debug.Log("�ε�");
        StartCoroutine(LoadCoroutine());
       
    }
    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
        theSaveNLoad = FindObjectOfType<SaveNLoad>(); //���� ������ �Ѿ�� ã��
        theSaveNLoad.LoadData();//�̱����̱� ������ �����
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }


}
