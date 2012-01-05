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
        private uint _dataStart;
        private uint _deviceEnd;

        private int _dataUsed;
        private int _sdcardUsed;

        private bool _partitionTableRead = false;

        //Establish a minimum /cache size to be safe
        private readonly int _minCache = 64;

        CommandManager _command;

        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        /// <summary>
        /// Constructor; Initializes objects, disables all buttons and restarts ADB
        /// </summary>
        public fmMain()
        {
            InitializeComponent();

            _command = new CommandManager(this);
            DisableSpinners();
            DisableButtons();
            SetWindowTheme(progDataUsage.Handle, " ", " ");
            SetWindowTheme(progSdUsage.Handle, " ", " ");

            if (!File.Exists("nowarningdialog"))
            {
                fmWarningDialog warning = new fmWarningDialog();
                warning.ShowDialog(this);
            }

            this.Show();

            Thread t = new Thread(new ThreadStart(RestartAdb));
            t.Start();
        }

        /// <summary>
        /// Writes a string to the output console.
        /// </summary>
        /// <param name="Text">The string to write to the console</param>
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

        /// <summary>
        /// Updates all GUI controls (numeric controls, labels, progress meters)
        /// </summary>
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

                _availableSpace = _deviceEnd -_dataStart - _dataSize - _cacheSize - _sdcardSize;

                lblDataUsage.Text = _dataUsed.ToString() + "MB / " + _dataSize.ToString() + "MB";
                lblSdUsage.Text = _sdcardUsed.ToString() + "MB / " + _sdcardSize.ToString() + "MB";

                progDataUsage.Maximum = (int)_dataSize;
                progDataUsage.Value = _dataUsed;

                progSdUsage.Maximum = (int)_sdcardSize;
                progSdUsage.Value = _sdcardUsed;

                lblAvailSpace.Text = _availableSpace.ToString() + "MB";
            }
        }

        /// <summary>
        /// Reads the partition table from the device, and sets the values of the associated private fields.
        /// </summary>
        private void ReadPartitions()
        {
            WriteToConsole("Reading partition table...\n");

            DisableButtons();

            Dictionary<string, uint> partitionTable = _command.ReadPartitionTable();
            Dictionary<string, int> usageTable = _command.ReadPartitionUsage();

            if (partitionTable == null || partitionTable.Count == 0)
            {
                WriteToConsole("Error: Unable to read partition table from device!\n");
                EnableButtons();
                return;
            }

            _deviceEnd = partitionTable["deviceend"];
            _dataStart = partitionTable["datastart"];
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

            _partitionTableRead = true;
            EnableButtons();
        }

        /// <summary>
        /// Creates a backup of the /data partition to a local archive file.
        /// </summary>
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
                WriteToConsole("Pulling archive from device. Please wait, this can take several minutes...\n");

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

        /// <summary>
        /// Restores a local data backup to the device's /data partition.
        /// </summary>
        private void RestoreData()
        {
            DisableButtons();

            WriteToConsole("Restoring /data from backup archive...\n");
            WriteToConsole("Pushing archive to device. Please wait, this can take several minutes...\n");

            string pushOutput = _command.PushDataArchive();

            if (pushOutput.Contains("KB/s"))
            {
                WriteToConsole(pushOutput.Substring(7) + "\n");
                WriteToConsole("Extracting archive...\n");

                pushOutput = _command.ExtractDataArchive();

                if (!pushOutput.StartsWith("Error:"))
                {
                    WriteToConsole("Cleaning up...\n");
                    AdbCommand.ExecuteShellCommand("rm /data/data.tgz");
                    WriteToConsole("Done.\n");
                }
                else
                {
                    WriteToConsole("Error extracting archive.\n");
                    WriteToConsole(pushOutput);
                }
            }
            else
            {
                WriteToConsole("Error pushing archive to device.\n");
                WriteToConsole(pushOutput);
            }

            EnableButtons();
        }

        /// <summary>
        /// Removes a data.tgz archive from the device.
        /// </summary>
        private void RemoveRemoteArchive()
        {
            WriteToConsole("Deleting archive from /sdcard...");
            WriteToConsole(AdbCommand.ExecuteShellCommand("rm /sdcard/data.tgz") + "\n");
            WriteToConsole("Done.\n");
        }

        /// <summary>
        /// Restarts the ADB daemon.
        /// </summary>
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
        
        /// <summary>
        /// Enables the numeric GUI controls.
        /// </summary>
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

        /// <summary>
        /// Disables the numeric GUI controls.
        /// </summary>
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

        /// <summary>
        /// Enables the GUI's buttons.
        /// </summary>
        private void EnableButtons()
        {
            if (btnBackupData.InvokeRequired)
            {
                btnBackupData.Invoke(new Action(
                    delegate() { EnableButtons(); }
                ));
            }
            else
            {
                btnBackupData.Enabled = true;
                btnReadPartitions.Enabled = true;
                btnReboot.Enabled = true;

                if (File.Exists(@".\data.tgz"))
                    btnRestoreData.Enabled = true;

                if (_partitionTableRead)
                {
                    btnResetValues.Enabled = true;
                    btnApplyChanges.Enabled = true;
                }   
            }
        }

        /// <summary>
        /// Disables the GUI's buttons.
        /// </summary>
        private void DisableButtons()
        {
            if (btnBackupData.InvokeRequired)
            {
                btnBackupData.Invoke(new Action(
                    delegate() { DisableButtons(); }
                ));
            }
            else
            {
                btnBackupData.Enabled = false;
                btnReadPartitions.Enabled = false;
                btnResetValues.Enabled = false;
                btnApplyChanges.Enabled = false;
                btnRestoreData.Enabled = false;
                btnReboot.Enabled = false;
            }
        }

        /// <summary>
        /// Runs validation on the value entered into a numeric control.
        /// </summary>
        /// <param name="Spinner">The spinner whose value was changes</param>
        /// <param name="Value">The newly entered value</param>
        /// <param name="MinValue">he minimum allowed value for the current spinner</param>
        /// <remarks>
        /// This is just a basic sanity check to ensure that the values selected don't try to create
        /// an impossible stuation. It will check to make sure the maximum allowable space of all three
        /// combined partitions is not exceeded, and will prevent the partition from shrinking to be
        /// smaller than its current contents.
        /// </remarks>
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

        /// <summary>
        /// Performs the device repartition.
        /// </summary>
        private void RepartitionDevice()
        {
            DisableButtons();

            try
            {
                Dictionary<string, int> beginEndValues = CalculateBeginEndValues();

                WriteToConsole("Writing new partition table to device, please don't power down or unplug your Kindle.\n");

                _command.RepartitionPreparation();
                _command.ResizeSdcard(beginEndValues["mediabegin"], beginEndValues["mediaend"]);
                _command.RepartitionData(beginEndValues["databegin"], beginEndValues["dataend"]);
                _command.RepartitionCache(beginEndValues["cachebegin"], beginEndValues["cacheend"]);

                WriteToConsole("\nDone! Parition table successfully written to device.\n");
                WriteToConsole("Don't forget to restore your /data partition if you made a backup!\n");
            }
            catch (PartitionException pEx)
            {
                WriteToConsole(pEx.Message.Replace("\r", "\n"));
                WriteToConsole("**Please check your partition table before rebooting!**\n");
            }
            finally
            {
                EnableButtons();
            }
        }

        /// <summary>
        /// Calculates the beginning and ending values for each partition based on their entered sizes.
        /// </summary>
        /// <returns>
        /// A dictionary of partitions to values.
        /// </returns>
        private Dictionary<string, int> CalculateBeginEndValues()
        {
            Dictionary<string, int> values = new Dictionary<string, int>();

            values.Add("databegin", 849);
            values.Add("dataend", 849 + (int)_dataSize);
            values.Add("cachebegin", values["dataend"]);
            values.Add("cacheend", values["cachebegin"] + (int)_cacheSize);
            values.Add("mediabegin", values["cacheend"]);
            values.Add("mediaend", values["mediabegin"] + (int)_sdcardSize);

            return values;
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

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This will apply your partition changes to the device. Be aware that this WILL erase " +
                "your data partition and may also result in data loss from your sdcard partition. " +
                "Please make sure you have backed up anything you don't want to lose!\n\n" +
                "Apply changes?", "Warning! Data Will Be Erased!", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Thread t = new Thread(new ThreadStart(RepartitionDevice));
                t.Start();
            }
        }

        private void btnRestoreData_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(RestoreData));
            t.Start();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Reboot device now?", "Reboot", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                WriteToConsole("Rebooting device...\n");

                _partitionTableRead = false;
                DisableButtons();
                AdbCommand.Reboot();
                EnableButtons();
            }
        }
    }
}
