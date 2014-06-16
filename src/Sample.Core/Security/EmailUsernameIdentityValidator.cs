using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Sample.Core.Models;

namespace Sample.Core.Security
{
    public class EmailUsernameIdentityValidator : IIdentityValidator<User>
    {
        private const string EmailRegex = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$";
        private Regex reg;

        public EmailUsernameIdentityValidator()
        {
            reg = new Regex(EmailRegex, RegexOptions.IgnoreCase);
        }

        public Task<IdentityResult> ValidateAsync(User item)
        {
            IdentityResult res = IdentityResult.Success;
            if (!reg.IsMatch(item.UserName))
            {
                res = IdentityResult.Failed("Username must be a valid Email address.");
            }
            return Task.FromResult(res);
        }
    }
}
