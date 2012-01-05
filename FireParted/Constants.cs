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
    class Constants
    {
        //Total space set aside by default for userdata + cache + media
        public static readonly uint TOTAL_SPACE = 6841;

        //Partition names and block devices (assume default ordering)
        public static readonly string DEVICE_BLOCK_ROOT = "/dev/block/mmcblk0";

        public static readonly string DATA_NAME = "userdata";
        public static readonly string DATA_DEVICE = "/dev/block/mmcblk0p10";
        public static readonly string DATA_PART_NUMBER = "10";

        public static readonly string SDCARD_NAME = "media";
        public static readonly string SDCARD_DEVICE = "/dev/block/mmcblk0p12";
        public static readonly string SDCARD_PART_NUMBER = "12";

        public static readonly string CACHE_NAME = "cache";
        public static readonly string CACHE_DEVICE = "/dev/block/mmcblk0p11";
        public static readonly string CACHE_PART_NUMBER = "11";

        //du commands
        public static readonly string USED_SDCARD = "du -csm /sdcard/";
        public static readonly string USED_DATA = "du -csm /data/";

        //Mount/umunt commands
        public static readonly string MOUNT_DATA = "mount " + DATA_DEVICE + " /data";
        public static readonly string MOUNT_SDCARD = "mount " + SDCARD_DEVICE + " /sdcard";
        public static readonly string UMOUNT_DATA = "umount /data";
        public static readonly string UMOUNT_SDCARD = "umount /sdcard";
        public static readonly string UMOUNT_CACHE = "umount /cache";

        //Various parted commands
        public static readonly string PARTED_PRINT = "parted " + DEVICE_BLOCK_ROOT + " print";
        public static readonly string PARTED_RESIZE = "parted " + DEVICE_BLOCK_ROOT + " resize ";
        public static readonly string PARTED_MAKE_EXTFS = "parted " + DEVICE_BLOCK_ROOT + " mkpartfs primary ext2 ";
        public static readonly string PARTED_NAME = "parted " + DEVICE_BLOCK_ROOT + " name ";
        public static readonly string PARTED_REMOVE = "parted " + DEVICE_BLOCK_ROOT + " rm ";

        //e2fsck command (-y flag assumes "yes" to all questions)
        public static readonly string E2FSCK = "e2fsck -fDy ";

        //tune2fs commands
        public static readonly string TUNE2FS_EXT3 = "tune2fs -j ";
        public static readonly string TUNE2FS_EXT4 = "tune2fs -O extents,uninit_bg,dir_index ";

        //tar commands
        public static readonly string TAR_CREATE = "tar -cvpzf /sdcard/data.tgz -C /data .";
        public static readonly string TAR_EXTRACT = "tar -xvpzf /data/data.tgz -C /data";

        //Pull command for getting archive file
        public static readonly string PULL_DATA_ARCHIVE = "pull /sdcard/data.tgz";
        public static readonly string PUSH_DATA_ARCHIVE = "push data.tgz /data";

        //Regular expressions for parsing parted's print command
        public static Regex DATA_REGEX = new Regex(@"10\s+(\d+)MB\s+\d+MB\s+(\d+)MB(?:\s|\w)+?userdata");
        public static Regex CACHE_REGEX = new Regex(@"11\s+\d+MB\s+\d+MB\s+(\d+)MB(?:\s|\w)+?cache");
        public static Regex SDCARD_REGEX = new Regex(@"12\s+\d+MB\s+\d+MB\s+(\d+)MB(?:\s|\w)+?media");
        public static Regex USAGE_REGEX = new Regex(@"(\d+)\s+total");
        public static Regex DEVICE_SIZE_REGEX = new Regex(@"Disk /dev/block/mmcblk0: (\d+)MB");
    }
}
