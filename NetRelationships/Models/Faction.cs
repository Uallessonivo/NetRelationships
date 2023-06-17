using System.Text.Json.Serialization;

namespace NetRelationships.Models
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Character> Characters { get; set; }
    }
}
