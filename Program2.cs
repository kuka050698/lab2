using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piazza_02
{

    public class Programs
    {
        static void Main(string[] args)
        {
            // Specify the starting folder on the command line, or in 
            // Visual Studio in the Project > Properties > Debug pane.
            //stackBasedIteration(Console.ReadLine());

            //readMinAndMaxNumbersFromFile(Console.ReadLine());

            //definingMinPrimeNumberFromFileNumbers(Console.ReadLine());

            DirectoryInfo d = new DirectoryInfo(@Console.ReadLine());

            if (d.Exists)
            {
                consoleFilesExplore(d);
            }

            Console.ReadKey();
        }

        public static void stackBasedIteration(string root)
        {
            // Data structure to hold names of subfolders to be
            // examined for files.
            Stack<string> dirs = new Stack<string>(20);

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable 
                // to ignore the exception and continue enumerating the remaining files and 
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception 
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The 
                // choice of which exceptions to catch depends entirely on the specific task 
                // you are intending to perform and also on how much you know with certainty 
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }

                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                // Perform the required action on each file here.
                // Modify this block to perform your required task.
                foreach (string file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        FileInfo fi = new FileInfo(file);
                        Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
                    }
                    catch (FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
        }

        public static void readMinAndMaxNumbersFromFile(string path)
        {
            Console.WriteLine("\nFinding of min and max numbers:");

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader rd = new StreamReader(fs);
            string[] numbers = rd.ReadLine().Split(' ');
            int min = 0, max = 0;
            bool activated = false;
            foreach (string number in numbers)
            {
                int num = int.Parse(number);
                if (!activated)
                {
                    min = num;
                    max = num;
                    activated = true;
                }
                if (num > max)
                    max = num;
                if (num < min)
                    min = num;
            }
            Console.WriteLine("Min number is " + min + ", Max number is " + max);
            fs.Close();
            rd.Close();
        }

        public static void definingMinPrimeNumberFromFileNumbers(string path)
        {
            Console.WriteLine("\nFinding of min prime number from the list of numbers:");

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader rd = new StreamReader(fs);
            string[] stringNumbers = rd.ReadLine().Split(' ');
            int[] numbers = new int[stringNumbers.Length];

            for (int i = 0; i < stringNumbers.Length; i++)
            {
                numbers[i] = int.Parse(stringNumbers[i]);
            }

            //Defining Prime numbers from the list
            int[] primeNumbers = new int[numbers.Length];
            int count = 0;
            foreach (int number in numbers)
            {
                int numberOfDivisors = 0;
                for (int i = 2; i <= number; i++)
                {
                    if (number % i == 0)
                        numberOfDivisors++;
                    if (numberOfDivisors > 1)
                        break;
                }
                if (numberOfDivisors == 1)
                {
                    primeNumbers[count] = number;
                    count++;
                }

            }

            int min = primeNumbers[0];

            for (int i = 1; i < count; i++)
            {
                if (primeNumbers[i] < min)
                    min = primeNumbers[i];
            }

            Console.WriteLine("Min prime number is " + min);
            fs.Close();
            rd.Close();
        }

        static List<DirectoryInfo> previousDirectories = new List<DirectoryInfo>();
        static bool escIsPressed = false;
        public static void consoleFilesExplore(DirectoryInfo d)
        {
            if (escIsPressed)
            {
                previousDirectories.RemoveAt(previousDirectories.Count - 1);
            }

            Console.Clear();

            try
            {
                DirectoryInfo[] directories = d.GetDirectories();
                int current = 0;
                while (true)
                {
                    for (int i = 0; i < directories.Length; i++)
                    {
                        if (current == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(directories[i].Name);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(directories[i].Name);

                        }
                    }
                    ConsoleKeyInfo pressedButton = Console.ReadKey();
                    if (pressedButton.Key == ConsoleKey.UpArrow)
                    {
                        current--;
                        if (current < 0)
                            current = directories.Length - 1;
                    }
                    else if (pressedButton.Key == ConsoleKey.DownArrow)
                    {
                        current++;
                        if (current > directories.Length - 1)
                            current = 0;
                    }
                    else if (pressedButton.Key == ConsoleKey.Enter)
                    {
                        escIsPressed = false;
                        previousDirectories.Add(d);
                        consoleFilesExplore(directories[current]);
                    }
                    else if (pressedButton.Key == ConsoleKey.Escape)
                    {
                        if (previousDirectories.Count != 0)
                        {
                            escIsPressed = true;
                            consoleFilesExplore(previousDirectories[previousDirectories.Count - 1]);
                        }
                    }
                    Console.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
        }

    }
}