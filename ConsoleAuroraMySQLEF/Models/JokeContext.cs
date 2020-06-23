using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConsoleAuroraMySQLEF.Models
{
    public class JokeContext : DbContext, IDisposable
    {
        SshClient _client;
        ForwardedPortLocal _local;
        MySqlConnection _connection;

        public DbSet<Joke> Jokes { get; set; }

        public JokeContext(string auroraAddress, string auroraPassword, string sshTunnelAddress, string sshUserName, string keyFileName, bool isDebug = false)
        {
            var builder = new MySqlConnectionStringBuilder();
            builder.UserID = "root";
            builder.Password = auroraPassword;
            builder.Database = "jokes";
            builder.PersistSecurityInfo = true;
            builder.Port = 3306;

            if (isDebug)
            {
                // We are using our PEM file to securely access our SSH tunnel 
                // and forward all the traffic coming in to our local port 
                // that gets automatically bound then forwarding it to our Aurora cluster.
                var dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                PrivateKeyFile pkfile = new PrivateKeyFile($@"{dir}/{keyFileName}");
                _client = new SshClient(sshTunnelAddress, 22, sshUserName, pkfile);
                _client.Connect();

                if (_client.IsConnected)
                {
                    _local = new ForwardedPortLocal("127.0.0.1", auroraAddress, 3306);
                    _client.AddForwardedPort(_local);
                    _local.Start();
                }

                builder.Port = _local.BoundPort;
                builder.Server = "127.0.0.1";
            }
            else
            {
                builder.Server = System.Net.Dns.GetHostEntry(auroraAddress)
                    .AddressList[0]
                    .MapToIPv4()
                    .ToString();
                // builder.Server = "127.0.0.1";
            }

            _connection = new MySqlConnection(builder.ConnectionString);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connection);
            //optionsBuilder.UseMySql("server=localhost;database=jokes;user=;password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
