using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Zenergy.Models;
using Zenergy.Services;

namespace Zenergy.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var userManager = new UserServices(new ZenergyContext());

            //ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
            user user = await userManager.findByMailAndPassword(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return;
            }

            /*ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);*/

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("mail", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()));
            identity.AddClaim(new Claim("UserId", user.userId.ToString()));

            if (user.member != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Member"));
            }
            if (user.contributor != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Contributor"));
            }
            if (user.manager != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            }
            if (user.admin != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.userId.ToString()));
            identity.AddClaim(new Claim("UserId", user.userId.ToString()));


            if (user.member != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Member"));
            }
            if (user.contributor != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Contributor"));
            }
            if (user.manager != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            }
            if (user.admin != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            AuthenticationProperties properties = CreateProperties(user.userId.ToString(), user.mail);
            AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
            context.Request.Context.Authentication.SignIn(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            context.AdditionalResponseParameters.Add("userID", context.Identity.GetUserId());

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Les informations d'identification du mot de passe du propriétaire de la ressource ne fournissent pas un ID client.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userId, string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userId", userId },
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}