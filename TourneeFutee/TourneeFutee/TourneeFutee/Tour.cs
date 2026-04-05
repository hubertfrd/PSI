namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // attributs

        private List<(string source, string destination)> segments;
        private float cost;

        // Constructeur par défaut, sans paramètres
        public Tour()
        {
            this.segments = new List<(string, string)>();
            this.cost = 0;
        }

        // Constructeur avec paramètres
        public Tour(List<(string source, string destination)> segments, float cost)
        {
            this.segments = new List<(string, string)>(segments);
            this.cost = cost;
        }

        // propriétés
        // Coût total de la tournée
        public float Cost
        {
            get
            {
                return this.cost; 
            }
        }

       
        // Nombre de trajets dans la tournée
        public int NbSegments
        {
            get
            {
                return this.segments.Count; 
            }
        }


        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            return this.segments.Contains(segment);
        }


        // Affiche les informations sur la tournée : coût total et trajets
        public void Print()
        {
            Console.WriteLine("Coût total de la tournée :" + this.Cost);
            Console.WriteLine("Trajets :");
            foreach (var segment in this.segments)
            {
                Console.WriteLine(segment.source + " -> " + segment.destination);
            }
        }

        

    }
}
