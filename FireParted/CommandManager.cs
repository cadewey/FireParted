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
using System.Text.RegularExpressions;

namespace FireParted
{
    class CommandManager
    {
        fmMain _parent;

        public CommandManager(fmMain Parent)
        {
            _parent = Parent;
        }

        /// <summary>
        /// Reads the current partition table's values.
        /// </summary>
        /// <returns>
        /// A dictionary that maps partition names to their sizes.
        /// </returns>
        public Dictionary<string, uint> ReadPartitionTable()
        {
            Dictionary<string, uint> partTable = new Dictionary<string, uint>();

            string partInfo = AdbCommand.ExecuteShellCommand(Constants.PARTED_PRINT);

            if (!String.IsNullOrEmpty(partInfo))
            {
                _parent.WriteToConsole(partInfo.Replace("\r", ""));
                ProcessPartitionTable(partInfo, partTable);
            }

            return partTable;
        }

        /// <summary>
        /// Reads the current usage of each partition.
        /// </summary>
        /// <returns>
        /// A dictionary that maps partition names to their usage.
        /// </returns>
        public Dictionary<string, int> ReadPartitionUsage()
        {
            Dictionary<string, int> partUsage = new Dictionary<string, int>();

            AdbCommand.ExecuteShellCommand(Constants.MOUNT_DATA);
            AdbCommand.ExecuteShellCommand(Constants.MOUNT_SDCARD);

            string dataUsageInfo = AdbCommand.ExecuteShellCommand(Constants.USED_DATA);
            string sdUsageInfo = AdbCommand.ExecuteShellCommand(Constants.USED_SDCARD);

            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_DATA);
            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_SDCARD);

            if (!String.IsNullOrEmpty(dataUsageInfo) && !String.IsNullOrEmpty(sdUsageInfo))
            {
                ProcessUsageData(dataUsageInfo, sdUsageInfo, partUsage);
            }

