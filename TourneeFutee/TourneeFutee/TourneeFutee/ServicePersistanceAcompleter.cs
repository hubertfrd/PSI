using System;
using System.Reflection.PortableExecutable;
using MySql.Data.MySqlClient;

namespace TourneeFutee
{
    /// <summary>
    /// Service de persistance permettant de sauvegarder et charger
    /// des graphes et des tournées dans une base de données MySQL.
    /// </summary>
    public class ServicePersistance
    {
        // ─────────────────────────────────────────────────────────────────────
        // Attributs privés
        // ─────────────────────────────────────────────────────────────────────

        private readonly string _connectionString;

        // TODO : si vous avez besoin de maintenir une connexion ouverte,
        //        ajoutez un attribut MySqlConnection ici.

        // ─────────────────────────────────────────────────────────────────────
        // Constructeur
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Instancie un service de persistance et se connecte automatiquement
        /// à la base de données <paramref name="dbname"/> sur le serveur
        /// à l'adresse IP <paramref name="serverIp"/>.
        /// Les identifiants sont définis par <paramref name="user"/> (utilisateur)
        /// et <paramref name="pwd"/> (mot de passe).
        /// </summary>
        /// <param name="serverIp">Adresse IP du serveur MySQL.</param>
        /// <param name="dbname">Nom de la base de données.</param>
        /// <param name="user">Nom d'utilisateur.</param>
        /// <param name="pwd">Mot de passe.</param>
        /// <exception cref="Exception">Levée si la connexion échoue.</exception>
        public ServicePersistance(string serverIp, string dbname, string user, string pwd)
        {
            // 1. Définition de la chaîne de connexion
            _connectionString = $"server={serverIp};database={dbname};uid={user};pwd={pwd};";

            // 2. Test de la connexion dès la construction
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    // Si on arrive ici, les identifiants sont bons et on peut accéder à la BDD
                }
            }
            catch (MySqlException ex)
            {
                // On capture l'erreur pour remonter un message clair
                throw new Exception("Erreur de connexion : " + ex.Message);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes publiques
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Sauvegarde le graphe <paramref name="g"/> en base de données
        /// (sommets et arcs inclus) et renvoie son identifiant.
        /// </summary>
        /// <param name="g">Le graphe à sauvegarder.</param>
        /// <returns>Identifiant du graphe en base de données (AUTO_INCREMENT).</returns>
        public uint SaveGraph(Graph g)
        {
            // TODO : implémenter la sauvegarde du graphe
            //
            // Ordre recommandé :
            //   1. INSERT dans la table Graphe -> récupérer l'id avec LAST_INSERT_ID()
            //   2. Pour chaque sommet de g : INSERT dans Sommet (valeur + graphe_id)
            //      -> conserver la correspondance sommet C# <-> id BdD
            //   3. Pour chaque arc de la matrice d'adjacence (poids != +inf) :
            //      INSERT dans Arc (sommet_source_id, sommet_dest_id, poids, graphe_id)
            //
            // Exemple pour récupérer l'id généré :
            //   uint id = Convert.ToUInt32(cmd.ExecuteScalar());
            using (MySqlConnection conn = OpenConnection())
            {
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Insertion du Graphe
                        string sqlGraph = "INSERT INTO Graphe (est_oriente) VALUES (@directed); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmdGraph = new MySqlCommand(sqlGraph, conn, trans);
                        cmdGraph.Parameters.AddWithValue("@directed", g.Directed);
                        uint graphId = Convert.ToUInt32(cmdGraph.ExecuteScalar());

                        // Insertion des Sommets et mise en mémoire de leurs IDs SQL
                        var vertexNames = g.GetVertexNames();
                        Dictionary<string, uint> vertexIdMap = new Dictionary<string, uint>();

                        foreach (string name in vertexNames)
                        {
                            string sqlVertex = "INSERT INTO Sommet (nom, valeur, graphe_id) VALUES (@nom, @val, @gid); SELECT LAST_INSERT_ID();";
                            MySqlCommand cmdVertex = new MySqlCommand(sqlVertex, conn, trans);
                            cmdVertex.Parameters.AddWithValue("@nom", name);
                            cmdVertex.Parameters.AddWithValue("@val", g.GetVertexValue(name));
                            cmdVertex.Parameters.AddWithValue("@gid", graphId);

                            uint vId = Convert.ToUInt32(cmdVertex.ExecuteScalar());
                            vertexIdMap.Add(name, vId);
                        }

                        // Insertion des Arcs
                        foreach (string source in vertexNames)
                        {
                            foreach (string dest in vertexNames)
                            {
                                try
                                {
                                    float weight = g.GetEdgeWeight(source, dest);
                                    string sqlArc = "INSERT INTO Arc (sommet_source, sommet_dest, poids, graphe_id) " +
                                     "VALUES (@src, @dst, @poids, @gid);";
                                    MySqlCommand cmdArc = new MySqlCommand(sqlArc, conn, trans);
                                    cmdArc.Parameters.AddWithValue("@src", vertexIdMap[source]);
                                    cmdArc.Parameters.AddWithValue("@dst", vertexIdMap[dest]);
                                    cmdArc.Parameters.AddWithValue("@poids", weight);
                                    cmdArc.Parameters.AddWithValue("@gid", graphId);
                                    cmdArc.ExecuteNonQuery();
                                }
                                catch (ArgumentException) { /* L'arc n'existe pas, on passe au suivant */ }
                            }
                        }

                        trans.Commit();
                        return graphId;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw new Exception("Erreur lors de la sauvegarde du graphe : " + ex.Message);
                    }
                }
            }

