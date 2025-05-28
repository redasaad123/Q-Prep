using Core.Interfaces;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CodeDatabaseServices
    {
        private readonly IUnitOfWork<CodeVerification> codeUnitOfWork;

        public CodeDatabaseServices(IUnitOfWork<CodeVerification> CodeUnitOfWork)
        {
            codeUnitOfWork = CodeUnitOfWork;
        }



        public async Task SaveCode(string code, string Email)
        {
            var codeVerification = new CodeVerification
            {
                Id = Guid.NewGuid().ToString(),
                Code = code,
                Email = Email
            };
            await codeUnitOfWork.Entity.AddAsync(codeVerification);
            codeUnitOfWork.Save();


        }

        public async Task<CodeVerification> GetCodeByEmail(string email)
        {
            var code = codeUnitOfWork.Entity.Find(x => x.Email == email);
            
            return code;
        }


        public async Task<bool> IsCodeValid(string email, string code)
        {
            var codeVerification = await GetCodeByEmail(email);
            if (codeVerification != null && codeVerification.Code == code)
            {
                // Optionally, you can remove the code after validation
                codeUnitOfWork.Entity.Delete(codeVerification);
                codeUnitOfWork.Save();
                return true;
            }
            return false;
        }

    }
}
