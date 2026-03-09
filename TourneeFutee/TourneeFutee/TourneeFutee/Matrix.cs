using System.Data.Common;

namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
    
        private readonly float valeurpardefaut;
        private readonly List<List<float>> données;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        public Matrix(int nblignes = 0, int nbcolonnes = 0, float valeurpardefaut = 0)
        {
            if (nblignes < 0 || nbcolonnes < 0)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            this.valeurpardefaut = valeurpardefaut;
            données = new List<List<float>>();

            // Initialisation de la structure de données
            for (int i = 0; i < nblignes; i++)
            {
                List<float> ligne = new List<float>();
                for (int j = 0; j < nbcolonnes; j++)
                {
                    ligne.Add(valeurpardefaut);
                }
                données.Add(ligne);
            }
        }

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule
        public float Valeurpardefaut
        {
            get { return this.valeurpardefaut; }
        }

        // Propriété : nombre de lignes
        // Lecture seule
        public int NbLignes
        {
            get { return données.Count; }
        }

        // Propriété : nombre de colonnes
        // Lecture seule
        public int NbColonnes
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
            if (i < 0 || i > NbLignes )
                throw new ArgumentOutOfRangeException("Indice invalide.");

            List<float> nouvligne = new List<float>();
            for (int j = 0; j < NbColonnes; j++)
            {
                nouvligne.Add(Valeurpardefaut);
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
            if (j < 0 || j > NbColonnes)
                throw new ArgumentOutOfRangeException("Indice invalide. ");
            foreach (var ligne in données)
            {
                ligne.Insert(j, Valeurpardefaut);
            }
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            if (i < 0 || i >= NbLignes)
                throw new ArgumentOutOfRangeException("Indice invalide.");

            données.RemoveAt(i);
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            if (j < 0 || j >= NbColonnes)
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
            if (i < 0 || i >= NbLignes || j < 0 || j >= NbColonnes)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            return données[i][j];
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbLignes || j < 0 || j >= NbColonnes)
                throw new ArgumentOutOfRangeException("Indice invalide.");
            données[i][j] = v;
        }
        

        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                    Console.Write($"{données[i][j],8}");
                Console.WriteLine();
            }
        }


        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
