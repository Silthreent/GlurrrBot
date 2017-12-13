using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlurrrBotDiscord2
{
    class SQLManager
    {
        static SQLiteConnection dbConnection;

        public static void createConnection()
        {
            if(!File.Exists("GlurrrBot.sqlite"))
            {
                Console.WriteLine("Creating GlurrrBot.sqlite");
                SQLiteConnection.CreateFile("GlurrrBot.sqlite");
            }

            dbConnection = new SQLiteConnection("Data Source=GlurrrBot.sqlite;Version=3;");
        }

        public static void saveMessage(ulong messageID, string tag)
        {
            dbConnection.Open();
            {
                try
                {
                    new SQLiteCommand("insert into savedmessages (message_id, tag) values ('" + messageID + "', '" + tag + "')", dbConnection).ExecuteNonQuery();
                }
                catch(SQLiteException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            dbConnection.Close();
        }

        public static List<ulong> getMessagesByTag(string tag)
        {
            List<ulong> messages = new List<ulong>();

            dbConnection.Open();
            {
                try
                {
                    SQLiteDataReader reader = new SQLiteCommand("SELECT message_id, tag FROM savedmessages WHERE tag='" + tag + "'", dbConnection).ExecuteReader();
                    while(reader.Read())
                    {
                        messages.Add((ulong)(long)reader["message_id"]);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            dbConnection.Close();

            return messages;
        }
    }
}
