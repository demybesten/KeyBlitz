using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.ViewModels
{
    public class Player
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("wpm")]
        public int? Wpm { get; set; }

        [JsonProperty("cpm")]
        public int? Cpm { get; set; }

        [JsonProperty("accuracy")]
        public int? Accuracy { get; set; }
    }
}
