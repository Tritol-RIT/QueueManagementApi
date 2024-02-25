﻿namespace QueueManagementApi.Application.Services.EncryptionService;

public interface IEncryptionService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}