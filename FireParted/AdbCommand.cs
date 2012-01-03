/*
    Copyright 2011, Carter Dewey

    This file is part of FireParted.

    FireParted is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FireParted is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FireParted.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace FireParted
{
    class AdbCommand
    {
        private static fmMain _console;

        public AdbCommand(fmMain Console)
        {
            _console = Console;
        }

        /// <summary>
        /// Starts the ADB daemon
        /// </summary>
        /// <returns>
        /// Always returns null; the callee is expecting a string, but we have nothing important to return.
        /// </returns>
        public static string StartServer()
        {
            KillAdbServer();
            
            Process proc = Process.Start(CreateAdbStartInfo("start-server"));
            proc.WaitForExit();

            return null;
        }

        /// <summary>
        /// Reboots the device (issues 'adb reboot')
        /// </summary>
        public static void Reboot()
        {
            Process proc = Process.Start(CreateAdbStartInfo("reboot"));
            proc.WaitForExit();
        }

        /// <summary>
        /// Executes an 'adb shell' command.
        /// </summary>
        /// <param name="Command">The command to execute ('shell' will be prepended)</param>
        /// <returns>
        /// The output of the command (stderr on failure, stdout on success)
        /// </returns>
        public static string ExecuteShellCommand(string Command)
        {
            string output;

            Process proc = Process.Start(CreateAdbStartInfo("shell " + Command));
            proc.WaitForExit();

            string stdErr = proc.StandardError.ReadToEnd();

            output = !String.IsNullOrEmpty(stdErr) ? "Error: " + stdErr : proc.StandardOutput.ReadToEnd();

            return output;
        }

        /// <summary>
        /// Executes a partitioning command, throwing a PartitionException if an error occurs
        /// </summary>
        /// <param name="Command">The command to execute</param>
        /// <param name="console">A fmMain object to write output to</param>
        public static void ExecutePartitionCommand(string Command, fmMain console)
        {
            string output = null;

            Process proc = Process.Start(CreateAdbStartInfo("shell " + Command));
            proc.WaitForExit();

            string stdErr = proc.StandardError.ReadToEnd();

            if (!String.IsNullOrEmpty(stdErr))
            {
                throw new PartitionException(stdErr);
            }

            output = proc.StandardOutput.ReadToEnd();

            if (!String.IsNullOrEmpty(output))
                console.WriteToConsole(output.Replace("\r", ""));
        }

        /// <summary>
        /// Executes an adb command
        /// </summary>
        /// <param name="Command">The command to execute</param>
        /// <returns>
        /// The output of the command (stderr on failure, stdout on success)
        /// </returns>
        public static string ExecuteCommand(string Command)
        {
            string output;

            Process proc = Process.Start(CreateAdbStartInfo(Command));
            proc.WaitForExit();

            string stdErr = proc.StandardError.ReadToEnd();

            output = !String.IsNullOrEmpty(stdErr) ? "Error: " + stdErr : proc.StandardOutput.ReadToEnd();

            return output;
        }

        /// <summary>
        /// Executes an 'adb shell' command, writing output asynchonously to the console window.
        /// </summary>
        /// <param name="Command">The command to execute ('shell' will be prepended)</param>
        /// <param name="Console">The fmMain object to write output to</param>
        /// <returns>
        /// The output of the command (stderr on failure, stdout on success)
        /// </returns>
        public static string ExecuteShellCommandWithOutput(string Command, fmMain Console)
        {
            string output;
            _console = Console;

            ProcessStartInfo info = CreateAdbStartInfo("shell " + Command);

            Process proc = new Process();
            proc.StartInfo = info;
            proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
            proc.Start();

            proc.BeginOutputReadLine();

            proc.WaitForExit();

            string stdErr = proc.StandardError.ReadToEnd();

            output = !String.IsNullOrEmpty(stdErr) ? "Error: " + stdErr : "";

            return output;
        }

        /// <summary>
        /// Creates a ProcessStartInfo object for an adb command
        /// </summary>
        /// <param name="Command">The command string to be executed</param>
        /// <returns>
        /// A ProcessStartInfo object for the providec command string
        /// </returns>
        private static ProcessStartInfo CreateAdbStartInfo(string Command)
        {
            ProcessStartInfo info = new ProcessStartInfo(@".\lib\adb.exe", Command);
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;

            return info;
        }

        /// <summary>
        /// Finds all running adb processes and terminates them
        /// </summary>
        public static void KillAdbServer()
        {
            Process[] adbProc = Process.GetProcessesByName("adb");

            if (adbProc.Length > 0)
            {
                foreach (Process proc in adbProc)
                {
                    proc.Kill();
                }
            }
        }

        private static void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                _console.WriteToConsole(e.Data.Replace("\r", "") + "\n");
            }
        }
    }
}
