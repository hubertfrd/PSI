namespace TourneeFutee
{
    public class Graph
    {

        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        private List<string> vertexNames;
        private List<float> vertexValues;
        private List<List<float>> adjacencyMatrix;

        private bool directed;
        private float noEdgeValue;



        // --- Construction du graphe ---

        // Contruction d'un graphe 
        //Indique si le graphe est orienté (true) ou non (false)
        // La valeur `noEdgeValue` est loids qui modélise l'absence d'un arc (0 par défaut)
        public Graph(bool directed, float noEdgeValue = 0)
        {

            this.directed = directed;
            this.noEdgeValue = noEdgeValue;

            this.vertexNames = new List<string>();
            this.vertexValues = new List<float>();
            this.adjacencyMatrix = new List<List<float>>();
        }


        // --- Propriétés ---

        // Propriété : on obtient l'ordre du graphe
        // Lecture seule
        public int Order
        {
            get { return this.vertexNames.Count; }
            // pas de set
        }

        // Propriété : Nous indique si un graphe est orienté ou non
        // Lecture seule
        public bool Directed
        {
            get { return this.directed; }   // TODO : implémenter
                                            // pas de set
        }


        // --- Gestion des sommets ---

        // Ajoute le sommet de nom `name` et de valeur `value` (0 par défaut) dans le graphe
        // Une exception est relevée si   existe déjà un sommet avec le même nom dans le graphe
        public void AddVertex(string name, float value = 0)
        {
            if (vertexNames.Contains(name))
                throw new ArgumentException("Existe deja");

            vertexNames.Add(name);
            vertexValues.Add(value);

            // Permet d'ajouter une colonne dans chaque ligne existante
            foreach (var row in adjacencyMatrix)
                row.Add(noEdgeValue);

            // Créer nouvelle ligne
            List<float> newRow = new List<float>();


            for (int i = 0; i < Order; i++)
                newRow.Add(noEdgeValue);

            adjacencyMatrix.Add(newRow);
        }


        // Supprime un sommet du graphe et tous les arcs qui y lui sont associés.
        // Si le sommet n'a pas été trouvé dans le graphe alors une exception est levée
        public void RemoveVertex(string name)
        {
            int index = GetVertexIndex(name);

            vertexNames.RemoveAt(index);
            vertexValues.RemoveAt(index);

            adjacencyMatrix.RemoveAt(index);//supprimer ligne

            foreach (var row in this.adjacencyMatrix)
                row.RemoveAt(index); //supprimer colonne
        }

        // Renvoie la valeur du sommet de nom `name`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public float GetVertexValue(string name)
        {
            int index = GetVertexIndex(name);
            return vertexValues[index];
        }

        // Affecte la valeur du sommet de nom `name` à `value`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void SetVertexValue(string name, float value)
        {
            int index = GetVertexIndex(name);
            vertexValues[index] = value;
        }


        // Renvoie la liste des noms des voisins du sommet de nom `vertexName`
        // (si ce sommet n'a pas de voisins, la liste sera vide)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public List<string> GetNeighbors(string vertexName)
        {
            int index = GetVertexIndex(vertexName);
            List<string> neighborNames = new List<string>();

            // On parcourt la ligne correspondant au sommet dans la matrice d'adjacence
            for (int j = 0; j < Order; j++)
            {
                // Si le poids est différent de noEdgeValue, il y a un arc vers le sommet j
                if (adjacencyMatrix[index][j] != noEdgeValue)
                {
                    neighborNames.Add(vertexNames[j]);
                }
            }

            return neighborNames;
        }
        private int GetVertexIndex(string name)
        {
            int index = vertexNames.IndexOf(name);
            if (index == -1)
                throw new ArgumentException($"Le sommet '{name}' n'existe pas.");
            return index;
        }

        // --- Gestion des arcs ---

        /* Ajoute un arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`, avec le poids `weight` (1 par défaut)
         * Si le graphe n'est pas orienté, ajoute aussi l'arc inverse, avec le même poids
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - il existe déjà un arc avec ces extrémités
         */
        public void AddEdge(string sourceName, string destinationName, float weight = 1)
        {
            int src = GetVertexIndex(sourceName);
            int dest = GetVertexIndex(destinationName);

            // Vérifier si l'arc existe déjà
            if (adjacencyMatrix[src][dest] != noEdgeValue)
                throw new ArgumentException("Existe deja.");

            // Ajouter l'arc
            adjacencyMatrix[src][dest] = weight;

            // Si le graphe n'est pas orienté, on ajoute l'arc inverse
            if (!directed)
            {
                adjacencyMatrix[dest][src] = weight;
            }
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName)
        {
            int src = GetVertexIndex(sourceName);
            int dest = GetVertexIndex(destinationName);

            // Vérifier si l'arc existe
            if (adjacencyMatrix[src][dest] == noEdgeValue)
                throw new ArgumentException("N'existe pas.");

            // "Supprimer" en remettant la valeur par défaut
            adjacencyMatrix[src][dest] = noEdgeValue;

            if (!directed)
            {
                adjacencyMatrix[dest][src] = noEdgeValue;
            }
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName)
        {
            int src = GetVertexIndex(sourceName);
            int dest = GetVertexIndex(destinationName);

            if (adjacencyMatrix[src][dest] == noEdgeValue)
                throw new ArgumentException("N'existe pas.");

            return adjacencyMatrix[src][dest];
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight)
        {
            int src = GetVertexIndex(sourceName);
            int dest = GetVertexIndex(destinationName);

            // Note : On ne vérifie pas si l'arc existe ici selon l'énoncé, 
            // on affecte simplement le poids (cela peut créer l'arc s'il n'existait pas)
            adjacencyMatrix[src][dest] = weight;

            if (!directed)
            {
                adjacencyMatrix[dest][src] = weight;
            }
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
