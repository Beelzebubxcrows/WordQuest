namespace Persistence.Data
{
    public class PlayerData : IPersistenceData
    {
        public  bool MusicState { get; set; }
        public  bool EffectsState { get; set; }

        public PlayerData()
        {
            MusicState = true;
            EffectsState = true;
        }
        
    }
}