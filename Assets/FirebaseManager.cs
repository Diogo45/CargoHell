using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{

    public enum DBState
    {
        None, Succesful, Error
    }


    private const string url = @"https://cargohell-default-rtdb.firebaseio.com/";

    public DBState state { get; private set; } = DBState.None;

    private void Reset()
    {
        state = DBState.None;
    }


    public IEnumerator Get<T>(string key, Action<T> callback) where T : new()
    {

        Reset();

        T _response = new T();

        RestClient.Get(url + key + ".json").Then(response =>
        {
           
            try
            {
                T resp = new T();
                JsonUtility.FromJsonOverwrite(response.Text, resp);
                _response = resp;
                state = DBState.Succesful;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                state = DBState.Error;
            }



        }).Catch(e => { Debug.LogError(e); state = DBState.Error; });


        yield return new WaitWhile(() => state == DBState.None);

        if (state == DBState.Succesful)
            callback(_response);
        else
            callback(default(T));



    }


    public void Put<T>(string key, T obj) where T : new()
    {
        RestClient.Put(url + key + ".json", obj).Then(response =>
        {
            Debug.Log("SUCESS");
        }).Catch(e => { Debug.LogError(e);});


    }




}
