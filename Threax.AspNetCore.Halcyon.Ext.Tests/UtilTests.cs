using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Ext.Tests
{
    public class HalcyonExtUtilsTests
    {
        [Fact]
        public void NoRoles()
        {
            var identity = new ClaimsIdentity("Test");

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void NoRolesDeny()
        {
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckSingle()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow"
                }
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckSingleDeny()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "NotValidRole"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow"
                }
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleOr()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                }
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleOrSecond()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "OtherAllow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                }
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleOrDeny()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "NotValidRole"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                }
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleAnd()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));
            identity.AddClaim(new Claim(identity.RoleClaimType, "OtherAllow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow"
                },
                new AuthorizeAttribute()
                {
                    Roles = "OtherAllow"
                }
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleAndDeny()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow"
                },
                new AuthorizeAttribute()
                {
                    Roles = "OtherAllow"
                }
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleCombo()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));
            identity.AddClaim(new Claim(identity.RoleClaimType, "AndAllow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                },
                new AuthorizeAttribute()
                {
                    Roles = "AndAllow"
                }
            };

            Assert.True(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleComboDenyOr()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "AndAllow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                },
                new AuthorizeAttribute()
                {
                    Roles = "AndAllow"
                }
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }

        [Fact]
        public void RoleCheckMultipleComboDenyAnd()
        {
            var identity = new ClaimsIdentity("Test");
            identity.AddClaim(new Claim(identity.RoleClaimType, "Allow"));

            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);

            var attributes = new AuthorizeAttribute[]
            {
                new AuthorizeAttribute()
                {
                    Roles = "Allow,OtherAllow"
                },
                new AuthorizeAttribute()
                {
                    Roles = "AndAllow"
                }
            };

            Assert.False(HalcyonExtUtils.CheckRoles(user, attributes));
        }
    }
}
