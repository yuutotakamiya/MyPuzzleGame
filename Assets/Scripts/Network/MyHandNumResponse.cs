using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHandNumResponse
{
    [JsonProperty("min_hand_num")]
    public int MinHandNum { get; set; }
}