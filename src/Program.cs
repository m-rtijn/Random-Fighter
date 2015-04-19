using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace LudumDareTextBasedGame
{
    class Program
    {

        #region Main game things and main game loop

        // I stil need to come up with one
        private static string gameTitle = "TO DO: add game title";
        private static Player player;
        public static Event currentEvent;
        public static Random random = new Random();
        private static int dmgResult;
        private static int currentSeed;

        // These are all the available scenario's
        public static string[] eventTexts = new string[] { 
            "You are ambushed by five little monkeys!"
                , "An unicorn blocks the path!"
                , "A crazy sysadmin tries to choke you with a CAT 5e calbe!"
                , "An open source enthousiast tries to confince you of the benefits of the GPLv3+ license" 
                , "A Pokémon trainer challenges you to a battle!"};
        public static string[] eventTargetNames = new string[] { 
            "the five little monkeys"
            , "the unicorn"
            , "the crazy sysadmin" 
            , "the open source enthousiast"
            , "the Pokémon trainer"};
        public static string[] eventAttackNames = new string[] { 
            "The monkeys are throwing nuts at you!"
                , "The unicorn blinds you with his majesty."
                , "The crazy sysadmin still tries to choke you with a CAT 5e cable!"
                , "The open source enthousiast bores you with his arguments"
                , "The Pokémon trainer commands his Pikachu to electrocute you!"};

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to " + gameTitle);
            
            Console.WriteLine("What is your name?");
            Console.Write(">");
            string name = Console.ReadLine();

            Console.WriteLine("What is your first weapon?");
            Console.Write(">");
            string weapon = Console.ReadLine();

            player = new Player(name, weapon);

            Console.WriteLine("You are walking on a path in a magical forest.");
            Event();

            while (true)
            {
                LoopMethod();
            }
        }

        public static void LoopMethod()
        {
            Console.WriteLine("Do you want to: \n 1) Continue walking \n 2) Buy a new weapon (100 Gold Coins) \n 3) Heal (100 Gold Coins) \n 4) Get status");
            Console.Write(">");
            string choice = Console.ReadLine();

            switch(choice.ToLower())
            {
                case "1":
                    Event();
                    break;
                case "2":
                    BuyNewWeapon();
                    break;
                case "3":
                    Heal();
                    break;
                case "4":
                    GetPlayerStatus(2);
                    break;
                default:
                    Console.WriteLine("Please enter the number of the option.");
                    LoopMethod();
                    break;
            }
            
        }

        #endregion

        #region buying weapons

        public static void BuyNewWeapon()
        {
            if (player.money < 100)
            {
                Console.WriteLine("Not enough gold coins to buy a new weapon.");
                LoopMethod();
            }
            else
            {
                Console.WriteLine("Which weapon do you want to buy?");
                Console.Write(">");
                string newWeapon = Console.ReadLine();

                player.weapons.Add(newWeapon);
                player.money -= 100;

                Console.WriteLine("You spent 100 gold coins to buy the " + newWeapon);
                Console.WriteLine("You now have " + player.money.ToString() + " gold coins.");
                LoopMethod();
            }
        }

        #endregion

        #region Fighting and Event methods

        /// <summary>
        /// Create a new event
        /// </summary>
        public static void Event()
        {
            // Create a seed
            int seed = random.Next(0, eventTexts.Length);

            currentSeed = seed;

            currentEvent = new Event(eventTexts[seed], player.xp * random.Next(1, 5), eventTargetNames[seed]);
            GetInput();
        }

        /// <summary>
        /// Get input
        /// </summary>
        public static void GetInput()
        {
            GetAttacked();
            CheckDead();
            Console.WriteLine("What do you want to do? \n 1) Attack \n 2) Get player status \n 3) Get target status");
            Console.Write(">");
            string choice = Console.ReadLine();

            switch(choice.ToLower())
            {
                case "1":
                    Attack();
                    break;
                case "2":
                    GetPlayerStatus(1);
                    break;
                case "3":
                    GetTargetStatus();
                    break;
                default:
                    Console.WriteLine("Please enter the number of the option.");
                    GetInput();
                    break;
            }

        }

        /// <summary>
        /// Prints the health, xp and money of the player to the console and then continues the program.
        /// </summary>
        /// <param name="i">if i == 1 it wil call GetInput(), if i == 2 it will call LoopMethod()</param>
        public static void GetPlayerStatus(int i)
        {
            Console.WriteLine("You have " + player.health.ToString() + " health, " + player.xp.ToString() + " xp and " + player.money.ToString() + " gold coins.");

            if (i == 1)
            {
                GetInput();
            }
            if (i == 2)
            {
                LoopMethod();
            }
        }

        /// <summary>
        /// Prints the health of the current target to the console.
        /// </summary>
        public static void GetTargetStatus()
        {
            Console.WriteLine(currentEvent.targetName + " has " + currentEvent.targetHealth.ToString() + " health.");
            GetInput();
        }

        /// <summary>
        /// Let the player attack. The dmg of the attack depends on the Player's current xp.
        /// </summary>
        public static void Attack()
        {
            // Get the weapon the player wants to use and get the dmg
            Console.WriteLine("Which weapon do you want to use?");
            Console.Write(">");
            string weapon = Console.ReadLine();
            int dmg = player.DoMove(weapon);

            // Do the move (or don't when the player doesn't have the weapon (yet))
            if (dmg == -1)
            {
                Console.WriteLine("Oh no! You don't have that weapon! In all confusion you hit yourself with a pointy stick.");
                player.GetDamage(1);
                GetInput();
            }
            else
            {
                dmgResult = currentEvent.GetDamage(dmg, weapon);
            }

            // Check if the enemy is dead or not
            if (dmgResult == 0)
            {
                player.xp += currentEvent.targetBaseHealth;
                player.money += currentEvent.targetBaseHealth / 2;
                Console.WriteLine("You got " + currentEvent.targetBaseHealth.ToString() + " xp and " + (currentEvent.targetBaseHealth / 2).ToString() + " gold coins");
                return;
            }
            else if (dmgResult == 1)
            {
                GetInput();
            }
        }

        /// <summary>
        /// Lets the player get 5 health back, in exchange for 100 money
        /// </summary>
        public static void Heal()
        {
            // First be sure that the player actually has 100 money to spend
            if (player.money < 100)
            {
                Console.WriteLine("Not enough money to heal yourself.");
            }
            else
            {
                // Change the variables
                player.money -= 100;
                player.health += 5;

                // Tell the player
                Console.WriteLine("You got 5 extra health for 100 gold coins.");
                Console.WriteLine("You now have " + player.health.ToString() + " health and " + player.money.ToString() + " gold coins.");
            }
        }

        /// <summary>
        /// The player gets attacked by the current event. dmg is based on current xp of the player.
        /// </summary>
        public static void GetAttacked()
        {
            Console.WriteLine(eventAttackNames[currentSeed]);
            player.GetDamage(random.Next(player.xp / 11, player.xp / 1));
        }

        /// <summary>
        /// Check if the player is dead
        /// </summary>
        public static void CheckDead()
        {
            if (player.health < 1)
            {
                Die();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// End the game with some horrible ASCII art and with the score of the player (the total xp the player got during the game)
        /// </summary>
        public static void Die()
        {
            Console.WriteLine("You took to much damage and died.");

            Console.WriteLine("    ||   \n ===RIP=== \n    ||   \n    ||   ");
            Console.WriteLine("SCORE: " + player.xp.ToString());
            Console.Write("<press enter to exit>");
            Console.ReadLine();
            Environment.Exit(0);
        }

        #endregion

        #region other things
        /*
        public static void Output(string msg, bool enter = true)
        {
            Thread.Sleep(250);
            if (enter == true)
            {
                Console.WriteLine(msg);
            }
            else
            {
                Console.Write(msg);
            }
        }
        */
        #endregion
    }
}