            return partUsage;
        }

        /// <summary>
        /// Creates an archive of the /data partition
        /// </summary>
        /// <returns>
        /// The output of the archive command.
        /// </returns>
        public string ArchiveDataPartition()
        {
            //Try to mount /data and /sdcard, the result is pretty much irrelevant since tar will error out if need be
            _parent.WriteToConsole(AdbCommand.ExecuteShellCommand(Constants.MOUNT_DATA).Replace("\r", "") + "\n");
            _parent.WriteToConsole(AdbCommand.ExecuteShellCommand(Constants.MOUNT_SDCARD).Replace("\r", "") + "\n");
            return AdbCommand.ExecuteShellCommandWithOutput(Constants.TAR_CREATE, _parent);
        }

        /// <summary>
        /// Pulls the data.tgz archive from the device.
        /// </summary>
        /// <returns>
        /// he output of the pull command.
        /// </returns>
        public string PullDataArchive()
        {
            return AdbCommand.ExecuteCommand(Constants.PULL_DATA_ARCHIVE);
        }

        /// <summary>
        /// Pushes a data.tgz archive from the local machine to the device.
        /// </summary>
        /// <returns>
        /// The output of the push command.
        /// </returns>
        public string PushDataArchive()
        {
            _parent.WriteToConsole(AdbCommand.ExecuteShellCommand(Constants.MOUNT_DATA).Replace("\r", "") + "\n");
            return AdbCommand.ExecuteCommand(Constants.PUSH_DATA_ARCHIVE);
        }

        /// <summary>
        /// Extracts a data archive on the remote device, effectively restoring the /data partition.
        /// </summary>
        /// <returns>
        /// The output of the extract command.
        /// </returns>
        public string ExtractDataArchive()
        {
            return AdbCommand.ExecuteShellCommandWithOutput(Constants.TAR_EXTRACT, _parent);
        }

        /// <summary>
        /// Preps the device for repartitioning by unmounting all partitions and deleting the cache
        /// and data partitions.
        /// </summary>
        public void RepartitionPreparation()
        {
            //Unmount the data, cache, and sdcard partitions since we can't modify them
            //if they're mounted.
            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_CACHE);
            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_DATA);
            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_SDCARD);

            //Remove the data and cache partitions up front
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_REMOVE + Constants.CACHE_PART_NUMBER, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_REMOVE + Constants.DATA_PART_NUMBER, _parent);
        }

        /// <summary>
        /// Resizes the sdcard partition (this should be non-destructive to the data)
        /// </summary>
        /// <param name="begin">Where the new partition will start</param>
        /// <param name="end">Where the new partition will end</param>
        public void ResizeSdcard(int begin, int end)
        {
            _parent.WriteToConsole("Resizing /sdcard partition (begin=" + begin + ", end=" + end + ")...\n");

            AdbCommand.ExecutePartitionCommand(Constants.PARTED_RESIZE + Constants.SDCARD_PART_NUMBER + " " + begin + " " + end, _parent);

            _parent.WriteToConsole("Sdcard resized successfully.\n\n");
        }

        /// <summary>
        /// Recreates the /data partition
        /// </summary>
        /// <param name="begin">Where the new partition will start</param>
        /// <param name="end">Where the new partition will end</param>
        public void RepartitionData(int begin, int end)
        {
            _parent.WriteToConsole("Repartitioning /data (begin=" + begin + ", end=" + end + ")...\n");

            AdbCommand.ExecutePartitionCommand(Constants.PARTED_MAKE_EXTFS + begin + " " + end, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_NAME + Constants.DATA_PART_NUMBER + " userdata", _parent);

            _parent.WriteToConsole("Running e2fsck and tune2fs...\n");

            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT3 + Constants.DATA_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.DATA_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT4 + Constants.DATA_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.DATA_DEVICE, _parent);

            _parent.WriteToConsole("Data repartitioning complete.\n\n");
        }

        /// <summary>
        /// Recreates the /cache partition
        /// </summary>
        /// <param name="begin">Where the partition will start</param>
        /// <param name="end">Where the partition will end</param>
        public void RepartitionCache(int begin, int end)
        {
            _parent.WriteToConsole("Repartitioning /cache (begin=" + begin + ", end=" + end + ")...\n");

            AdbCommand.ExecutePartitionCommand(Constants.PARTED_MAKE_EXTFS + begin + " " + end, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_NAME + Constants.CACHE_PART_NUMBER + " cache", _parent);
            
            _parent.WriteToConsole("Running e2fsck and tune2fs...\n");

            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT3 + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT4 + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.CACHE_DEVICE, _parent);

            _parent.WriteToConsole("Cache repartitioning complete.\n\n");
        }

        /// <summary>
        /// Parses the device's partition table data
        /// </summary>
        /// <param name="PartInfo">The partition table string</param>
        /// <param name="PartTable">A dictionary that maps partition names to their sizes (in MB)</param>
        private void ProcessPartitionTable(string PartInfo, Dictionary<string, uint> PartTable)
        {
            Match dataMatch = Constants.DATA_REGEX.Match(PartInfo);
            Match cacheMatch = Constants.CACHE_REGEX.Match(PartInfo);
            Match sdcardMatch = Constants.SDCARD_REGEX.Match(PartInfo);
            Match totalSizeMatch = Constants.DEVICE_SIZE_REGEX.Match(PartInfo);

            if (dataMatch.Captures.Count == 0 || cacheMatch.Captures.Count == 0 
                || sdcardMatch.Captures.Count == 0 || totalSizeMatch.Captures.Count == 0)
                return;

            PartTable.Add("deviceend", UInt32.Parse(totalSizeMatch.Groups[1].Value));
            PartTable.Add("datastart", UInt32.Parse(dataMatch.Groups[1].Value));
            PartTable.Add("data", UInt32.Parse(dataMatch.Groups[2].Value));
            PartTable.Add("cache", UInt32.Parse(cacheMatch.Groups[1].Value));
            PartTable.Add("media", UInt32.Parse(sdcardMatch.Groups[1].Value));
        }

        /// <summary>
        /// Parses the output of the 'du' command
        /// </summary>
        /// <param name="dataUsage">The output from 'du /data'</param>
        /// <param name="sdUsage">The output from 'du /sdcard'</param>
        /// <param name="usage">A dictionary that maps partition names to their usage (in MB)</param>
        private void ProcessUsageData(string dataUsage, string sdUsage, Dictionary<string, int> usage)
        {
            Match dataMatch = Constants.USAGE_REGEX.Match(dataUsage);
            Match sdMatch = Constants.USAGE_REGEX.Match(sdUsage);

            if (dataMatch.Captures.Count == 0 || sdMatch.Captures.Count == 0)
                return;

            usage.Add("data", Int32.Parse(dataMatch.Groups[1].Value));
            usage.Add("media", Int32.Parse(sdMatch.Groups[1].Value));
        }
    }
}
