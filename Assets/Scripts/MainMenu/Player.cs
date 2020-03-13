using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string _httpServerAddress = "https://localhost:44325/";
    public string HttpServerAddress
    {
        get
        {
            return _httpServerAddress;
        } 
    }

    private string _token;
    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _firstName;
    public string FirstName
    {
        get { return _firstName; }
        set { _firstName = value; }
    }

    private string _lastName;
    public string LastName
    {
        get { return _lastName; }
        set { _lastName = value; }
    }

    private string _nickName;
    public string NickName
    {
        get { return _nickName; }
        set { _nickName = value; }
    }

    private string _city;
    public string City
    {
        get { return _city; }
        set { _city = value; }
    }

    private DateTime _birthday;
    public DateTime BirthDay
    {
        get { return _birthday; }
        set { _birthday = value; }
    }

    private string _email;
    public string Email
    {
        get { return _email; }
        set { _email = value; }
    }

}
