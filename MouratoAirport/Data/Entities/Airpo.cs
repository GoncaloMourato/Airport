namespace MouratoAirport.Data.Entities
{
    public class Airpo :IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Location { get; set; }
    }
}
