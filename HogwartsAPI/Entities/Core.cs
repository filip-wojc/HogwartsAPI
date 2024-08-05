namespace HogwartsAPI.Entities
{
    public class Core
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual IEnumerable<Wand>? Wands { get; set; }
    }
}
