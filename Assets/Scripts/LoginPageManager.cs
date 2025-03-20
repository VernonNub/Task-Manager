using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

public class LoginPageManager : MonoBehaviour
{
    private List<string> adminUsernames = new List<string> { "Gary" };
    private List<string> adminPasswords = new List<string> { "200377" };

    [Header("UI_Elements")]
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button loginButton;
    [SerializeField] Button loginAsWorker;
    [SerializeField] Button saveButton;

    [Header("Pages")]
    public GameObject adminPage;
    public GameObject workerPage;

    [Header("Pop-up")]
    public GameObject errorPOPUP;

    private async void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
        loginAsWorker.onClick.AddListener(OnWorkerButtonClick);
        saveButton.onClick.AddListener(SaveData);
    }

    private void OnLoginButtonClick()
    {
        if(adminUsernames.Contains(usernameInput.text))
        {
            if (passwordInput.text == adminPasswords[adminUsernames.IndexOf(usernameInput.text)])
            {
                AccountManager.instance.isAdmin = true;
                adminPage.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                errorPOPUP.SetActive(true);
            }
        }
        else
        {
            errorPOPUP.SetActive(true);
        }
    }
    
    private void OnWorkerButtonClick()
    {
        workerPage.SetActive(true);
        gameObject.SetActive(false);
    }

    private async void SaveData()
    {
        var userData = new Dictionary<string, object>
        {
            {"isAdmin", AccountManager.instance.isAdmin},
            {"NameList", AccountManager.instance.nameList},
            {"Version", AccountManager.instance.versionInfo}
        };

        await CloudSaveService.Instance.Data.Player.SaveAsync(userData, new SaveOptions(new PublicWriteAccessClassOptions()));
        Debug.Log($"Saved data {string.Join(',', userData)}");
    }
}
