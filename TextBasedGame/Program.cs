namespace TextBasedGame;

internal class TextBasedGame
{
    private const int TotalRooms = 3;
    private const int ItemsPerRoom = 5;
    
    private static readonly int[] RoomInteractionLimits = [5, 3, 1];
    
    private int _currentRoom = 1;
    private int _remainingInteractions;
    private readonly bool[,] _interactedItems;
    
    private readonly Random _random;
    
    private static readonly string[][] RoomItems = new string[][]
    {
        [], // 0-index unused
        ["", "Faded Photo", "Stuffed Toy", "Stationary and Papers", "Worn Down Toys", "Old Faded Bed"], // Room 1
        ["", "Fading Polaroid", "Stringless Guitar", "Broken DS", "Maps and Brochures", "Music Box"], // Room 2
        ["", "Phone", "Coupled Wedding Rings", "Ultrasound Photos", "Empty Crib", "Final Door"] // Room 3
    };

    private static readonly string[][] PositiveObservations = new string[][]
    {
        [], // 0-index unused
        [
            "",
            "Mom and Dad... I missed you both, you were both nice to me in your own ways. I never understood what you always said but I knew you had my best intentions at heart, I wish I understood how much you loved me but maybe when we meet again...",
            "An old worn down bunny rabbit, I used to carry it all day around the house and park... I always had my grandma repair it as she told me it was something that I should always cherish. It felt well loved, doesn't it?",
            "I drew pictures day and night until my crayons broke, the flowers, the sky, the cats outside, everything felt new and I loved trying to draw what I saw each and every single day. I wasn't the best at it but it was my best.",
            "I played with my friends with these all day, it felt so nice just having them in our house when my parents weren't there. I wasn't bored out of my mind when I heard the doorbell ring and my friends visited me. These bring back those memories...",
            "There were nights were I slept well in this bed... I wished I didn't have to move and just lived in there, the sheets being my favourite characters and my stuff being within arm's reach."
        ],
        [
            "",
            "A photo of my Grandma and Grandpa, they were always talking about how I should present myself to others and thanks to them and my own learnings, I had gotten to experience so many new things. I feel like without them I couldn't be who I was back then...",
            "The old black guitar my uncle gave me on my birthday, I sucked at it at first but I always wanted to create songs for others to listen to and I got better over time to sing love songs to my crushes.",
            "A broken DS, I used to play so many games on this, having to carry it around me was fun since I got to interact with strangers and share game information with them. I wonder if I can still repair it...",
            "Maps and brochures from all the places I dreamed of visiting, Grandma and Grandpa took me to some of them during summer breaks. The beach trips, the mountain hikes, the city adventures... Each place helped me find pieces of who I wanted to become.",
            "The music box still plays that gentle lullaby... Grandma would wind it up when I was stressed about exams or heartbreak. The melody always reminded me that no matter how hard things got, there was still beauty in the world worth holding onto."
        ],
        [
            "",
            "My phone filled with messages from them, voice recordings I saved, photos of our dates and lazy Sundays. Every notification was a reminder that I wasn't alone anymore, that someone chose to build a life with me. We talked about everything and nothing at all...",
            "Our wedding rings... I remember how nervous we both were at the altar, how the rings felt too big and too small at the same time. But when we put them on, everything felt right. Like two puzzle pieces finally finding their match after searching for so long.",
            "The ultrasound photos, Our baby. Our little miracle. I remember staring at these for hours, tracing the tiny outline, imagining what they'd look like, who they'd become. We had so many plans, so many dreams we wanted to share with them...",
            "The empty crib we built together, Every piece assembled with care and love, painted in soft colors we picked out together. It was supposed to be filled with laughter and tiny hands reaching up. It represented all our hopes for the future...",
            "The final door... Beyond it lies everything after I've lived through, everything I've loved and lost. But also everything I've learned and everyone who shaped me. I'm ready to move on, carrying all of it with me without regrets."
        ]
    };

