﻿using Kit.Azure;
using Kit.Mail;
using System.IO;

namespace Kit.Tests {
    public class Program {

        public static void Main(string[] args) {
            ConsoleClient.Setup(minLevel: LogLevel.Log);

            var azureStorageLogin = File.ReadAllLines("../../azure-storage-login.txt");

            AzureBlobClient.Setup(
                accountName: azureStorageLogin[0],
                accountKey: azureStorageLogin[1],
                containerName: "test"
            );

            var mailCredentials = File.ReadAllLines("../../mail-credentials.txt");

            MailClient.Setup(
                host: mailCredentials[0],
                port: int.Parse(mailCredentials[1]),
                userName: mailCredentials[2],
                password: mailCredentials[3],
                from: mailCredentials[4],
                to: mailCredentials[5]
            );

            Kit.Execute(ct => new Test().RunAsync(ct));
        }
    }
}
