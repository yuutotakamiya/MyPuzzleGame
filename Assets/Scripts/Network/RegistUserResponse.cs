using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RegistUserResponse
{
    [JsonProperty("user_id")]
    public int UserID { get; set; }
    [JsonProperty("token")]
    public string AuthToken { get; set; }


}
