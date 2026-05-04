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




        //Nouveau constructeur attendu par les tests
        // Construit une tournée à partir d'une séquence de sommets (ex: "A", "C", "F"...)
        public Tour(List<string> sequence, float cost)
        {
            this.segments = new List<(string, string)>();

            // Transforme la séquence de sommets consécutifs en segments (A->C, C->F, etc.)
            for (int i = 0; i < sequence.Count - 1; i++)
            {
                this.segments.Add((sequence[i], sequence[i + 1]));
            }

            this.cost = cost;
        }

        // Propriété attendue par les tests
        // Reconstruit et renvoie la séquence ordonnée des sommets visités
        public List<string> Vertices
        {
            get
            {
                var list = new List<string>();
                if (this.segments.Count == 0)
                    return list;

                // Ajoute la source du tout premier segment
                list.Add(this.segments[0].source);

                // Ajoute les destinations de tous les segments
                foreach (var segment in this.segments)
                {
                    list.Add(segment.destination);
                }
                return list;
            }
        }


    }
}
