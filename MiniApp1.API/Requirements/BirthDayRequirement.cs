using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.API.Requirements
{
    //Policy-Based Authorizationda biz ek business kodları çalıştırabiliyoruz, örnek yaş hesaplaması gibi
    //haricinde claim bazlı yetkilendirme ile aynıdır.
    public class BirthDayRequirement:IAuthorizationRequirement
    {
        public int Age { get; set; }

        public BirthDayRequirement(int age)
        {
            Age = age;
        }
    }


    public class BirthDayRequirementHandler : AuthorizationHandler<BirthDayRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            BirthDayRequirement requirement)
        {
            var birthDate = context.User.FindFirst("Birthday"); //Token Payloadundan doğum tarihini aldık

            if (birthDate == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

                var today = DateTime.Now;
                var age = today.Year- (Convert.ToDateTime(birthDate.Value).Year) ; 


            if (requirement.Age >= age) //Belirlediğimiz yaşa göre endpointe erişim kontrolü
            {
                context.Succeed(requirement);
            }

            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
