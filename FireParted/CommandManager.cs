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

        public string ArchiveDataPartition()
        {
            //Try to mount /data and /sdcard, the result is pretty much irrelevant since tar will error out if need be
            _parent.WriteToConsole(AdbCommand.ExecuteShellCommand(Constants.MOUNT_DATA).Replace("\r", "\n") + "\n");
            _parent.WriteToConsole(AdbCommand.ExecuteShellCommand(Constants.MOUNT_SDCARD).Replace("\r", "\n") + "\n");
            return AdbCommand.ExecuteShellCommandWithOutput(Constants.TAR_CREATE, _parent);
        }

        public string PullDataArchive()
        {
            return AdbCommand.ExecuteCommand(Constants.PULL_DATA_ARCHIVE);
        }

        public void RepartitionCache(int begin, int end)
        {
            _parent.WriteToConsole("Repartitioning /cache (begin=" + begin + ", end=" + end + ")...\n");

            AdbCommand.ExecuteShellCommand(Constants.UMOUNT_CACHE);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_REMOVE + Constants.CACHE_PART_NUMBER, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_MAKE_EXTFS + begin + " " + end, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.PARTED_NAME + Constants.CACHE_PART_NUMBER + " cache", _parent);
            
            _parent.WriteToConsole("Running e2fsck and tune2fs...\n");

            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT3 + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.TUNE2FS_EXT4 + Constants.CACHE_DEVICE, _parent);
            AdbCommand.ExecutePartitionCommand(Constants.E2FSCK + Constants.CACHE_DEVICE, _parent);

            _parent.WriteToConsole("\nDone! Parition table successfully written to device.\n");
        }

        private void ProcessPartitionTable(string PartInfo, Dictionary<string, uint> PartTable)
        {
            Match dataMatch = Constants.DATA_REGEX.Match(PartInfo);
            Match cacheMatch = Constants.CACHE_REGEX.Match(PartInfo);
            Match sdcardMatch = Constants.SDCARD_REGEX.Match(PartInfo);

            if (dataMatch.Captures.Count == 0 || cacheMatch.Captures.Count == 0 || sdcardMatch.Captures.Count == 0)
                return;

            PartTable.Add("data", UInt32.Parse(dataMatch.Groups[1].Value));
            PartTable.Add("cache", UInt32.Parse(cacheMatch.Groups[1].Value));
            PartTable.Add("media", UInt32.Parse(sdcardMatch.Groups[1].Value));
        }

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
