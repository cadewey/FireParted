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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace FireParted
{
    public partial class fmMain : Form
    {
        private uint _origDataSize;
        private uint _origSdcardSize;
        private uint _origCacheSize;

        private uint _dataSize;
        private uint _sdcardSize;
        private uint _cacheSize;
        private uint _availableSpace;

        private int _dataUsed;
        private int _sdcardUsed;

        //Establish a minimum /cache size to be safe
        private readonly int _minCache = 64;

        CommandManager _command;

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        public fmMain()
        {
            InitializeComponent();

            _command = new CommandManager(this);
            DisableSpinners();
            DisableButtons();
            SetWindowTheme(progDataUsage.Handle, " ", " ");
            SetWindowTheme(progSdUsage.Handle, " ", " ");

            Thread t = new Thread(new ThreadStart(RestartAdb));
            t.Start();
        }

        public void WriteToConsole(string Text)
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { WriteToConsole(Text); }
                ));
            }
            else
            {
                rtbConsole.AppendText(Text);
                rtbConsole.ScrollToCaret();
            }
        }

        private void UpdateDisplay()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { UpdateDisplay(); }
                ));
            }
            else
            {
                numData.Value = (decimal)_dataSize;
                numCache.Value = (decimal)_cacheSize;
                numSdcard.Value = (decimal)_sdcardSize;

                _availableSpace = Constants.TOTAL_SPACE - _dataSize - _cacheSize - _sdcardSize;

                lblDataUsage.Text = _dataUsed.ToString() + "MB / " + _dataSize.ToString() + "MB";
                lblSdUsage.Text = _sdcardUsed.ToString() + "MB / " + _sdcardSize.ToString() + "MB";

                progDataUsage.Maximum = (int)_dataSize;
                progDataUsage.Value = _dataUsed;

                progSdUsage.Maximum = (int)_sdcardSize;
                progSdUsage.Value = _sdcardUsed;

                lblAvailSpace.Text = _availableSpace.ToString() + "MB";
            }
        }

        private void ReadPartitions()
        {
            WriteToConsole("Reading partition table...\n");

            DisableButtons();

            Dictionary<string, uint> partitionTable = _command.ReadPartitionTable();
            Dictionary<string, int> usageTable = _command.ReadPartitionUsage();

            if (partitionTable == null || partitionTable.Count == 0)
            {
                WriteToConsole("Error: Unable to read partition table from device!\n");
                return;
            }

            _dataSize = partitionTable["data"];
            _cacheSize = partitionTable["cache"];
            _sdcardSize = partitionTable["media"];

            _dataUsed = usageTable["data"];
            _sdcardUsed = usageTable["media"];

            _origDataSize = _dataSize;
            _origSdcardSize = _sdcardSize;
            _origCacheSize = _cacheSize;

            UpdateDisplay();
            EnableSpinners();

            WriteToConsole("Done!\n");

            EnableButtons();
        }

        private void BackupData()
        {
            WriteToConsole("Creating archive of /data partition at /data/data.tgz...\n");

            DisableButtons();

            string output = _command.ArchiveDataPartition();
            if (output.StartsWith("Error:"))
            {
                WriteToConsole("Error archiving /data partition:\n");
                WriteToConsole(output + "\n");
            }
            else
            {
                WriteToConsole("Done!\n");
                WriteToConsole("Pulling archive from device...\n");

                string pullOutput = _command.PullDataArchive();

                if (pullOutput.StartsWith("Error:"))
                {
                    if (pullOutput.Contains("KB/s"))
                    {
                        WriteToConsole(pullOutput.Substring(7) + "\n");
                        WriteToConsole("Done. Backup saved to " + Directory.GetCurrentDirectory() + "\\data.tgz\n");
                        RemoveRemoteArchive();
                    }
                    else
                    {
                        WriteToConsole("Error pulling data archive:\n");
                        WriteToConsole(pullOutput + "\n");
                    }
                }
                else
                {
                    WriteToConsole(pullOutput);
                    WriteToConsole("Done!\n");
                }
            }

            EnableButtons();
        }

        private void RemoveRemoteArchive()
        {
            WriteToConsole("Deleting archive from /sdcard...");
            WriteToConsole(AdbCommand.ExecuteShellCommand("rm /sdcard/data.tgz") + "\n");
            WriteToConsole("Done.\n");
        }

        private void RestartAdb()
        {
            WriteToConsole("Restarting adb server, please wait (this should only take a few seconds)...\n");

            AdbCommand.StartServer();

            WriteToConsole("Done. Ready to go!\n");

            EnableButtons();
        }

        private void btnReadPartitions_Click(object sender, EventArgs e)
        {
            Thread readThread = new Thread(new ThreadStart(ReadPartitions));
            readThread.Start();
        }
        
        private void EnableSpinners()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { EnableSpinners(); }
                ));
            }
            else
            {
                numData.Enabled = true;
                numCache.Enabled = true;
                numSdcard.Enabled = true;
            }
        }

        private void DisableSpinners()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { DisableSpinners(); }
                ));
            }
            else
            {
                numData.Enabled = false;
                numCache.Enabled = false;
                numSdcard.Enabled = false;
            }
        }

        private void EnableButtons()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { EnableButtons(); }
                ));
            }
            else
            {
                btnBackupData.Enabled = true;
                btnReadPartitions.Enabled = true;
                btnResetValues.Enabled = true;
            }
        }

        private void DisableButtons()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(
                    delegate() { DisableButtons(); }
                ));
            }
            else
            {
                btnBackupData.Enabled = false;
                btnReadPartitions.Enabled = false;
                btnResetValues.Enabled = false;
            }
        }

        private void ValidateValues(NumericUpDown Spinner, ref uint Value, int MinValue)
        {
            int diff = (int)Spinner.Value - (int)Value;

            if (diff > 0)
            {
                if (_availableSpace == 0)
                {
                    Spinner.Value = Value;
                }
                else if (diff > _availableSpace)
                {
                    Value += _availableSpace;
                }
                else
                {
                    //This cast is okay because we know diff is greater than 0
                    Value += (uint)diff;
                }
            }
            else
            {
                Value -= (uint)(-1 * diff);

                if (Value < MinValue)
                {
                    //This cast is okay because MinValue will be at least zero
                    Value = (uint)MinValue;
                }
            }

            UpdateDisplay();
        }

        private void btnBackupData_Click(object sender, EventArgs e)
        {
            Thread backupThread = new Thread(new ThreadStart(BackupData));
            backupThread.Start();
        }

        private void numData_ValueChanged(object sender, EventArgs e)
        {
            ValidateValues(numData, ref _dataSize, _dataUsed);
        }

        private void numSdcard_ValueChanged(object sender, EventArgs e)
        {
            ValidateValues(numSdcard, ref _sdcardSize, _sdcardUsed);
        }

        private void numCache_ValueChanged(object sender, EventArgs e)
        {
            ValidateValues(numCache, ref _cacheSize, _minCache);
        }

        private void numData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                numData_ValueChanged(sender, e);
            }
        }

        private void numCache_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                numCache_ValueChanged(sender, e);
            }
        }

        private void numSdcard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                numSdcard_ValueChanged(sender, e);
            }
        }

        private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Try to clean up leftover adb process(es), if any
            AdbCommand.KillAdbServer();
        }

        private void btnResetValues_Click(object sender, EventArgs e)
        {
            _dataSize = _origDataSize;
            _cacheSize = _origCacheSize;
            _sdcardSize = _origSdcardSize;

            numData.Value = _dataSize;
            numCache.Value = _cacheSize;
            numSdcard.Value = _sdcardSize;

            UpdateDisplay();
        }
    }
}
