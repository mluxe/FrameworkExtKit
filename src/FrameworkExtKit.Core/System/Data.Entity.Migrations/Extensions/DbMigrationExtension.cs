using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETCORE
using Microsoft.EntityFrameworkCore.Migrations;
#endif

namespace System.Data.Entity.Migrations {
    public static class DbMigrationExtension {

#if NET45
        public static string GetFilePath(this DbMigration dbMigration, string relativePath) {
#elif NETCORE
        public static string GetFilePath(this Migration dbMigration, string relativePath) {
#endif
            string base_dir = AppDomain.CurrentDomain.BaseDirectory;

            string[] pathOptions = new string[]{
                Path.Combine(base_dir, relativePath),
                Path.Combine(base_dir, "..", relativePath)
            };
            foreach (var path in pathOptions) {
                if (Directory.Exists(path)) {
                    return path;
                }
            }
            String msg = relativePath + " is not found in the application folders " + base_dir + "\n";

            foreach(var path in pathOptions){
                msg += path +"\n";
            }
            throw new DirectoryNotFoundException(msg);
        }

#if NETCORE
        public static void SqlFile(this Migration dbMigration, MigrationBuilder migrationBuilder, string path){
            migrationBuilder.Sql(File.ReadAllText(path));
        }
#endif
    }
}
