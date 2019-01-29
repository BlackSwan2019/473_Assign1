/*
 * Program:     Assignment 1
 * Author:      Patrick Klesyk, Ben Lane, Matt Rycraft
 * Z-ID:        Z1782152        Z1806979  Z1818053 
 * Description: This console application is an MMORPGFPSMOBA.
 *              It demonstrates several basic C# capabilities.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NBN_Assign1
{
    // The types of items a character can have.
    public enum ItemType {
        Helmet, Neck, Shoulders, Back, Chest, Wrists,
        Gloves, Belt, Pants, Boots, Ring, Trinket
    };

    // The available races for the character.
    public enum Race {
        Orc, Troll, Tauren, Forsaken
    }

    class Program {
        private static uint MAX_ILVL = 360;             // Max item level.
        private static uint MAX_PRIMARY = 200;          // Max primary level.
        private static uint MAX_STAMINA = 275;          // Max stamina level.
        private static uint MAX_LEVEL = 60;             // Highest level a player can be.
        private static uint GEAR_SLOTS = 14;            // The amount of gear slots a player has.
        private static uint MAX_INVENTORY_SIZE = 20;    // The max amount of objects that can be held in the inventory.

        private static Dictionary<uint, string> guildList = new Dictionary<uint, string>();     // Guild dictionary.
        private static Dictionary<uint, Item> gearList = new Dictionary<uint, Item>();          // Item dictionary.
        private static Dictionary<uint, Player> playerList = new Dictionary<uint, Player>();    // Player dictionary.

        static void Main(string[] args) {

            // Build Guild Dictionary
            using (var reader = new StreamReader(@"..\..\Resources\guilds.txt")) {
                string line;

                while ((line = reader.ReadLine()) != null) {
                    string[] itemTokens = line.Split('\t');

                    guildList.Add(Convert.ToUInt32(itemTokens[0]), itemTokens[1]);
                }
            }

            // Build item dictionary.
            using (var reader = new StreamReader(@"..\..\Resources\equipment.txt")) {
                string line;

                while ((line = reader.ReadLine()) != null) {
                    string[] itemTokens = line.Split('\t');

                    //uint newId, string newName, ItemType newType, uint newIlvl, uint newPrimary, uint newStamina, uint newRequirement, string newFlavor
                    Item item = new Item(
                        Convert.ToUInt32(itemTokens[0]),
                        itemTokens[1],
                        (ItemType)Convert.ToUInt32(itemTokens[2]),
                        Convert.ToUInt32(itemTokens[3]),
                        Convert.ToUInt32(itemTokens[4]),
                        Convert.ToUInt32(itemTokens[5]),
                        Convert.ToUInt32(itemTokens[6]),
                        itemTokens[7]
                        );

                    gearList.Add(Convert.ToUInt32(itemTokens[0]), item);
                }
            }

            // Build Player Dictionary
            using (var reader = new StreamReader(@"..\..\Resources\players.txt")) {
                string line;

                while ((line = reader.ReadLine()) != null) {
                    string[] itemTokens = line.Split('\t');

                    //uint id, string name, Race race, uint level, uint exp, uint guildID, uint[] gear, List<uint> inventory
                    uint[] gear = {
                        Convert.ToUInt32(itemTokens[6]),
                        Convert.ToUInt32(itemTokens[7]),
                        Convert.ToUInt32(itemTokens[8]),
                        Convert.ToUInt32(itemTokens[9]),
                        Convert.ToUInt32(itemTokens[10]),
                        Convert.ToUInt32(itemTokens[11]),
                        Convert.ToUInt32(itemTokens[12]),
                        Convert.ToUInt32(itemTokens[13]),
                        Convert.ToUInt32(itemTokens[14]),
                        Convert.ToUInt32(itemTokens[15]),
                        Convert.ToUInt32(itemTokens[16]),
                        Convert.ToUInt32(itemTokens[17]),
                        Convert.ToUInt32(itemTokens[18]),
                        Convert.ToUInt32(itemTokens[19])
                    };

                    Player player = new Player(
                        Convert.ToUInt32(itemTokens[0]),
                        itemTokens[1],
                        (Race)Convert.ToUInt32(itemTokens[2]),
                        Convert.ToUInt32(itemTokens[3]),
                        Convert.ToUInt32(itemTokens[4]),
                        Convert.ToUInt32(itemTokens[5]),
                        gear,
                        new List<uint>()
                        );

                    playerList.Add(Convert.ToUInt32(itemTokens[0]), player);
                }
            }
            Menu();
        }
        //display all menu options for player to choose from
        public static void Menu() {
            Console.WriteLine("1. Print All Players -- print a list of all Players.");
            Console.WriteLine("2. Print All Guilds -- print the names of all Guilds.");
            Console.WriteLine("3. Print All Gear -- print a list of all Items.");
            Console.WriteLine("4. Print Gear List for Player -- get a Player name and print their gear list.");
            Console.WriteLine("5. Leave Guild -- get a Player name and leave their Guild (only if they are in one).");
            Console.WriteLine("6. Join Guild -- get a Player and Guild name, and have that Player \"join\".");
            Console.WriteLine("7. Equip Gear -- get a Player and Item name, then have the Player attempt to equip.");
            Console.WriteLine("8. Unequip Gear -- get a Player name and Item Slot (see sample output) and attempt to remove gear.");
            Console.WriteLine("9. Award Experience -- get a Player name and experience amount to award.");
            Console.WriteLine("10. Quit -- triggered by entering \"10\", \"q\", \"Q\", \"quit\", \"Quit\", \"exit\", or \"Exit\".");

            string ans = Console.ReadLine().ToLower();

            if (ans == "10" || ans == "q" || ans == "quit" || ans == "exit" ) {
                ans = "quit";
                return;
            }

            switch (ans) {
                case "1":
                    PrintAllPlayers();
                    break;
                case "2":
                    PrintAllGuilds();
                    break;
                case "3":
                    PrintAllItems();
                    break;
                case "4":
                    Boolean repeat = false;
                    Console.WriteLine("Enter the player name: ");
                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);
                        if (player != null) {
                            repeat = false;
                            player.PrintGearList();
                        } else {
                            Console.WriteLine("Enter a valid player name: ");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "5":
                    repeat = false;
                    Console.WriteLine("Enter the player name: ");
                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);
                        if (player != null) {
                            repeat = false;
                            player.LeaveGuild();
                        } else {
                            Console.WriteLine("Enter a valid player name: ");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "6":
                    repeat = false;
                    Console.WriteLine("Enter the player name: ");
                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);
                        if (player != null) {
                            repeat = false;

                            Console.WriteLine("Enter the guild name: ");
                            string guildName = Console.ReadLine();
                            uint guildID = GetGuildIDFromString(guildName);
                            if (guildList.ContainsKey(guildID)) {
                                player.JoinGuild(guildID);
                            } else {
                                Console.WriteLine("That guild does not exit!" );
                            }

                        } else {
                            Console.WriteLine("Enter a valid player name: ");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "7":
                    repeat = false;
                    Console.WriteLine("Enter the player name: ");
                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);
                        if (player != null) {
                            repeat = false;

                            Console.WriteLine("Enter the item name: ");
                            string itemName = Console.ReadLine();
                            uint itemID = GetItemFromString(itemName).Id;
                            if (gearList.ContainsKey(itemID)) {
                                player.EquipGear(itemID);
                            } else {
                                Console.WriteLine("That item does not exit!");
                            }

                        } else {
                            Console.WriteLine("Enter a valid player name: ");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "8":
                    repeat = false;
                    Console.WriteLine("Enter the player name: ");
                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);
                        if (player != null) {
                            repeat = false;

                            Console.WriteLine("Enter the slot to unequip: ");
                            Console.WriteLine("0 = Helmet");
                            Console.WriteLine("1 = Neck");
                            Console.WriteLine("2 = Shoulders");
                            Console.WriteLine("3 = Back");
                            Console.WriteLine("4 = Chest");
                            Console.WriteLine("5 = Wrist");
                            Console.WriteLine("6 = Gloves");
                            Console.WriteLine("7 = Belt");
                            Console.WriteLine("8 = Pants");
                            Console.WriteLine("9 = Boots");
                            Console.WriteLine("10 = Ring 1");
                            Console.WriteLine("11 = Ring 2");
                            Console.WriteLine("12 = Trinket 1");
                            Console.WriteLine("13 = Trinket 2");

                            int itemSlot = Convert.ToInt32(Console.ReadLine());
                            if (itemSlot >= 0 && itemSlot <= 13) {
                                player.UnequipGear(itemSlot);
                                Console.WriteLine("Slot " + itemSlot + " has been unequiped from " + player.Name);
                            } else {
                                Console.WriteLine("Slot doesn't exist");
                            }

                        } else {
                            Console.WriteLine("Enter a valid player name: ");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "9":
                    repeat = false;

                    Console.WriteLine("Enter the player name: ");

                    do {
                        string playerName = Console.ReadLine();

                        Player player = GetPlayerFromString(playerName);

                        if (player != null) {
                            repeat = false;

                            Console.WriteLine("Enter the amount of experience: ");
                            uint exp = Convert.ToUInt32(Console.ReadLine());
                            player.Exp = exp;
                        } else {
                            Console.WriteLine("Enter a valid player name.");
                            repeat = true;
                        }
                    } while (repeat);
                    break;
                case "t":
                    SortStuff();
                    break;
                case "quit":
                    Console.WriteLine("Thank you for playing this awesome game!!!");
                    return;
                default:
                    break;
            }
            Menu();
        }

        //display all players in player directory
        public static void PrintAllPlayers() {
            foreach(KeyValuePair<uint, Player> entry in playerList) {
                Console.WriteLine(entry.Value.ToString());
            }
        }
        //display all guilds in guild directory
        public static void PrintAllGuilds() {
            foreach (KeyValuePair<uint, string> entry in guildList) {
                Console.WriteLine(entry.Value.ToString());
            }
        }
        //display all items in item directory
        public static void PrintAllItems() {
            foreach (KeyValuePair<uint, Item> entry in gearList) {
                Console.WriteLine(entry.Value.ToString());
            }
        }
        //display specific player based on user entry 
        public static Player GetPlayerFromString(string playerName) {
            foreach(KeyValuePair<uint, Player> entry in playerList) {
                if (entry.Value.Name == playerName) {
                    return entry.Value;
                }
            }
            return null;
        }
        //display specific item based on user entry 
        public static Item GetItemFromString(string itemName) {
            foreach (KeyValuePair<uint, Item> entry in gearList) {
                if (entry.Value.Name == itemName) {
                    return entry.Value;
                }
            }
            return null;
        }
        //grab guild ID based on user entry 
        public static uint GetGuildIDFromString(string guildName) {
            foreach (KeyValuePair<uint, string> entry in guildList) {
                if (entry.Value == guildName) {
                    return entry.Key;
                }
            }
            return 0;
        }
        //sorts players and items and displays new order
        public static void SortStuff() {
            SortedSet<Player> sortedPlayer = new SortedSet<Player>();
            SortedSet<Item> sortedItems = new SortedSet<Item>();

            foreach(KeyValuePair<uint, Player> entry in playerList) {
                sortedPlayer.Add(entry.Value);
            }

            foreach (KeyValuePair<uint, Item> entry in gearList) {
                sortedItems.Add(entry.Value);
            }

            Console.WriteLine("Sorted Players: ");
            foreach (Player pl in sortedPlayer) {
                Console.WriteLine(pl.ToString());
            }

            Console.WriteLine("\n\nSorted Items: ");
            foreach (Item pl in sortedItems) {
                Console.WriteLine(pl.ToString());
            }

        }

        /*
         * Class:       Item
         * Description: Game item that a player can have in their inventory and also equip.
         */
        public class Item : IComparable {
            readonly uint id;   // ID of item.
            string name;        // Name of item.
            ItemType type;      // Type of item.
            uint ilvl;          // Item level.
            uint primary;       // Primary stat.
            uint stamina;       // Stamina of item.
            uint requirement;   // Player level required to equip item.
            string flavor;      // Description of item.

            public Item() {
                this.id = 0;            
                Name = "";         
                IType = 0;          
                ILevel = 0;          
                Primary = 0;       
                Stamina = 0;       
                Requirement = 0;   
                Flavor = "";       
            }

            public Item(uint newId, string newName, ItemType newType, uint newIlvl, uint newPrimary, uint newStamina, uint newRequirement, string newFlavor) {
                this.id = newId;
                Name = newName;
                IType = newType;
                ILevel = newIlvl;
                Primary = newPrimary;
                Stamina = newStamina;
                Requirement = newRequirement;
                Flavor = newFlavor;
            }

            /*
             *  Property:       Id
             *  
             *  Description:    Gets ID of item.
             */
            public uint Id {
                get { return this.id; }
            }

            /*
             *  Property:       Name
             *  
             *  Description:    Gets and sets name of item.
             */
            public string Name {
                set { this.name = value; }
                get { return this.name; }
            }

            /*
             *  Property:       IType
             *  
             *  Description:    Gets and sets the type of the item.
             */
            public ItemType IType {
                set {
                    // If item type exists, then set item.
                    if ((int)value >= 0 && (int)value <= 12) {
                    this.type = value;
                    } else {
                        this.type = 0;
                    }
                }
                get { return this.type; }
            }

            /*
             *  Property:       ILevel
             *  
             *  Description:    Gets and sets level of item.
             */
            public uint ILevel {
                set {
                    // If item's level is permissible, the set item level.
                    if (value >= 0 && value <= MAX_ILVL) { 
                        this.ilvl = value;
                    } else {
                        this.ilvl = 0;
                    }
                }
                get { return this.ilvl; }
            }

            /*
             *  Property:       Primary
             *  
             *  Description:    Gets and sets primary stat of item.
             */
            public uint Primary {
                set {
                    if (value >= 0 && value <= MAX_PRIMARY) {
                        this.primary = value;
                    } else {
                        this.primary = 0;
                    }
                }
                get { return this.primary; }
            }

            /*
            *  Property:       Stamina
            *  
            *  Description:    Gets and sets stamina stat of item.
            */
            public uint Stamina {
                set {
                    if (value >= 0 && value <= MAX_STAMINA) {
                        this.stamina = value;
                    } else {
                        this.stamina = 0;
                    }
                }
                get { return this.stamina; }
            }

            /*
            *  Property:       Requirement
            *  
            *  Description:    Gets and sets requirement stat of item.
            */
            public uint Requirement {
                set {
                    if (value >= 0 && value <= MAX_LEVEL) {
                        this.requirement = value;
                    } else {
                        this.requirement = 0;
                    }
                }
                get { return this.requirement; }
            }

            /*
            *  Property:       Flavor
            *  
            *  Description:    Gets and sets flavor (description) of item.
            */
            public string Flavor {
                set { this.flavor = value; }
                get { return this.flavor; }
            }

            /*
            *  Method:          CompareTo
            *  Arguments:       obj   (Object to compare.)
            *  
            *  Description:     IComparable method that needs to be defined so that Item class
            *                   objects can be ordered.
            */
            public int CompareTo(object obj) {
                // Check for null value.
                if (obj == null) return 1;

                // Convert obj to Item.
                Item rightOp = obj as Item;

                // Make sure conversion of obj to Item was successful.
                if (rightOp != null) {
                    return Name.CompareTo(rightOp.Name);
                } else {
                    throw new ArgumentException("You're comparing apples and oranges!");
                }
            }
            
            /*
             *  Method: ToString
             *  
             *  Description:    ToString implementation for Item class.
             */
            public override string ToString() {
                return "" + String.Format("{0,-13}", "(" + this.IType + ") ") +
                       "" + String.Format("{0,-30}", this.Name) +
                       "" + String.Format("{0,-5}", "|" + this.ILevel + "| ") +
                       "" + String.Format("{0,-5}", "--" + this.Requirement + "-- ") + "\n" +
                       "    " + "\"" + this.Flavor + "\"" + "";
            }
        }

        /*
         * Class:       Player
         * Description: Game character.
         */
        public class Player : IComparable {
            readonly uint id;        // ID of player.
            readonly string name;    // Name of player.
            readonly Race race;      // Race of player.
            uint level;              // Level of player.
            uint exp;                // Player's accumulated experience.
            uint guildID;            // ID of guild that player is in.
            private uint[] gear;     // Player's gear slots. 
            List<uint> inventory;    // Player's inventory.
            Boolean ring = false;    // Used for determining which ring slot to populate.
            Boolean trinket = false; // Used for determining which ring slot to populate.

            public Player() {
                this.id = 0;
                this.name = "Ben";
                this.race = 0;
                this.level = 3;
                this.exp = 0;
                this.guildID = 0;
                this.gear = new uint[GEAR_SLOTS];
                this.inventory = null;
            }

            public Player(uint id, string name, Race race, uint level, uint exp, uint guildID, uint[] gear, List<uint> inventory) {
                this.id = id;
                this.name = name;
                this.race = race;
                this.level = level;
                this.exp = exp;
                this.guildID = guildID;
                this.gear = gear;
                this.inventory = inventory;
            }
            
            /*
            * Property: ID
            *
            * Description: Gets the ID of the player.
            */
            public uint ID {
                get { return this.id; }
            }
            /*
            * Property: Name
            *
            *Description: Gets the Name of the player.
            */
            public string Name {
                get { return this.name; }
            }
            /*
            *Property: Race
            *
            *Description: Gets the Race of the player.
            */
            public Race Race {
                get { return this.race; }
            }
            /*
            * Property: Level
            *
            * Description: Gets the Level of the player.
            */
            public uint Level {
                set {
                    if (value >= 0 && value <= MAX_LEVEL)
                        this.level = value;
                    else
                        this.level = 0;
                }

                get { return this.level; }
            }
            /*
            * Property: Exp
            *
            * Description: Gets the Exp of the player.
            */
            public uint Exp {
                set {

                    if (Level == MAX_LEVEL) {
                        return;
                    } 

                    // Add new experience points to existing experience points.
                    this.exp += value;


                    // Determine what the next level-up experience threshold is.
                    uint nextLevelExp = (this.level * 1000);

                    // While the total character experience is greater than level-up threshold, level up and readjust threshold.
                    while (this.exp >= nextLevelExp) {
                        // Level up character by 1.
                        this.Level += 1;
                        Console.WriteLine("Ding!");

                        //XP = 35000
                        //NextLvL = 10000
                        this.exp = this.exp - nextLevelExp;

                        // Redetermine what the next level-up experience threshold is, now that the character is 1 level higher.
                        nextLevelExp = (this.level * 1000);
                    }
                }

                get { return this.exp; }
            }
            /*
            * Property: GuildID
            *
            * Description: Gets the Guild ID of the guild the player is in.
            */
            public uint GuildID {
                get { return guildID; }
                set { guildID = value; }
            }

            // Indexer for Player class, which is based on gear.
            public uint this[int i] {
                set { gear[i] = value; }
                get { return gear[i]; }
            }
            //will compare two different players 
            public int CompareTo(object newRightOp) {
                if (newRightOp == null) return 1;

                Player rightOp = newRightOp as Player;

                if (rightOp != null)
                    return this.name.CompareTo(rightOp.name);
                else
                    throw new ArgumentException("The argument being compared is not of type Player.");
            }
            /*
            * Property: EquipGear
            *
            * Description: Equips gear to player.
            */
            public void EquipGear(uint newGearID) {
                if (!gearList.ContainsKey(newGearID)) {
                    return;
                }

                Item item = gearList[newGearID];
                if (this.Level < item.Requirement) {
                    throw new Exception("You are too weak to equip this item.");
                }

                uint slot = (uint)item.IType;

                if (item.IType.Equals(ItemType.Ring)) {
                    // Figure out which slot
                    if (ring) {
                        slot = 11;
                    } else {
                        slot = 10;
                    }
                    ring = !ring;

                }
                if (item.IType.Equals(ItemType.Trinket)) {
                    // Figure out which slot
                    if (trinket) {
                        slot = 13;
                    } else {
                        slot = 12;
                    }
                    trinket = !trinket;
                }
                gear[slot] = newGearID;

                //Console.WriteLine(gear[(uint)item.getType()].ToString());
                //Console.WriteLine(item.getName());
            }
            /*
            * Property: UnequipGear
            *
            * Description: Unequips Gear from player.
            */
            public void UnequipGear(int gearSlot) {
                if (gear[gearSlot] == 0) {
                    return;
                }

                Item item = gearList[gear[gearSlot]];

                PlaceInInventory(item);
                gear[gearSlot] = 0;

                return;
            }
            /*
            * Property: PlaceInInventory
            *
            * Description: Takes item and places item in player inventory.
            */
            public void PlaceInInventory(Item item) {
                if (inventory.Count >= MAX_INVENTORY_SIZE) {
                    throw new Exception("Your inventory is full!");
                }

                // Adds the item into the inv.
                inventory.Add(item.Id);

                return;
            }
            /*
            * Property: PrintGearList
            *
            * Description: Prints all the gear the player has.
            */         
            public void PrintGearList () {
                //Console.WriteLine("Player Name: " + Name + " lvl. (" + Level + ")");
                Console.WriteLine(ToString());
                Console.WriteLine("Gear: ");
                String msg = "";
                String type = "";
                Item item;
                for (int i = 0; i < gear.Length; i++) {
                    msg = "empty";
                    if (i < 10) {
                        type = ((ItemType)i).ToString();
                    } else {
                        switch (i) {
                            case 10:
                            case 11:
                                type = "Ring";
                                break;
                            case 12:
                            case 13:
                                type = "Trinket";
                                break;
                            default:
                                type = "You borked it";
                                break;
                        }
                    }
                        // Dic contains id from gear slot (i)
                    if (gearList.ContainsKey(gear[i])) {
                        item = gearList[gear[i]];
                        msg = item.Name;
                        Console.WriteLine(item.ToString());
                    } else {
                        Console.WriteLine("" + String.Format("{0,-20}", "(" + type + ") ") + " " + msg);
                    }
                }
            }
            /*
            * Property: JoinGuild
            *
            * Description: Player can join any guild in the guild directory.
            */
            public void JoinGuild(uint guildID) {
                // Need to leave before you can join another guild
                if (this.GuildID != 0) {
                    LeaveGuild();
                }

                string guildName = "N/A";
                if (guildList.ContainsKey(guildID)) {
                    this.GuildID = guildID;
                    guildName = guildList[this.GuildID];
                    Console.WriteLine(this.Name + " has joined the guild: " + guildName);
                } else {
                    throw new Exception("Unable to find guild with ID: " + guildID);
                }


            }
            /*
            * Property: LeaveGuild
            *
            * Description: Removes player from current guild.
            */
            public void LeaveGuild() {
                // Need to be in a guild to leave a guild
                if (this.GuildID == 0) {
                    throw new Exception("You must be in a guild to leave a guild...");
                }
                string guildName = "N/A";
                if (guildList.ContainsKey(this.guildID)) {
                    guildName = guildList[this.GuildID];
                }

                this.GuildID = 0;

                Console.WriteLine(this.Name + " has left the guild: " + guildName);

            }

            public override string ToString() {
                string guildName = "None";
                if (guildList.ContainsKey(this.GuildID)) {
                    guildName = guildList[this.GuildID];
                }
                return "" + String.Format("Name: {0,-20}", this.Name) +
                       "" + String.Format("Race: {0,-20}", this.Race) +
                       "" + String.Format("Level: {0,-15}", this.Level) +
                       "" + String.Format("Guild: {0,-20}", guildName) + "";
            }

        }
    }
}
