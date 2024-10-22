using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistStageRequest
{
    [JsonProperty("result")]
    public int Result { get; set; }
    [JsonProperty("min_hand_num")]
    public int HandNum { get; set; }
    [JsonProperty("stage_id")]
    public int StageID { get; set; }
    [JsonProperty("user_id")]
    public int UserID { get; set; }
}
