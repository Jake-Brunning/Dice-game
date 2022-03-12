using System;
using System.IO;
namespace Dice_game
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press 1 for creating a new user, anything else to play the game.");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    NewUser();
                    continue;
                }
                break;
            }
            Console.WriteLine("Player 1 sign in");
            Player Player1 = new Player(getData());
            Console.WriteLine("Player 2 sign in");
            Player Player2 = new Player(getData());

            for (int i = 0; i < 10; i++)
            {
                Console.Clear();
                if(i % 2 == 0)
                {
                    Player1.RollDice(Player1);
                }
                else
                {
                    Player2.RollDice(Player2);
                }
            }
            Console.WriteLine("End of game! final scores");
            Console.WriteLine(Player1.name + " got " + Player1.score + " points!");
            Console.WriteLine(Player2.name + " got " + Player2.score + " points!");

            if(Player1.score > Player2.score)
            {
                Console.WriteLine(Player1.name + " " + " won!");
            }
            else if (Player1.score == Player2.score)
            {
                Console.WriteLine("Draw!");
            }
            else
            {
                Console.WriteLine(Player2.name + " " + " won!");
            }

            Console.WriteLine("Displaying top 5 scores...");
            Console.ReadKey();
            Console.Clear();
            string[] data = getData();
            
            if(int.Parse(data[Player1.whereInDatabase + 2]) < Player1.score)
            {
                data[Player1.whereInDatabase + 2] = Player1.score.ToString();
            }
            if(int.Parse(data[Player2.whereInDatabase + 2]) < Player2.score)
            {
                data[Player1.whereInDatabase + 2] = Player2.score.ToString();
            }

            int[] scores = new int[data.Length / 3];
            int count = 0;
            for(int i = 2; i < data.Length; i+= 3)
            {
                scores[count] = int.Parse(data[i]);
                count++;
            }
            Array.Sort(scores);
            Array.Reverse(scores);
            int HowManyToDisplay;
            if(scores.Length < 5)
            {
                HowManyToDisplay = scores.Length;
            }
            else
            {
                HowManyToDisplay = 5;
            }
            for(int i = 0; i < HowManyToDisplay; i++)
            {
                Console.WriteLine(scores[i]);
            }

            StreamWriter writer = new StreamWriter("Database.txt");
            string toWrite = "";
            for(int i = 0; i < data.Length; i++)
            {
                toWrite += data[i] + ",";
                if(i + 1 % 3 == 0)
                {
                    toWrite += "\n";
                }
            }
            writer.Write(toWrite);
            writer.Close();
            System.Environment.Exit(0);
        }
       
        static void NewUser()
        {
            string username;
            string password;
            string checkpass;

            while (true)
            {
                //Enter username and check if username is already being used
                Console.WriteLine("Enter username");
                username = Console.ReadLine();

                //check if there is a username like that already       
                string[] savedUsernames = getData(); //split it into array
                bool exists = false;

                for (int i = 0; i < savedUsernames.Length; i += 3) //Go through each username in database
                {
                    if (username == savedUsernames[i])
                    {
                        exists = true; 
                    }
                }
                if (exists == true)
                {
                    Console.WriteLine("Username exists, enter a new one");
                    continue;
                }
                break;
            }       
            while (true)
            {
                Console.WriteLine("Enter a password");
                password = Console.ReadLine();

                Console.WriteLine("Re-enter your password");
                checkpass = Console.ReadLine();

                if (password == checkpass)
                {
                    Console.WriteLine("User created!");
                    StreamWriter writer = File.AppendText("Database.txt");
                    writer.Write(username + "," + password + "," + "0" + "," + "\n");
                    writer.Close();
                    break;
                }
                Console.WriteLine("Entered passwords Did not match, Try again");
            }

        }
         static string[] getData()
        {
            string databaseText = File.ReadAllText("Database.txt"); //All text in database
            databaseText = databaseText.Replace("\n", ""); //get rid of new lines
            string[] savedUsernames = databaseText.Split(','); //split it into array
            return savedUsernames;
            //Getdata array formatted like: [username][password][highscore][2ndUsername][2ndPassword][2ndHighscore][3rdUsername]...
        }
    }

    class Player
    {
       public string name;
       public int score;
       public int whereInDatabase; //username position in database
        public Player(string[] data)
        {
           while (true)
           {
                Console.WriteLine("Enter username");
                string username = Console.ReadLine();

                Console.WriteLine("Enter Password");
                string password = Console.ReadLine();

                bool accountDetails = false;             
                for(int i = 0; i < data.Length; i++)
                {
                    if(i % 3 == 0 && username == data[i]) //If on a username and it equals entered username
                    {
                        if (password == data[i + 1]) //Next one is always password
                        {
                            accountDetails = true;
                            whereInDatabase = i;
                        }
                    }
                }
                if (accountDetails == true)
                {
                    name = username;
                    score = 0;
                    Console.WriteLine("Player signed in");
                    break;
                }
                Console.WriteLine("Account details were not right, try again");
           }

        }
        public void RollDice(Player self)
        {       
            Console.WriteLine(self.name + " Press enter to roll the Dice! (Currently on " + self.score + " points)");
            Console.ReadKey();
            Random rnd = new Random();
            int dice1 = rnd.Next(1, 7);
            int dice2 = rnd.Next(1, 7);

            Console.WriteLine(dice1);
            Console.WriteLine(dice2);
            self.score += dice1;
            self.score += dice2;
            if (dice1 % 2 == 0 && dice2 % 2 == 0)
            {
                Console.WriteLine("Both even! +5 extra points");
                self.score += 5;
            }
            if (dice1 % 2 == 1 && dice2 % 2 == 1)
            {
                Console.WriteLine("Both odd! -5 points");
                self.score -= 5;
            }
            if (dice1 == dice2)
            {
                Console.WriteLine("Both the same! +10 extra points");
                self.score += 10;
            }
            if (self.score < 0)
            {
                self.score = 0;
            }
            Console.WriteLine("Press anything to end your turn");
            Console.ReadKey();
        }
 
    }
    
}
