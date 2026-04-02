using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EventManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Event> events = new List<Event>();
            int nextId = 1;

            Console.WriteLine("Welcome to the Event Management System!");
            Console.WriteLine("Type 'help' to see available commands.");

            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "exit")
                    break;

                // HELP
                else if (input == "help")
                {
                    Console.WriteLine("Commands:");
                    Console.WriteLine("create | list | get <id> | update <id> | delete <id> | search <keyword> | exit");
                }

                // CREATE
                else if (input == "create")
                {
                    string name;
                    do
                    {
                        Console.Write("Enter Event Name: ");
                        name = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(name))
                            Console.WriteLine("Event name cannot be empty.");
                    }
                    while (string.IsNullOrWhiteSpace(name));

                    Console.Write("Enter Description (optional): ");
                    string description = Console.ReadLine();

                    Console.Write("Enter Date (yyyy-MM-dd): ");
                    DateTime date;
                    while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        Console.Write("Invalid date format. Try again: ");
                    }

                    Console.Write("Enter Location (optional): ");
                    string location = Console.ReadLine();

                    // Duplicate check
                    if (events.Any(e => e.Name.ToLower() == name.ToLower() && e.Date == date))
                    {
                        Console.WriteLine("An event with the same name and date already exists.");
                        continue;
                    }

                    Event newEvent = new Event
                    {
                        Id = nextId++,
                        Name = name,
                        Description = description,
                        Date = date,
                        Location = location
                    };

                    events.Add(newEvent);
                    Console.WriteLine("Event created successfully!");
                }

                // LIST
                else if (input == "list")
                {
                    if (!events.Any())
                    {
                        Console.WriteLine("No events found. Try creating one using 'create'.");
                    }
                    else
                    {
                        Console.WriteLine($"Total Events: {events.Count}");

                        foreach (var ev in events.OrderBy(e => e.Date))
                        {
                            Console.WriteLine($"ID: {ev.Id}, Name: {ev.Name}, Date: {ev.Date:yyyy-MM-dd}, Location: {ev.Location}");
                        }
                    }
                }

                // GET
                else if (input.StartsWith("get "))
                {
                    string[] parts = input.Split(' ');

                    if (parts.Length < 2 || !int.TryParse(parts[1], out int id))
                    {
                        Console.WriteLine("Invalid ID.");
                        continue;
                    }

                    Event found = events.Find(e => e.Id == id);

                    if (found == null)
                    {
                        Console.WriteLine("Event not found.");
                    }
                    else
                    {
                        Console.WriteLine("Event Details:");
                        Console.WriteLine($"Name: {found.Name}");
                        Console.WriteLine($"Description: {found.Description}");
                        Console.WriteLine($"Date: {found.Date:yyyy-MM-dd}");
                        Console.WriteLine($"Location: {found.Location}");
                    }
                }

                // SEARCH
                else if (input.StartsWith("search "))
                {
                    string keyword = input.Substring(7);

                    var results = events
                        .Where(e => e.Name.ToLower().Contains(keyword.ToLower()));

                    if (!results.Any())
                    {
                        Console.WriteLine("No matching events found.");
                    }
                    else
                    {
                        foreach (var ev in results)
                        {
                            Console.WriteLine($"ID: {ev.Id}, Name: {ev.Name}, Date: {ev.Date:yyyy-MM-dd}");
                        }
                    }
                }

                // DELETE
                else if (input.StartsWith("delete "))
                {
                    string[] parts = input.Split(' ');

                    if (parts.Length < 2 || !int.TryParse(parts[1], out int id))
                    {
                        Console.WriteLine("Invalid ID.");
                        continue;
                    }

                    Event ev = events.Find(e => e.Id == id);

                    if (ev == null)
                    {
                        Console.WriteLine("Event not found.");
                    }
                    else
                    {
                        Console.Write("Are you sure you want to delete? (yes/no): ");
                        string confirm = Console.ReadLine().ToLower();

                        if (confirm == "yes")
                        {
                            events.Remove(ev);
                            Console.WriteLine("Event deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Delete cancelled.");
                        }
                    }
                }

                // UPDATE
                else if (input.StartsWith("update "))
                {
                    string[] parts = input.Split(' ');

                    if (parts.Length < 2 || !int.TryParse(parts[1], out int id))
                    {
                        Console.WriteLine("Invalid ID.");
                        continue;
                    }

                    Event ev = events.Find(e => e.Id == id);

                    if (ev == null)
                    {
                        Console.WriteLine("Event not found.");
                        continue;
                    }

                    Console.Write("Enter new Event Name (leave empty to keep current): ");
                    string name = Console.ReadLine();

                    Console.Write("Enter new Description (leave empty to keep current): ");
                    string description = Console.ReadLine();

                    Console.Write("Enter new Date (yyyy-MM-dd, leave empty to keep current): ");
                    string dateInput = Console.ReadLine();
                    DateTime? date = null;

                    if (!string.IsNullOrWhiteSpace(dateInput))
                    {
                        if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd",
                            CultureInfo.InvariantCulture, DateTimeStyles.None,
                            out DateTime parsedDate))
                        {
                            date = parsedDate;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format.");
                            continue;
                        }
                    }

                    Console.Write("Enter new Location (leave empty to keep current): ");
                    string location = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(name)) ev.Name = name;
                    if (!string.IsNullOrWhiteSpace(description)) ev.Description = description;
                    if (date.HasValue) ev.Date = date.Value;
                    if (!string.IsNullOrWhiteSpace(location)) ev.Location = location;

                    Console.WriteLine("Event updated successfully!");
                }

                // UNKNOWN
                else
                {
                    Console.WriteLine("Unknown command. Type 'help' to see commands.");
                }
            }
        }
    }
}