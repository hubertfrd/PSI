namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        private readonly int nbRows;
        private readonly int nbColumns;
        private readonly float defaultValue;
        private readonly List<List<float>> données;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        public Matrix(List<List<float>> données,int nbRows = 0, int nbColumns = 0, float defaultValue = 0)
        {
            if (nbRows < 0 || nbColumns < 0)
                Console.WriteLine("Les dimensions ne peuvent pas être négatives.");

            this.nbRows = nbRows;
            this.nbColumns = nbColumns;
            this.defaultValue = defaultValue;
            this.données = new List<List<float>>();

            // Initialisation de la structure de données
            for (int i = 0; i < nbRows; i++)
            {
                List<float> row = new List<float>();
                for (int j = 0; j < nbColumns; j++)
                {
                    row.Add(defaultValue);
                }
                données.Add(row);
            }
        }

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule
        public float DefaultValue
        {
            get; // TODO : implémenter
                 // pas de set
        }

        // Propriété : nombre de lignes
        // Lecture seule
        public int NbRows
        {
            get; // TODO : implémenter
                 // pas de set
        }

        // Propriété : nombre de colonnes
        // Lecture seule
        public int NbColumns
        {
            get; // TODO : implémenter
                 // pas de set
        }

        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            if (i < 0 || i > NbRows)
                Console.WriteLine(nameof(i), "Indice de ligne hors limites.");

            List<float> newRow = new List<float>();
            int currentCols = NbColumns;

            for (int j = 0; j < currentCols; j++)
            {
                newRow.Add(defaultValue);
            }

            données.Insert(i, newRow);
           
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            if (j < 0 || j > NbColumns)
                Console.WriteLine(nameof(j), "Indice de colonne hors limites.");

            foreach (var row in données)
            {
                row.Insert(j, defaultValue);
            }
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            if (i < 0 || i >= NbRows)
                Console.WriteLine(nameof(i), "Indice invalide.");

            données.RemoveAt(i);
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            if (j < 0 || j >= NbColumns)
                Console.WriteLine(nameof(j), "Indice invalide.");

            foreach (var row in données)
            {
                row.RemoveAt(j);
            }
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            // TODO : implémenter
            return 0.0f;
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
        }

        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
        }


        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
