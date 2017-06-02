using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JSONConsumer
{
    public class PetOwnerRecord
    {
        public PetOwnerRecord()
        {
            Pets = new List<PetRecord>();            
        }

        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        
        [JsonProperty("pets")]
        public List<PetRecord> Pets { get; set; }
    }
}
