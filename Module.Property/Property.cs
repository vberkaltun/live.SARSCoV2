﻿using System.Collections.Generic;
using System.Reflection;

namespace live.SARSCoV2.Module.Property
{
    class Property<T> : IProperty<T>
    {
        #region Properties

        public static string ClassName => typeof(T).FullName;

        private T File { get; set; }
        private Dictionary<string, object> Properties { get; set; }

        #endregion

        #region Methods

        public Property(T file)
        {
            File = file;
            Properties = new Dictionary<string, object>();
        }

        public T GetFile() => File;
        public Dictionary<string, object> GetProperties()
        {
            // clear first
            Properties.Clear();

            foreach (var item in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                Properties.Add(item.Name, File.GetType().GetProperty(item.Name).GetValue(File, null));

            return Properties;
        }

        #endregion
    }
}