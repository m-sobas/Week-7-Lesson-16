using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Diary.Models.Configurations
{
    public static class DBConnection
    {
        public static string ConnectionString = 
                        $"Server={Properties.Settings.Default.Host}\\{Properties.Settings.Default.ServerName};" +
                        $"Database={Properties.Settings.Default.Database};" +
                        $"User Id={Properties.Settings.Default.UserId};" +
                        $"Password={Properties.Settings.Default.Password}";
    }
}
