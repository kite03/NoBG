using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoBG
{
    class Program
    {
        static void Main(string[] args)
        {
            string cfgPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Config\\cfg.txt";

            int delay = 5000;

            while (true)
            {
                System.Threading.Thread.Sleep(delay);

                List<string> targets = File.ReadAllLines(cfgPath).ToList();

                List<Process> arrProcess = new List<Process>(Process.GetProcesses());

                for (int i = 0; i < targets.Count; i++)
                {
                    bool shouldKill = true;
                    List<Process> killList = new List<Process> { };
                    for (int k = 0; k < arrProcess.Count; k++)
                    {
                        if (arrProcess[k].ProcessName == targets[i])
                        {
                            if (!string.IsNullOrEmpty(arrProcess[k].MainWindowTitle))
                            {
                                shouldKill = false;
                            }
                            else
                            {
                                killList.Add(arrProcess[k]);
                            }
                        }
                    }

                    if (shouldKill)
                    {
                        // a hacky solution to check if the process is still open
                        // basically if a process is OPENING it runs in the background. This can lead to the program killing it.
                        System.Threading.Thread.Sleep(5000);
                        bool finalCheck = true;
                        for (int k = 0; k < killList.Count; k++)
                        {
                            if (!string.IsNullOrEmpty(killList[k].MainWindowTitle))
                            {
                                finalCheck = false;
                            }
                        }

                        if (finalCheck)
                        {
                            foreach (Process p in killList)
                            {
                                p.Kill();
                            }
                        }
                    }
                }
            }
        }
    }
}