    // Negative observations for each room's items
    private static readonly string[][] NegativeObservations = new string[][]
    {
        [], // 0-index unused
        [
            "",
            "Mom and Dad... I wished you two never separated, even when you two were nice to me... You both never talked it out, I stayed late waiting at school for one of you to pick me up until grandpa had to... I love you both... still..",
            "An old worn down bunny rabbit, I never liked how weird it looked since the other kids made fun of it, but I tried my best to love it like it was a little sibling of mine but I was reckless with it, always dragging it around until grandma repaired and cleaned it for me.",
            "I tried my best to draw but I could never make it look like how I pictured it, I hated how what I drew didn't match what I had in my mind so I stopped drawing after a while, hiding my tools in the drawers that I never opened afterwards...",
            "I played with these toys alone, it felt... okay but the loneliness of the house felt too big, I placed my toys everywhere, trying to reenact scenes all by myself but after a while it got boring, my dinosaurs, my dolls, my castles, my houses, they just felt empty.",
            "There were nights where I slept horribly in this bed... Crying out that I wanted to go home, to go to where Mom and Dad was but I knew it wasn't meant to be, I always waited for those two even until now..."
        ],
        [
            "",
            "A photo of my Grandma and Grandpa... I wish I told them I loved them more often. I was so caught up in my teenage rebellion, thinking I knew better than them. Now looking at their faces, I see all the times I rolled my eyes when they just wanted to talk...",
            "The stringless guitar, I gave up on it. I kept telling myself I wasn't talented enough, that my music didn't matter. I stopped trying because I was afraid of not being good enough, and now it just sits here as a reminder of dreams I abandoned.",
            "A broken DS, I broke it in a fit of anger one night after a fight with my grandparents. I threw it against the wall and regretted it immediately. It held all my progress, my memories, my connections with others, and I destroyed it in a moment of stupidity...",
            "Maps and brochures of places I never got to visit... Grandma and Grandpa were getting older, and we kept postponing trips, saying 'next year, next summer.' But next year never came for some of them. These are just reminders of chances I missed.",
            "The music box... It reminds me of the nights I cried myself to sleep, feeling like I didn't belong anywhere. Not with Mom, not with Dad, not even with my grandparents. The melody that once comforted me became my swan song of loneliness."
        ],
        [
            "",
            "My phone... The last messages are still here. The fights we never resolved, the 'we need to talk' that came too late, the missed calls I ignored. If I could go back, I'd answer every single one but all I have now are regrets..",
            "Our wedding rings, One of them sits here alone now. I remember the promises we made, the 'forever' we swore to. But forever ended too soon, and I'm left wondering if I loved them enough, if I said 'I love you' enough times...",
            "The ultrasound photos... What could never got to take their first breath. Never got to open their eyes, never got to hear us say their name. These blurry images are all I have of what could have been, what should have been. A future that ended before it began...",
            "The empty crib that was never used... We built it with so much hope, but it stayed empty. Every time I look at it, I see the nursery we planned, the lullabies that were never sung, the goodnight kisses that never happened.",
            "The final door... I'm not ready. Behind me is things I missed, every mistake I can't undo, every person I couldn't bare to live without. How do I move forward knowing I'm leaving them all behind? How do I walk through when I'm still carrying all this weight?"
        ]
    };

    private TextBasedGame()
    {
        _interactedItems = new bool[TotalRooms + 1, ItemsPerRoom + 1];
        _remainingInteractions = RoomInteractionLimits[0];
        _random = new Random();
    }

    private void Start()
    {
        Console.WriteLine("I woke up in a blank room though it changed each time I blinked... It felt weird like I was in a dream yet I was aware of it...");
        Console.WriteLine("The items in these rooms feel familiar... Were these symbols of my past? I wonder if these were the things that helped me grow...");
        Console.WriteLine();
        
        while (_currentRoom <= TotalRooms)
        {
            PlayRoom();
        }
        
        Console.WriteLine("\nAlthough it felt so quick I wonder... Could I have done anything more? Was there a pattern or a sequence to what I should have chosen?");
        Console.WriteLine("No, that's stupid... Why do I harbor so much regrets in my heart even in the end, though... I wish I could tell them one more thing... but I don't think I would be able to, but I'm sorry. I wasn't perfect and I hope you all can forgive me... wherever you may be...");
    }
    
