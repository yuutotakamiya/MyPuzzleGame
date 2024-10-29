using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistlandRequest
{
    [JsonProperty("land_id")]
    public int LandID { get; set; }
    [JsonProperty("user_id")]
    public int UserID { get; set; }
    [JsonProperty("land_block_num")]
    public int LandBlockNum { get; set; }
}
