using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegistLandResponse
{
    [JsonProperty("id")]
    public int LandID { get; set; }
    [JsonProperty("stage_id")]
    public int StageID { get; set; }
    [JsonProperty("block_mission_sum")]
    public int BlockMissionSum { get; set; }
    [JsonProperty("result")]
    public int Result { get; set; }
    [JsonProperty("land_block_num")]
    public int LandBlockNum { get; set; }

}
