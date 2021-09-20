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

    public Dictionary<string, DBState> state { get; private set; } = new Dictionary<string, DBState>();

    private void Reset()
    {
        state = new Dictionary<string, DBState>();
    }


    public IEnumerator Get<T>(string key, Action<T> callback) where T : new()
    {

        //Reset();

        T _response = new T();

        state.Add(key, DBState.None);

        var pro = RestClient.Get(url + key + ".json").Then(response =>
        {
           
            try
            {
                if(response.Text == "")
                {
                    state[key] = DBState.Error;
                    _response = default(T);

                }
                else
                {
                    T resp = new T();
                    JsonUtility.FromJsonOverwrite(response.Text, resp);
                    _response = resp;
                    state[key] = DBState.Succesful;
                }

               
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                state[key] = DBState.Error;
            }



        }).Catch(e => { Debug.LogError(e); state[key] = DBState.Error; });


        yield return new WaitWhile(() => state[key] == DBState.None);

        if (state[key] == DBState.Succesful)
            callback(_response);
        else
            callback(default(T));

        state.Remove(key);

    }


    public void Put<T>(string key, T obj) where T : new()
    {

        string jsonObj = JsonUtility.ToJson(obj);


        RestClient.Put(url + key + ".json", jsonObj).Then(response =>
        {
            Debug.Log("SUCESS");

        }).Catch(e => { Debug.LogError(e);});


    }




}
