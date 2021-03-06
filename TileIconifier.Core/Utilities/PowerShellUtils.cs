﻿#region LICENCE

// /*
//         The MIT License (MIT)
// 
//         Copyright (c) 2016 Johnathon M
// 
//         Permission is hereby granted, free of charge, to any person obtaining a copy
//         of this software and associated documentation files (the "Software"), to deal
//         in the Software without restriction, including without limitation the rights
//         to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//         copies of the Software, and to permit persons to whom the Software is
//         furnished to do so, subject to the following conditions:
// 
//         The above copyright notice and this permission notice shall be included in
//         all copies or substantial portions of the Software.
// 
//         THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//         IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//         FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//         AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//         LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//         OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//         THE SOFTWARE.
// 
// */

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using TileIconifier.Core.Shortcut;

namespace TileIconifier.Core.Utilities
{
    public static class PowerShellUtils
    {
        public static void DumpStartLayout(string outputPath)
        {
            using (var powershellInstance = PowerShell.Create())
            {
                powershellInstance.AddCommand("Export-StartLayout");
                powershellInstance.AddParameter("Path", outputPath);
                powershellInstance.Invoke();
            }

            if (!File.Exists(outputPath))
            {
                throw new PowershellException();
            }
        }

        public static void MarryAppIDs(List<ShortcutItem> shortcutsList)
        {
            using (var powershellInstance = PowerShell.Create())
            {
                powershellInstance.AddCommand("Get-StartApps");
                var results = powershellInstance.Invoke();
                foreach (var properties in results.Select(result => result.Properties))
                {
                    try
                    {
                        var shortcutItem =
                            shortcutsList.First(s => Path.GetFileNameWithoutExtension(s.ShortcutFileInfo.Name) ==
                                                     (string) properties["Name"].Value);
                        shortcutItem.AppId = properties["AppID"].Value.ToString();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }
    }
}