    private void PlayRoom()
    {
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine($"This is Room No. {_currentRoom}, it feels like a stage of my life I could never get back...");
        Console.WriteLine($"Available interactions: {_remainingInteractions}");
        Console.WriteLine(new string('=', 50));
        
        while (true)
        {
            Console.WriteLine("\nItems in the room:");
            for (var i = 1; i <= ItemsPerRoom; i++)
            {
                var status = _interactedItems[_currentRoom, i] ? " [OBSERVED]" : "";
                Console.WriteLine($"  {i}. {RoomItems[_currentRoom][i]}{status}");
            }
            
            Console.WriteLine("\nWhat should I do?");
            Console.WriteLine("1-5: Observe an item");
            Console.WriteLine("6: Move to next room");
            Console.WriteLine("7: Check interaction status");
            Console.Write("Enter your choice: ");
            
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            
            if (input == "6")
            {
                MoveToNextRoom();
                break;
            }
            else if (input == "7")
            {
                ShowStatus();
            }
            else if (int.TryParse(input, out var itemNumber) && itemNumber is >= 1 and <= 5)
            {
                InteractWithItem(itemNumber);
            }
            else
            {
                Console.WriteLine("Invalid choice! Please enter 1-7.");
            }
        }
    }
    
    private void InteractWithItem(int itemNumber)
    {
        if (_remainingInteractions <= 0)
        {
            Console.WriteLine("\n[!] You have no interactions left! Move to the next room.");
            return;
        }
        
        if (_interactedItems[_currentRoom, itemNumber])
        {
            Console.WriteLine($"\n[!] I've already observed the {RoomItems[_currentRoom][itemNumber]} in this room.");
            return;
        }
        
        // Perform interaction
        _interactedItems[_currentRoom, itemNumber] = true;
        _remainingInteractions--;
        
        var itemName = RoomItems[_currentRoom][itemNumber];
        Console.WriteLine($"\n→ You observe the {itemName}...");
        
        // Coin flip: 50% chance positive, 50% chance negative
        var isPositive = _random.Next(2) == 0;
        
        var observation = isPositive 
            ? PositiveObservations[_currentRoom][itemNumber]
            : NegativeObservations[_currentRoom][itemNumber];
        
        var observationType = isPositive ? "❤" : "💔";
        
        Console.WriteLine($"{observationType} {observation}");
        Console.WriteLine($"Remaining interactions: {_remainingInteractions}");
    }
    
    private void ShowStatus()
    {
        Console.WriteLine("\nI feel odd. It feels like a dream that I'm still awake for...");
        Console.WriteLine($"Room: {_currentRoom}");
        Console.WriteLine($"Remaining Interactions: {_remainingInteractions}");
        Console.WriteLine("\nObserved items in this room:");
        
        var anyInteraction = false;
        for (var i = 1; i <= ItemsPerRoom; i++)
        {
            if (!_interactedItems[_currentRoom, i]) continue;
            Console.WriteLine($"- {RoomItems[_currentRoom][i]}");
            anyInteraction = true;
        }
        
        if (!anyInteraction)
        {
            Console.WriteLine("(none yet)");
        }
    }
    
    private void MoveToNextRoom()
    {
        if (_currentRoom < TotalRooms)
        {
            Console.WriteLine($"\n>>> Shifting the Room to Room {_currentRoom + 1}...");
            
            // Carry over unused interactions to the next room
            var nextRoomBaseInteractions = RoomInteractionLimits[_currentRoom];
            var carriedInteractions = _remainingInteractions;
            _remainingInteractions = nextRoomBaseInteractions + carriedInteractions;
            
            Console.WriteLine($"Carried over {carriedInteractions} interaction(s) from Room {_currentRoom}");
            Console.WriteLine($"Room {_currentRoom + 1} base interactions: {nextRoomBaseInteractions}");
            Console.WriteLine($"Total interactions available: {_remainingInteractions}");
        }

        _currentRoom++;
    }

    private static void Main()
    {
        var game = new TextBasedGame();
        game.Start();
    }
}