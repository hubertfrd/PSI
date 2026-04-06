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
            List<string> names = graph.GetVertexNames();
            int n = names.Count;
            Matrix m = new Matrix(n, n, float.PositiveInfinity);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    try { m.SetValue(i, j, graph.GetEdgeWeight(names[i], names[j])); }
                    catch { /* L'arc n'existe pas, reste à l'infini */ }
                }
            }
            float initialBound = ReduceMatrix(m);
            this.bestCost = float.PositiveInfinity;
            this.bestsegments = new List<(string, string)>();

            Resoudre(m, initialBound, new List<(string, string)>(), names);

            return new Tour(this.bestsegments, this.bestCost);
        }

        private void Resoudre(Matrix m, float currentLowerBound, List<(string, string)> included, List<string> names)
        {
 
            if (currentLowerBound >= this.bestCost) return;
            if (included.Count == names.Count)
            {
                this.bestCost = currentLowerBound;
                this.bestsegments = new List<(string, string)>(included);
                return;
            }

          
            var pivot = GetMaxRegret(m);
            if (pivot.i == -1) return;

            string u = names[pivot.i], v = names[pivot.j];

            Matrix mInc = new Matrix(m.NbRows, m.NbColumns, float.PositiveInfinity);
            for (int r = 0; r < m.NbRows; r++)
            {
                for (int c = 0; c < m.NbColumns; c++)
                {
                  
                    mInc.SetValue(r, c, (r == pivot.i || c == pivot.j) ? float.PositiveInfinity : m.GetValue(r, c));
                }
            }

            if (pivot.j < mInc.NbRows && pivot.i < mInc.NbColumns)
            {
                mInc.SetValue(pivot.j, pivot.i, float.PositiveInfinity);
            }

            for (int r = 0; r < mInc.NbRows; r++)
            {
                for (int c = 0; c < mInc.NbColumns; c++)
                {
                    if (mInc.GetValue(r, c) != float.PositiveInfinity &&
                        IsForbiddenSegment((names[r], names[c]), new List<(string, string)>(included) { (u, v) }, names.Count))
                    {
                        mInc.SetValue(r, c, float.PositiveInfinity);
                    }
                }
            }

            float bInc = currentLowerBound + ReduceMatrix(mInc);
     
            Matrix mExc = new Matrix(m.NbRows, m.NbColumns, float.PositiveInfinity);
            for (int r = 0; r < m.NbRows; r++)
            {
                for (int c = 0; c < m.NbColumns; c++)
                {
                
                    mExc.SetValue(r, c, (r == pivot.i && c == pivot.j) ? float.PositiveInfinity : m.GetValue(r, c));
                }
            }

            float bExc = currentLowerBound + ReduceMatrix(mExc);
            if (bInc < bExc)
            {
                Resoudre(mInc, bInc, new List<(string, string)>(included) { (u, v) }, names);
                Resoudre(mExc, bExc, included, names);
            }
            else
            {
                Resoudre(mExc, bExc, included, names);
                Resoudre(mInc, bInc, new List<(string, string)>(included) { (u, v) }, names);
            }
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

        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            int bestI = -1, bestJ = -1;
            float maxRegret = -1;

            for (int i = 0; i < m.NbRows; i++)
            {
                for (int j = 0; j < m.NbColumns; j++)
                {
                    if (m.GetValue(i, j) == 0)
                    {
                        // Min de la ligne (hors case actuelle)
                        float minRow = float.PositiveInfinity;
                        for (int col = 0; col < m.NbColumns; col++)
                            if (col != j && m.GetValue(i, col) < minRow) minRow = m.GetValue(i, col);

                        // Min de la colonne (hors case actuelle)
                        float minCol = float.PositiveInfinity;
                        for (int row = 0; row < m.NbRows; row++)
                            if (row != i && m.GetValue(row, j) < minCol) minCol = m.GetValue(row, j);

                        float regret = (minRow == float.PositiveInfinity ? 0 : minRow) +
                                       (minCol == float.PositiveInfinity ? 0 : minCol);

                        if (regret > maxRegret)
                        {
                            maxRegret = regret;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }
            return (bestI, bestJ, maxRegret);
        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale


        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {

            if (includedSegments.Count == nbCities - 1) return false;

            string current = segment.destination;
            while (true)
            {
                var next = includedSegments.Find(s => s.source == current);
                if (next.source == null) break; 
                current = next.destination;

                if (current == segment.source) 
                    return true; 
            }
            return false;   
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }
}
