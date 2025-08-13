using SharpMonoInjector;
using System.Diagnostics;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Console.Title = "G-Injector";
            if (!IsRunAsAdministrator())
            {
                PrintLine("Please run this program as Administrator.", ConsoleColor.Red);
                PauseExit();
                return;
            }

            if (args.Length == 0)
            {
                PrintLine("Please drag a DLL file into the injector.", ConsoleColor.Yellow);
                PauseExit();
                return;
            }

            string dllPath = args[0];

            if (!File.Exists(dllPath))
            {
                PrintLine($"The file {dllPath} does not exist.", ConsoleColor.Red);
                PauseExit();
                return;
            }

            byte[] dllBytes = File.ReadAllBytes(dllPath);
            PrintLine("G-Injector - made by ventern", ConsoleColor.Blue, 5);
            PrintLine("https://discord.gg/D9fRUyt63T", ConsoleColor.Blue, 5);
            /*
            while (true)
            {
                try
                {
                    int monoProcessPID = GetMonoProcessPID();
                    if (monoProcessPID != 0)
                    {
                        Inject(dllBytes, monoProcessPID);
                        break;
                    }

                    PrintLine("Mono process not found. Checking again in 15 seconds...", ConsoleColor.Yellow);
                    Thread.Sleep(15000);
                }
                catch { }
            }
            */ // auto route to mono apps [DISABLED]
            while (true)
            {
                int ProcessPID = GetProcessPIDByName("Gorilla Tag");
                if (ProcessPID != 0)
                {
                    Thread.Sleep(5000);
                    Inject(dllBytes, ProcessPID);
                    break;
                }
                Thread.Sleep(15000);
            }
        }
        catch (Exception ex)
        {
            PrintLine($"An error occurred: {ex.Message}", ConsoleColor.Red);
            PauseExit();
        }
    }

    private static void Inject(byte[] dllBytes, int targetProcessID)
    {
        try
        {
            Injector injector = new Injector(targetProcessID);
            if (injector != null)
            {
                IntPtr injectionCode = injector.Inject(dllBytes, "Loader", "Loader", "Load");
                injector.Dispose();
                if (injectionCode != 0)
                {
                    PrintLine($"Injection Complete : {injectionCode}", ConsoleColor.Green, 5);
                    CountdownExit(3);
                }
            }

        }
        catch (Exception ex)
        {
            PrintLine($"Injection failed | Message:{ex.Message} Expection: {ex.InnerException}", ConsoleColor.Red);
        }
    }

    private static int GetMonoProcessPID()
    {
        try
        {
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.Modules.Cast<ProcessModule>().Any(m => m.ModuleName.ToLower().Contains("mono")))
                    {
                        PrintLine($"Mono process found: {process.ProcessName}, PID: {process.Id}", ConsoleColor.Green);
                        return process.Id;
                    }
                    PrintLine($"No mono process found. Checking again in 15 seconds...", ConsoleColor.Yellow);
                    return 0;
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            PrintLine($"Error checking for mono process: {ex.Message}", ConsoleColor.Red);
        }

        return 0;
    }

    private static bool IsRunAsAdministrator()
    {
        try
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    private static void PrintLine(string message, ConsoleColor color, int typingDelay = 3, bool noline = false)
    {
        try
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(typingDelay);
            }
            Console.ResetColor();

            if (!noline)
                Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.ResetColor();
            Console.WriteLine($"Error printing message: {ex.Message}");
        }
    }

    private static void CountdownExit(int seconds)
    {
        try
        {
            for (int i = seconds; i > 0; i--)
            {
                PrintLine($"Closing in {i}...", ConsoleColor.Red, 10);
                Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            PrintLine($"Error during countdown: {ex.Message}", ConsoleColor.Red);
        }
    }

    private static void PauseExit()
    {
        try
        {
            PrintLine("", ConsoleColor.Red);
            PrintLine("Press any key to close...", ConsoleColor.Red, 10);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            PrintLine($"Error while pausing for exit: {ex.Message}", ConsoleColor.Red);
        }
    }

    private static int GetProcessPIDByName(string processName)
    {
        try
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower() == processName.ToLower())
                {
                    PrintLine($"Found process with name '{processName}' found. PID: {process.Id}", ConsoleColor.Green);
                    return process.Id;
                }
            }
            PrintLine($"No process with name '{processName}' found. Checking again in 15 seconds...", ConsoleColor.Yellow);
            return 0;
        }
        catch (Exception ex)
        {
            PrintLine($"An error occurred: {ex.Message}", ConsoleColor.Yellow);
            return 0;
        }
    }
}