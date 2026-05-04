namespace TourneeFutee
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serverIp = "127.0.0.1"; 
            string dbname = "tourneefutee_test";
            string user = "root"; // L'utilisateur par défaut sous WAMP/XAMPP
            string pwd = "Clement2006!";      // Souvent vide sous WAMP/XAMPP, ou "root" sous MAMP

            try
            {
                // Permet d'appeler le constructeur et teste la connexion
                ServicePersistance bddService = new ServicePersistance(serverIp, dbname, user, pwd);
                Console.WriteLine("Connexion à MySQL réussie");

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
        }
    }
    }

