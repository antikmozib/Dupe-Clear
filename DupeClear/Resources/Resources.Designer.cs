﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DupeClear.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DupeClear.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search only for files whose creation dates fall within the specified range
        ///
        ///One of the fields from &quot;From&quot; and &quot;To&quot; can also be left empty to search for files whose creation dates fall before/after the specified date.
        /// </summary>
        public static string DateCreatedToolTip {
            get {
                return ResourceManager.GetString("DateCreatedToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search only for files whose modification dates fall within the specified range
        ///
        ///One of the fields from &quot;From&quot; and &quot;To&quot; can also be left empty to search for files whose modification dates fall before/after the specified date.
        /// </summary>
        public static string DateModifiedToolTip {
            get {
                return ResourceManager.GetString("DateModifiedToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Example: Files with the .exe and .dll extensions can be excluded from the search by entering &quot;*.exe;*.dll&quot;.
        /// </summary>
        public static string ExcludedExtensionsToolTip {
            get {
                return ResourceManager.GetString("ExcludedExtensionsToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Match files whose names (excluding extensions) match the specified regular expressions pattern
        ///
        ///&quot;Match Same Filename&quot; must be selected before a filename pattern can be specified.
        /// </summary>
        public static string FileNamePatternToolTip {
            get {
                return ResourceManager.GetString("FileNamePatternToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Example: Search can be limited to files with the .jpg, .png and .bmp extensions only by entering &quot;*.jpg;*.png;*.bmp&quot;
        ///
        ///Entering &quot;*.*&quot; will search for all files regardless of their extension.
        /// </summary>
        public static string IncludedExtensionsToolTip {
            get {
                return ResourceManager.GetString("IncludedExtensionsToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File locked - its marking won&apos;t be changed automatically unless search results (when auto-marking all files) or selection (when auto-marking selected files) doesn&apos;t contain any unlocked files.
        /// </summary>
        public static string LockIconToolTip {
            get {
                return ResourceManager.GetString("LockIconToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Match files based on their MD5 checksums
        ///
        ///&quot;Match Same Size&quot; must be selected before this option can be selected.
        /// </summary>
        public static string MatchSameContentsToolTip {
            get {
                return ResourceManager.GetString("MatchSameContentsToolTip", resourceCulture);
            }
        }
    }
}
