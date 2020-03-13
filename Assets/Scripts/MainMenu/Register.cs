using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Collections;
using System.Globalization;

public class Register : MonoBehaviour
{
    // Cached references
    public InputField FirstNameInputField;
    public InputField LastNameInputField;
    public InputField NickNameInputField;
    public InputField CityInputText;
    public InputField birthdateInputField;
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public Button registerButton;
    
    public Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void OnRegisterButtonClick()
    {
        StartCoroutine(RegisterNewUser());
    }

    private IEnumerator RegisterNewUser()
    {
        yield return RegisterUser();
        yield return Helper.InitializeToken(emailInputField.text, passwordInputField.text);  //Sets player.Token
        
        yield return Helper.GetPlayerId();  //Sets player.Id
        
        player.Email = emailInputField.text;
        player.FirstName = FirstNameInputField.text;
        player.LastName = LastNameInputField.text;
        player.NickName = NickNameInputField.text;
        player.City = CityInputText.text;
        player.BirthDay = DateTime.Parse(birthdateInputField.text);
        yield return InsertPlayer();
        
        yield return InsertPlayerToChat();
        player.Id = string.Empty;
        player.Token = string.Empty;
        player.FirstName = string.Empty;
        player.LastName = string.Empty;
        player.NickName = string.Empty;
        player.City = string.Empty;
        player.Email = string.Empty;
        player.BirthDay = DateTime.MinValue;
        
    }

    private IEnumerator RegisterUser()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Account/Register", "POST");

        AspNetUserRegister newUser = new AspNetUserRegister();
        newUser.Email = emailInputField.text;
        newUser.Password = passwordInputField.text;
        newUser.ConfirmPassword = confirmPasswordInputField.text;

        string jsonData = JsonUtility.ToJson(newUser);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");

        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
        }

        httpClient.Dispose();
    }

    private IEnumerator InsertPlayer()
    {
        PlayerSerializable playerSerializable = new PlayerSerializable();
        playerSerializable.Id = player.Id;
        playerSerializable.FirstName = player.FirstName;
        playerSerializable.LastName = player.LastName;
        playerSerializable.NickName = player.NickName;
        playerSerializable.City = player.City;
        playerSerializable.Email = player.Email;
        playerSerializable.BirthDay = player.BirthDay.ToString();

        using (UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Player/RegisterPlayer", "POST"))
        {
            string playerData = JsonUtility.ToJson(playerSerializable);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(playerData);
            httpClient.uploadHandler = new UploadHandlerRaw(bodyRaw);
            httpClient.SetRequestHeader("Content-type", "application/json");
            httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);

            yield return httpClient.SendWebRequest();

            if (httpClient.isNetworkError || httpClient.isHttpError)
            {
                throw new Exception("RegisterNewPlayer > InsertPlayer: " + httpClient.error);
            }
            else
            {
                Debug.Log("RegisterNewPlayer > InsertPlayer: " + httpClient.responseCode);
            }
        }

    }
    
    private IEnumerator InsertPlayerToChat()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Chat/PostNewUser", "POST");

        ChatModel cm = new ChatModel();
        cm.Id = player.Id;
        cm.LastMesage = "";

        string jsonData = JsonUtility.ToJson(cm);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);

        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnClickRegisterButton: Error > " + httpClient.error);
        }

        httpClient.Dispose();
    }


}
