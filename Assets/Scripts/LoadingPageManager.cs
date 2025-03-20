using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

public class LoadingPageManager : MonoBehaviour
{
    [SerializeField] Image pinwheel;
    [SerializeField] Image Logo;

    private bool isLoading = true;
    [SerializeField] int pinWheelSpinSpeed = 5;

    [SerializeField] GameObject loginPage;

    [Header("DataBase")]
    public int tempVersion = -1;
    public string currentID = "";
    private string adminToSync = "";

    private void Awake()
    {
        SyncToDataBase();
        CheckNewDay();
    }

    private void Update()
    {
        if(isLoading)
        {
            SpinPinWheel();
        }
        else
        {
            loginPage.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void SpinPinWheel()
    {

    }

    private async void SyncToDataBase()
    {
        var query = new Query(
            new List<FieldFilter>
            {
                new FieldFilter("isAdmin", true, FieldFilter.OpOptions.EQ, true)
            },

            new HashSet<string>()
        );
        
        //Find Admin's ID
        var results = await CloudSaveService.Instance.Data.Player.QueryAsync(query, new QueryOptions());

        //loop through each result collected
        foreach (var r in results)
        {
            //Save the ID used to find the version info
            currentID = r.Id;
            //collect version info
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Version" }, new LoadOptions(new PublicReadAccessClassOptions(currentID)));
            if (playerData.TryGetValue("Version", out var version))
            {
                //Check if version is newer by collecting public data "Version"
                if (tempVersion < version.Value.GetAs<int>())
                {
                    //Sync if newer version of database found
                    adminToSync = currentID;
                    tempVersion = version.Value.GetAs<int>();
                }
            }
        };
    }

    private void CheckNewDay()
    {

    }
}
