﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PillRefresh {
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
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PillRefresh.Resource", typeof(Resource).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Another pill calculation? (y)es / (n)o.
        /// </summary>
        internal static string AnotherCalculation {
            get {
                return ResourceManager.GetString("AnotherCalculation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Did not understand reply!.
        /// </summary>
        internal static string ErrorAnotherCalculation {
            get {
                return ResourceManager.GetString("ErrorAnotherCalculation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a number!.
        /// </summary>
        internal static string ErrorNotANumber {
            get {
                return ResourceManager.GetString("ErrorNotANumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reminder is older tan today!.
        /// </summary>
        internal static string ErrorReminder {
            get {
                return ResourceManager.GetString("ErrorReminder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No name was given!.
        /// </summary>
        internal static string ErrorReminderName {
            get {
                return ResourceManager.GetString("ErrorReminderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You run out of pills on {0}..
        /// </summary>
        internal static string PillEnd {
            get {
                return ResourceManager.GetString("PillEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How many pills have you left?.
        /// </summary>
        internal static string PillsLeft {
            get {
                return ResourceManager.GetString("PillsLeft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How many do you need to take per day?.
        /// </summary>
        internal static string PillsPerDay {
            get {
                return ResourceManager.GetString("PillsPerDay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to How many days before that do you want a reminder?.
        /// </summary>
        internal static string ReminderDays {
            get {
                return ResourceManager.GetString("ReminderDays", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What should be the name of the reminder?.
        /// </summary>
        internal static string ReminderName {
            get {
                return ResourceManager.GetString("ReminderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome to pill calculation!.
        /// </summary>
        internal static string Welcome {
            get {
                return ResourceManager.GetString("Welcome", resourceCulture);
            }
        }
    }
}