            throw new NotImplementedException("SaveGraph non implémenté.");
        }

        /// <summary>
        /// Charge depuis la base de données le graphe identifié par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Graph"/>.
        /// </summary>
        /// <param name="id">Identifiant du graphe à charger.</param>
        /// <returns>Instance de <see cref="Graph"/> reconstituée.</returns>
        public Graph LoadGraph(uint id)
        {
            // TODO : implémenter le chargement du graphe
            //
            // Ordre recommandé :
            //   1. SELECT dans Graphe WHERE id = @id -> récupérer IsOriented, etc.
            //   2. SELECT dans Sommet WHERE graphe_id = @id -> reconstruire les sommets
            //      (respecter l'ordre d'insertion pour que les indices de la matrice
            //       correspondent à ceux sauvegardés)
            //   3. SELECT dans Arc WHERE graphe_id = @id -> reconstruire la matrice
            //      d'adjacence en utilisant les correspondances sommet_id <-> indice
            
            throw new NotImplementedException("LoadGraph non implémenté.");
        }

        /// <summary>
        /// Sauvegarde la tournée <paramref name="t"/> (effectuée dans le graphe
        /// identifié par <paramref name="graphId"/>) en base de données
        /// et renvoie son identifiant.
        /// </summary>
        /// <param name="graphId">Identifiant BdD du graphe dans lequel la tournée a été calculée.</param>
        /// <param name="t">La tournée à sauvegarder.</param>
        /// <returns>Identifiant de la tournée en base de données (AUTO_INCREMENT).</returns>
        public uint SaveTour(uint graphId, Tour t)
        {
            // TODO : implémenter la sauvegarde de la tournée
            //
            // Ordre recommandé :
            //   1. INSERT dans Tournee (cout_total, graphe_id) -> récupérer l'id
            //   2. Pour chaque sommet de la séquence (avec son numéro d'ordre) :
            //      INSERT dans EtapeTournee (tournee_id, numero_ordre, sommet_id)
            //
            // Attention : conserver l'ordre des étapes est essentiel pour
            //             pouvoir reconstruire la tournée fidèlement au chargement.
           
            throw new NotImplementedException("SaveTour non implémenté.");
        }

        /// <summary>
        /// Charge depuis la base de données la tournée identifiée par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Tour"/>.
        /// </summary>
        /// <param name="id">Identifiant de la tournée à charger.</param>
        /// <returns>Instance de <see cref="Tour"/> reconstituée.</returns>
        public Tour LoadTour(uint id)
        {
            // TODO : implémenter le chargement de la tournée
            //
            // Ordre recommandé :
            //   1. SELECT dans Tournee WHERE id = @id -> récupérer cout_total et graphe_id
            //   2. SELECT dans EtapeTournee JOIN Sommet WHERE tournee_id = @id
            //      ORDER BY numero_ordre -> reconstruire la séquence ordonnée de sommets
            //   3. Construire et retourner l'instance Tour
           

            throw new NotImplementedException("LoadTour non implémenté.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes utilitaires privées (à compléter selon vos besoins)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Crée et retourne une nouvelle connexion MySQL ouverte.
        /// Encadrez toujours l'appel dans un bloc using pour garantir la fermeture.
        /// </summary>
        private MySqlConnection OpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}
