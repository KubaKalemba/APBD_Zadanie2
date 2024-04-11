using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsValidName(firstName) || !IsValidName(lastName))
            {
                return false;
            }

            if (!IsValidEmail(email))
            {
                return false;
            }

            if (!AgeOver21(dateOfBirth))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            UpdateCreditLimit(user);
            
            if (!IsUserValid(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        } 

        private static bool IsValidName(string name)
        {
            return string.IsNullOrEmpty(name);
        }

        private static bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool AgeOver21(DateTime dateOfBirth) 
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            return age>21;
        }

        private static bool IsUserValid(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }

        private static void UpdateCreditLimit(User user)
        {
            var client = user.Client;
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
                return;
            }

            user.HasCreditLimit = true;

            if (client.Type == "ImportantClient")
            {
                using var userCreditService = new UserCreditService();
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                creditLimit *= 2;
                user.CreditLimit = creditLimit;
                return;
            }
            
            using (var userCreditService = new UserCreditService())
            {
                user.CreditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            }
        }
        
    }
}
