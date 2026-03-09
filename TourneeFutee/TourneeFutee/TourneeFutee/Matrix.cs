using System.Data.Common;

namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 

        private readonly float defaultValue;
        private readonly List<List<float>> données;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        public Matrix(int nbRows = 0, int nbColumns = 0, float defaultValue = 0)
        {
            if (nbRows < 0 || nbColumns < 0)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            this.defaultValue = defaultValue;
            données = new List<List<float>>();

            // Initialisation de la structure de données
            for (int i = 0; i < nbRows; i++)
            {
                List<float> ligne = new List<float>();
                for (int j = 0; j < nbColumns; j++)
                {
                    ligne.Add(defaultValue);
                }
                données.Add(ligne);
            }
        }

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule
        public float DefaultValue
        {
            get { return this.defaultValue; }
        }

        // Propriété : nombre de lignes
        // Lecture seule
        public int NbRows
        {
            get { return données.Count; }
        }

        // Propriété : nombre de colonnes
        // Lecture seule
        public int NbColumns
        {
            get
            {
                if (données.Count == 0) return 0;
                return données[0].Count;
            }
        }

        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            if (i < 0 || i > NbRows)
                throw new ArgumentOutOfRangeException("Indice invalide.");

            List<float> nouvligne = new List<float>();
            for (int j = 0; j < NbColumns; j++)
            {
                nouvligne.Add(defaultValue);
            }

            données.Insert(i, nouvligne);

        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            if (j < 0 || j > NbColumns)
                throw new ArgumentOutOfRangeException("Indice invalide. ");
            foreach (var ligne in données)
            {
                ligne.Insert(j, defaultValue);
            }
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            if (i < 0 || i >= NbRows)
                throw new ArgumentOutOfRangeException("Indice invalide.");

            données.RemoveAt(i);
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            if (j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException("Indice invalide.");

            foreach (var ligne in données)
            {
                ligne.RemoveAt(j);
            }
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbRows || j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            return données[i][j];
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbRows || j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            données[i][j] = v;
        }


        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
            for (int i = 0; i < NbRows; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                    Console.Write($"{données[i][j],8}");
                Console.WriteLine();
            }
        }


        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
