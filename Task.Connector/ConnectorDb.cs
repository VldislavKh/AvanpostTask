﻿using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using Task.Connector.Infrastructure;
using Task.Connector.Validation;
using Task.Integration.Data.DbCommon.DbModels;
using Task.Integration.Data.Models;
using Task.Integration.Data.Models.Models;

namespace Task.Connector;

public class ConnectorDb : IConnector
{
    private TaskDbContext _context;
    private IMapper _mapper;

    public ConnectorDb()
    {
        
    }

    public void StartUp(string connectionString)
    {
        var actualConnectionString = GetActualConnectionString(connectionString);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices(actualConnectionString);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _context = serviceProvider.GetRequiredService<TaskDbContext>();

    }

    public void CreateUser(UserToCreate user)
    {
        Logger.Debug($"Создание пользователя {user.Login}");
        var newUser = _mapper.Map<User>(user);

        var validator = new UserValidator();
        try
        {
            validator.Validate(newUser, options => options.ThrowOnFailures());
        }
        catch
        {
            Logger.Error("Данные невалидны");
            throw;
        }
        _context.Users.Add(newUser);

        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Logger?.Error($"Ошибка при создании пользователя. Логин: {user.Login}.");
            throw;
        }
    }

    public IEnumerable<Property> GetAllProperties()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserProperty> GetUserProperties(string userLogin)
    {
        throw new NotImplementedException();
    }

    public bool IsUserExists(string userLogin)
    {
        throw new NotImplementedException();
    }

    public void UpdateUserProperties(IEnumerable<UserProperty> properties, string userLogin)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Permission> GetAllPermissions()
    {
        throw new NotImplementedException();
    }

    public void AddUserPermissions(string userLogin, IEnumerable<string> rightIds)
    {
        throw new NotImplementedException();
    }

    public void RemoveUserPermissions(string userLogin, IEnumerable<string> rightIds)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetUserPermissions(string userLogin)
    {
        throw new NotImplementedException();
    }

    public ILogger Logger { get; set; }

    private string GetActualConnectionString(string connectionString)
    {
        string connectionStringPattern = @"ConnectionString='([^']*)'";
        var match = Regex.Match(connectionString, connectionStringPattern);
        return match.Groups[1].Value;
    }
}