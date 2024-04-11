using Newtonsoft.Json;

namespace Persistence.Data
{
    
    public class PersistenceData
    {
        [JsonProperty("pld")]
        public PlayerData PlayerData { get; set; }
        
        [JsonProperty("prd")]
        public ProgressData ProgressData { get; set; }

        public PersistenceData()
        {
            PlayerData = new PlayerData();
            ProgressData = new ProgressData();
        }
    }

    public interface IPersistenceData
    {
        
    }
}