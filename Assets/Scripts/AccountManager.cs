using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor.Networking.PlayerConnection;

public class AccountManager : MonoBehaviour
{
    public static AccountManager instance;

    [Header("DataBase")]
    public int versionInfo = 0;
    public List<string> nameList = new List<string>();

    [Header("Account Details")]
    public string name = "";
    public bool isAdmin = false;

    private async void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //connect to cloud save anonymously
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
