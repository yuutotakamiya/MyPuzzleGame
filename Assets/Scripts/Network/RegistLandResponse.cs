using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistLandResponse
{
    [JsonProperty("stage_id")]
    public int StageID { get; set; }
    [JsonProperty("block_mission_sum")]
    public int BlockMissionNum { get; set; }
    [JsonProperty("result")]
    public int Result { get; set; }
}
