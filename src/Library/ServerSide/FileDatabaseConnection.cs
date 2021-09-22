using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Library.ServerSide
{
    /// <summary>
    /// This class acts as an intermediary between the program and a .json file, which acts as the permanent memory.
    /// </summary>
    public class FileMemory : IMemory
    {
        private readonly string path;

        private JsonData JsonData {
            get => JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(path));
            set => File.WriteAllText(path, JsonConvert.SerializeObject(value));
        }

        IEnumerable<User> IMemory.Users {
            get => JsonData.users.Select(data => data.ToUser());
        }

        SignInResult IMemory.SignIn(string name, string password)
        {
            UserData userData;
            try
            {
                userData = JsonData.users.First(data => data.name == name);
            } catch(InvalidOperationException)
            {
                return SignInResult.NotFound;
            }
            
            if (userData.password != password) return SignInResult.InvalidPassword;

            switch(userData.UserTypeFromString())
            {
                case UserType.Admin: return SignInResult.OkAdmin;
                case UserType.Entrepeneur: return SignInResult.OkEntrepeneur;
                case UserType.Company: return SignInResult.OkCompany;
            }
            throw new Exception();
        }

        public FileMemory(string path)
        {
            this.path = path;
        }
    }

    struct JsonData
    {
        public UserData[] users { get; set; }
    }

    struct UserData
    {
        public string name { get; set; }
        public string type { get; set; }
        public string password { get; set; }

        public UserType UserTypeFromString()
        {
            switch(type.Trim().ToUpper())
            {
                case "ADMIN": return UserType.Admin;
                case "ENTREPENEUR": return UserType.Entrepeneur;
                case "COMPANY": return UserType.Company;
                default: throw new Exception();
            }
        }

        public User ToUser()
        {
            UserType type = UserTypeFromString();
            return new User(type, name);
        }
    }
}
