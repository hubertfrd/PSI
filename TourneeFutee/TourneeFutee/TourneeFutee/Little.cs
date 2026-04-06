namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        private Graph graph;

        // Attributs pour garder en mémoire la meilleure tournée trouvée lors de la récursion
        private float bestCost= float.PositiveInfinity;
        private List<(string, string)> bestsegments =  new List<(string,string)> ();

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            // On stocke le graphe passé en paramètre dans notre attribut
            this.graph = graph;
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            // TODO : implémenter
            return new Tour();
        }

        // --- Méthodes utilitaires réalisant des étapes de l'algorithme de Little


        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            float totalReduction = 0;
            for (int i = 0; i < m.NbRows; i++)
            {
                float minRow = float.PositiveInfinity;
                for (int j = 0; j < m.NbColumns; j++)
                    if (m.GetValue(i, j) < minRow) minRow = m.GetValue(i, j);

                if (minRow != float.PositiveInfinity && minRow > 0)
                {
                    totalReduction += minRow;
                    for (int j = 0; j < m.NbColumns; j++)
                        if (m.GetValue(i, j) != float.PositiveInfinity)
                            m.SetValue(i, j, m.GetValue(i, j) - minRow);
                }
            }

        
            for (int j = 0; j < m.NbColumns; j++)
            {
                float minCol = float.PositiveInfinity;
                for (int i = 0; i < m.NbRows; i++)
                    if (m.GetValue(i, j) < minCol) minCol = m.GetValue(i, j);

                if (minCol != float.PositiveInfinity && minCol > 0)
                {
                    totalReduction += minCol;
                    for (int i = 0; i < m.NbRows; i++)
                        if (m.GetValue(i, j) != float.PositiveInfinity)
                            m.SetValue(i, j, m.GetValue(i, j) - minCol);
                }
            }
            return totalReduction;

        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            // TODO : implémenter
            return (0, 0, 0.0f);

        }

        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {

            // TODO : implémenter
            return false;   
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
