using System;
using System.Collections.Generic;
using BiuBiuServer.Interfaces;
using BiuBiuServer.Services;
using BiuBiuShare.Response;
using MagicOnion;

namespace BiuBiuServer.Tests
{
    public class AccountDatabaseDrivenTest : IAccountDatabaseDriven
    {
        private static readonly
            IDictionary<string, (string Password, long UserId, string
                DisplayName)> _dummyUsers
                = new Dictionary<string, (string, long, string)>(StringComparer
                    .OrdinalIgnoreCase)
                {
                    {"1776137198", ("123456789", 1001, "Eustiana von Astraea")}
                    , {"1250236422", ("123456789", 1002, "Kiruya Momochi")}
                    ,
                };

        private static readonly
            IDictionary<string, (string Password, long UserId, string
                DisplayName)> _adminDummyUsers
                = new Dictionary<string, (string, long, string)>(StringComparer
                    .OrdinalIgnoreCase)
                {
                    {"1250236422", ("123456789", 1002, "Kiruya Momochi")},
                };

        public async UnaryResult<SignInResponse> CommonSign(
            string signInId, string password)
        {
            if (_dummyUsers.TryGetValue(signInId, out var userInfo) &&
                userInfo.Password == password)
            {
                // if判断登录成功后返回对象，对象的Success字段必须是true
                return new SignInResponse()
                {
                    DisplayName = userInfo.DisplayName
                    ,
                    UserId = userInfo.UserId
                    ,
                    Success = true
                };
            }

            ;
            return SignInResponse.Failed;
        }

        public async UnaryResult<SignInResponse> AdministrantSign(
            string signInId, string password)
        {
            if (_adminDummyUsers.TryGetValue(signInId, out var userInfo) &&
                userInfo.Password == password)
            {
                // if判断登录成功后返回对象，对象的Success字段必须是true
                return new SignInResponse()
                {
                    DisplayName = userInfo.DisplayName
                    ,
                    UserId = userInfo.UserId
                    ,
                    Success = true
                };
            }

            return SignInResponse.Failed;
        }
    }
}