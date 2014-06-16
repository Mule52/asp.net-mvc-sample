using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sample.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Sample.Core.Security;

namespace Sample.Core.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected UserManager<User> userManager;

        protected override void Seed(DataContext context)
        {
            userManager = new UserManager<User>(new UserStore<User>(context))
            {
                UserValidator = new EmailUsernameIdentityValidator()
            };

            try
            {
                SeedRoles(context);
                SeedUsers(context);
                SeedLeads(context);
            }
            catch (DbEntityValidationException e)
            {
                WriteEntityErrorsAndThrow(e);
            }
        }

        private void SeedLeads(DataContext context)
        {
            var leads = new List<Lead>
                {
                    new Lead { FirstName = "Margaret", MiddleName = "P", LastName="Decaro", Email="MargaretPDecaro@einrot.com", Phone="320-394-4228", Address = new Address{ Street="1377 Brighton Circle Road", City="Holloway", State="MN", Zip="56249"}},
                    new Lead { FirstName = "Meghan", MiddleName = "M", LastName="Mauk", Email="MeghanMMauk@superrito.com", Phone="775-753-6304", Address = new Address{ Street="2253 Martha Ellen Drive", City="Elko", State="NV", Zip="89801"}},
                    new Lead { FirstName = "Matilde", MiddleName = "J", LastName="Underwood", Email="MatildeJUnderwood@teleworm.us", Phone="615-542-3298", Address = new Address{ Street="4182 Green Street", City="Nashville", State="TN", Zip="37209"}},
                    new Lead { FirstName = "Antonio", MiddleName = "G", LastName="Wicks", Email="AntonioGWicks@rhyta.com", Phone="484-283-1644", Address = new Address{ Street="1802 Renwick Drive", City="Allentown", State="PA", Zip="18109"}},
                    new Lead { FirstName = "Lester", MiddleName = "D", LastName="Coffman", Email="LesterDCoffman@armyspy.com", Phone="803-331-2236", Address = new Address{ Street="4092 Wexford Way", City="Columbia", State="SC", Zip="29210"}},
                    new Lead { FirstName = "Donald", MiddleName = "C", LastName="Winn", Email="DonaldCWinn@cuvox.de", Phone="386-426-1860", Address = new Address{ Street="792 Willis Avenue", City="New Smyrna Beach", State="FL", Zip="32069"}},
                    new Lead { FirstName = "Alfredo", MiddleName = "S", LastName="Sevigny", Email="AlfredoSSevigny@einrot.com", Phone="860-459-7984", Address = new Address{ Street="3815 Lochmere Lane", City="Hartford", State="CT", Zip="06103"}},
                    new Lead { FirstName = "Thomas", MiddleName = "J", LastName="Wall", Email="ThomasJWall@teleworm.us", Phone="313-275-6118", Address = new Address{ Street="3365 Nash Street", City="Southfield", State="MI", Zip="48076"}},
                    new Lead { FirstName = "Laura", MiddleName = "J", LastName="Stripling", Email="LauraJStripling@superrito.com", Phone="917-628-2484", Address = new Address{ Street="953 Duncan Avenue", City="New York", State="NY", Zip="10004"}},
                    new Lead { FirstName = "Sandra", MiddleName = "J", LastName="Reynoso", Email="SandraJReynoso@einrot.com", Phone="320-491-4336", Address = new Address{ Street="4620 Brighton Circle Road", City="St Cloud", State="MN", Zip="56301"}},
                    new Lead { FirstName = "Donna", MiddleName = "K", LastName="Harris", Email="DonnaKHarris@einrot.com", Phone="859-898-6202", Address = new Address{ Street="4949 Counts Lane", City="Cincinnati", State="KY", Zip="45202"}},
                    new Lead { FirstName = "Debra", MiddleName = "C", LastName="Daniels", Email="DebraCDaniels@jourrapide.com", Phone="918-383-9218", Address = new Address{ Street="3149 Hornor Avenue", City="Mena", State="OK", Zip="71953"}},
                    new Lead { FirstName = "Jody", MiddleName = "K", LastName="Nicholas", Email="JodyKNicholas@fleckens.hu", Phone="347-594-0065", Address = new Address{ Street="4164 Dancing Dove Lane", City="New York", State="NY", Zip="10013"}},
                    new Lead { FirstName = "Kara", MiddleName = "J", LastName="McKnight", Email="KaraJMcKnight@jourrapide.com", Phone="210-269-8922", Address = new Address{ Street="4014 Freshour Circle", City="San Antonio", State="TX", Zip="78212"}},
                    new Lead { FirstName = "Leland", MiddleName = "G", LastName="Johnson", Email="LelandGJohnson@einrot.com", Phone="408-490-0516", Address = new Address{ Street="1493 Ford Street", City="San Jose", State="CA", Zip="95134"}},
                    new Lead { FirstName = "James", MiddleName = "C", LastName="Wilson", Email="JamesCWilson@dayrep.com", Phone="973-920-4881", Address = new Address{ Street="1970 Hudson Street", City="Jersey City", State="NJ", Zip="07305"}},
                    new Lead { FirstName = "Carol", MiddleName = "J", LastName="Huffman", Email="CarolJHuffman@rhyta.com", Phone="662-266-6706", Address = new Address{ Street="3493 Oxford Court", City="Jackson", State="MS", Zip="39201"}},
                    new Lead { FirstName = "Mattie", MiddleName = "J", LastName="Hudgens", Email="MattieJHudgens@teleworm.us", Phone="903-423-0305", Address = new Address{ Street="3943 Florence Street", City="Dallas", State="TX", Zip="75234"}},
                    new Lead { FirstName = "Steven", MiddleName = "B", LastName="Kelly", Email="StevenBKelly@teleworm.us", Phone="781-962-8285", Address = new Address{ Street="256 Hummingbird Way", City="Woburn", State="MA", Zip="01801"}},
                    new Lead { FirstName = "David", MiddleName = "A", LastName="Guzman", Email="DavidAGuzman@cuvox.de", Phone="412-361-0614", Address = new Address{ Street="3327 Jacobs Street", City="Pittsburgh", State="PA", Zip="15206"}},
                    new Lead { FirstName = "Kimberly", MiddleName = "D", LastName="Clark", Email="KimberlyDClark@fleckens.hu", Phone="432-368-0135", Address = new Address{ Street="4817 Laurel Lane", City="Odessa", State="TX", Zip="79762"}},
                    new Lead { FirstName = "Patrick", MiddleName = "M", LastName="Taylor", Email="PatrickMTaylor@rhyta.com", Phone="636-255-2250", Address = new Address{ Street="4910 Irving Place", City="Saint Charles", State="MO", Zip="63301"}},
                    new Lead { FirstName = "Luz", MiddleName = "E", LastName="Bowen", Email="LuzEBowen@superrito.com", Phone="713-996-0856", Address = new Address{ Street="3439 Parkview Drive", City="Houston", State="TX", Zip="77040"}},
                    new Lead { FirstName = "David", MiddleName = "M", LastName="Orlando", Email="DavidMOrlando@jourrapide.com", Phone="816-278-5785", Address = new Address{ Street="4134 Nutter Street", City="Kansas City", State="MO", Zip="64105"}},
                    new Lead { FirstName = "William", MiddleName = "K", LastName="Davidson", Email="WilliamKDavidson@dayrep.com", Phone="718-307-4438", Address = new Address{ Street="772 Pride Avenue", City="Rego Park Queens", State="NY", Zip="11734"}},
                    new Lead { FirstName = "Raymond", MiddleName = "P", LastName="Hayes", Email="RaymondPHayes@gustr.com", Phone="815-218-9421", Address = new Address{ Street="2454 Emeral Dreams Drive", City="Rockford", State="IL", Zip="61101"}},
                    new Lead { FirstName = "Dolores", MiddleName = "M", LastName="Veras", Email="DoloresMVeras@cuvox.de", Phone="650-276-9178", Address = new Address{ Street="4568 Ella Street", City="San Francisco", State="CA", Zip="94107"}},
                    new Lead { FirstName = "Eva", MiddleName = "J", LastName="Mills", Email="EvaJMills@cuvox.de", Phone="305-878-1708", Address = new Address{ Street="4184 Arbutus Drive", City="Miami", State="FL", Zip="33176"}},
                    new Lead { FirstName = "Joseph", MiddleName = "M", LastName="Reyes", Email="JosephMReyes@einrot.com", Phone="716-751-0871", Address = new Address{ Street="1557 Bottom Lane", City="Wilson", State="NY", Zip="14172"}},
                    new Lead { FirstName = "Jennifer", MiddleName = "E", LastName="Cunningham", Email="JenniferECunningham@jourrapide.com", Phone="928-859-7266", Address = new Address{ Street="791 Farm Meadow Drive", City="Salome", State="AZ", Zip="85348"}},
                    new Lead { FirstName = "Shirley", MiddleName = "R", LastName="Johnson", Email="ShirleyRJohnson@teleworm.us", Phone="307-354-7507", Address = new Address{ Street="3281 Arbor Court", City="Mountain View", State="WY", Zip="82939"}},
                    new Lead { FirstName = "Robert", MiddleName = "P", LastName="Breland", Email="RobertPBreland@superrito.com", Phone="212-695-4708", Address = new Address{ Street="2118 Shinn Street", City="New York", State="NY", Zip="10018"}},
                    new Lead { FirstName = "Debbie", MiddleName = "G", LastName="Redrick", Email="DebbieGRedrick@superrito.com", Phone="484-631-9812", Address = new Address{ Street="1817 Franklee Lane", City="Philadelphia", State="PA", Zip="19108"}},
                    new Lead { FirstName = "Denae", MiddleName = "J", LastName="Grubb", Email="DenaeJGrubb@fleckens.hu", Phone="574-298-6716", Address = new Address{ Street="1038 Villa Drive", City="South Bend", State="IN", Zip="46601"}},
                    new Lead { FirstName = "Julius", MiddleName = "L", LastName="Gaiser", Email="JuliusLGaiser@teleworm.us", Phone="662-644-1141", Address = new Address{ Street="2729 Rafe Lane", City="Jackson", State="MS", Zip="39211"}},
                    new Lead { FirstName = "Glenda", MiddleName = "R", LastName="Franks", Email="GlendaRFranks@cuvox.de", Phone="423-364-2181", Address = new Address{ Street="209 Public Works Drive", City="Chattanooga", State="TN", Zip="37421"}},
                    new Lead { FirstName = "Teresa", MiddleName = "W", LastName="Siegel", Email="TeresaWSiegel@jourrapide.com", Phone="719-389-5226", Address = new Address{ Street="2098 Clover Drive", City="Colorado Springs", State="CO", Zip="80903"}},
                    new Lead { FirstName = "James", MiddleName = "G", LastName="Hall", Email="JamesGHall@rhyta.com", Phone="731-425-4982", Address = new Address{ Street="3277 Melville Street", City="Jackson", State="TN", Zip="38301"}},
                    new Lead { FirstName = "Chuck", MiddleName = "B", LastName="Boerger", Email="ChuckBBoerger@dayrep.com", Phone="325-785-3691", Address = new Address{ Street="2907 Anthony Avenue", City="Rockwood", State="TX", Zip="76873"}},
                    new Lead { FirstName = "Warren", MiddleName = "A", LastName="Chittum", Email="WarrenAChittum@einrot.com", Phone="334-708-6071", Address = new Address{ Street="2680 Franklin Street", City="Dothan", State="AL", Zip="36301"}},
                    new Lead { FirstName = "Carl", MiddleName = "B", LastName="Wang", Email="CarlBWang@teleworm.us", Phone="413-284-6222", Address = new Address{ Street="1055 Leverton Cove Road", City="Palmer", State="MA", Zip="01069"}},
                    new Lead { FirstName = "Todd", MiddleName = "R", LastName="Thompson", Email="ToddRThompson@armyspy.com", Phone="304-439-6934", Address = new Address{ Street="2655 Fulton Street", City="Stonewood", State="WV", Zip="26301"}},
                    new Lead { FirstName = "Megan", MiddleName = "E", LastName="Towler", Email="MeganETowler@jourrapide.com", Phone="361-302-1306", Address = new Address{ Street="3648 Washington Street", City="Corpus Christi", State="TX", Zip="78476"}},
                    new Lead { FirstName = "Jessie", MiddleName = "L", LastName="Kirby", Email="JessieLKirby@cuvox.de", Phone="818-924-2803", Address = new Address{ Street="900 Quiet Valley Lane", City="Los Angeles", State="CA", Zip="90017"}},
                    new Lead { FirstName = "Stanley", MiddleName = "D", LastName="Raymer", Email="StanleyDRaymer@einrot.com", Phone="661-547-7218", Address = new Address{ Street="3491 Lowndes Hill Park Road", City="Santa Fe Springs", State="CA", Zip="90670"}},
                    new Lead { FirstName = "James", MiddleName = "J", LastName="Patterson", Email="JamesJPatterson@cuvox.de", Phone="770-977-9271", Address = new Address{ Street="1882 Smith Road", City="Marietta", State="GA", Zip="30062"}},
                    new Lead { FirstName = "Julian", MiddleName = "W", LastName="Carpenter", Email="JulianWCarpenter@rhyta.com", Phone="417-345-8829", Address = new Address{ Street="610 Lighthouse Drive", City="Buffalo", State="MO", Zip="65685"}},
                    new Lead { FirstName = "Kim", MiddleName = "S", LastName="Kiefer", Email="KimSKiefer@superrito.com", Phone="540-377-6623", Address = new Address{ Street="3244 Hurry Street", City="Raphine", State="VA", Zip="24472"}},
                    new Lead { FirstName = "John", MiddleName = "S", LastName="Bissonnette", Email="JohnSBissonnette@rhyta.com", Phone="843-394-5433", Address = new Address{ Street="4385 Kessla Way", City="Lake City", State="SC", Zip="29560"}},
                    new Lead { FirstName = "Jana", MiddleName = "G", LastName="McCauley", Email="JanaGMcCauley@einrot.com", Phone="269-341-3877", Address = new Address{ Street="81 Shingleton Road", City="Kalamazoo", State="MI", Zip="49007"}},
                };

            leads.ForEach(l => context.Leads.AddOrUpdate(p => p.Email, l));

            context.SaveChanges();
        }

        private void SeedRoles(DataContext context)
        {
            var roleManager = new RoleManager<Role>(new RoleStore<Role>(context));

            roleManager.Create(new Role { Name = "SysAdmin", Description = "Site-wide system administrator" });
            roleManager.Create(new Role { Name = "Administrator", Description = "Org administrator" });
            roleManager.Create(new Role { Name = "User", Description = "Org user" });

            context.SaveChanges();
        }

        private void SeedUsers(DataContext context)
        {
            AddUser(context, "sysadmin@sample.com", "Bill", "West", "SysAdmin");
            AddUser(context, "admin@sample.com", "John", "Davis", "Administrator");
            AddUser(context, "user@sample.com", "Jill", "Smith", "User");
            AddUser(context, "automation@sample.com", "Automation", "TestUser", "SysAdmin");

            context.SaveChanges();
        }

        private void AddUser(DataContext context, string userName, string firstName, string lastName, string primaryRole)
        {
            var user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new User
                {
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordExpiresDate = DateTime.UtcNow.AddDays(60)
                };
                var res = userManager.Create(user, "password");

                if (res.Succeeded)
                {
                    userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, primaryRole));
                    userManager.AddToRole(user.Id, primaryRole);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException dbe)
                    {
                        WriteEntityErrorsAndThrow(dbe);
                    }
                }
            }
            else
            {
                if (!userManager.IsInRole(user.Id, primaryRole))
                {
                    userManager.AddToRole(user.Id, primaryRole);
                }

                if (!user.HasClaim(ClaimTypes.Role, primaryRole))
                    userManager.AddClaim(user.Id, new Claim(ClaimTypes.Role, primaryRole));
            }
        }


        private void WriteEntityErrorsAndThrow(DbEntityValidationException dbe)
        {
            var message = String.Empty;
            dbe.EntityValidationErrors.ToList().ForEach(e =>
            {
                foreach (var err in e.ValidationErrors)
                {
                    string msg = String.Format("{0}.{1}: {2}\r\n", e.Entry.Entity.GetType().Name, err.PropertyName, err.ErrorMessage);
                    Trace.WriteLine(msg);
                    Console.WriteLine(msg);
                    message += msg;
                }
            });
            throw new Exception(message, dbe);
        }
    }
}