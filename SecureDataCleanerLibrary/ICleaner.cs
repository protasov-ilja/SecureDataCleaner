﻿using System.Collections.Generic;
using SecureDataCleanerLibrary.Models.Enums;

namespace SecureDataCleanerLibrary
{
    public interface ICleaner
    {
        public string CleanSecureData(string data, string key, SecureDataLocation dataLocationType);

        /// <summary>
        /// Возвращает лист расположения данных, которые может обработать данный cleaner
        /// </summary>
        public List<SecureDataLocation> ProcessedLocations { get; }
    }
}
